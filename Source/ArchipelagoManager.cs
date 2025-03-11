using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;




namespace Celeste.Mod.Celeste_Multiworld
{
    public struct ArchipelagoMessage
    {
        public string Text { get; init; } = "";
        public int RemainingTime { get; set; } = 300;

        public ArchipelagoMessage(string text)
        {
            Text = text;
        }
    }


    public class ArchipelagoManager : DrawableGameComponent
    {
        public static ArchipelagoManager Instance { get; private set; }

        private static readonly Version _supportedArchipelagoVersion = new(7, 7, 7);

        private ArchipelagoSession _session;
        private DeathLinkService _deathLinkService;
        private DateTime _lastDeath;

        public bool GoalSent = false;

        public DeathLink DeathLinkData { get; private set; }
        public int DeathsCounted = 0;
        public bool IsDeathLinkSafe { get; set; }
        public bool Ready { get; private set; }
        public List<Tuple<int, ItemInfo>> ItemQueue { get; private set; } = new();
        public List<long> CollectedLocations { get; private set; } = new();
        public Dictionary<long, ItemInfo> LocationDictionary { get; private set; } = new();
        public HashSet<long> SentLocations { get; set; } = [];
        public List<ArchipelagoMessage> MessageLog { get; set; } = new();

        public int Slot => _session.ConnectionInfo.Slot;
        public bool DeathLink => _session.ConnectionInfo.Tags.Contains("DeathLink");
        public int HintPoints => _session.RoomState.HintPoints;
        public int HintCost => _session.RoomState.HintCost;
        public Hint[] Hints => _session.DataStorage.GetHints();

        #region Slot Data
        public int StrawberriesRequired { get; set; }
        public bool DeathLinkActive { get; set; }
        public int DeathLinkAmnesty { get; set; }
        public bool Roomsanity = false;
        public bool IncludeGoldens = false;
        public bool IncludeCore = false;
        public bool IncludeFarewell = false;
        public bool IncludeBSides = false;
        public bool IncludeCSides = false;
        #endregion

        public ArchipelagoManager(Game game) : base(game)
        {
            game.Components.Add(this);
            Instance = this;
        }

        public override void Update(GameTime gameTime)
        {
            if (Ready)
            {
                CheckReceivedItemQueue();
                CheckLocationsToSend();
                HandleCollectedLocations();
            }
        }

        public async Task<LoginFailure> TryConnect()
        {
            _lastDeath = DateTime.MinValue;
            _session = ArchipelagoSessionFactory.CreateSession(Celeste_MultiworldModule.Settings.Address);

            // (Re-)initialize state.
            DeathLinkData = null;
            IsDeathLinkSafe = false;
            Ready = false;
            ItemQueue = new();
            LocationDictionary = new();

            // Watch for the following events.
            _session.Socket.ErrorReceived += OnError;
            _session.Socket.PacketReceived += OnPacketReceived;
            _session.MessageLog.OnMessageReceived += OnMessageReceived;
            _session.Items.ItemReceived += OnItemReceived;
            _session.Locations.CheckedLocationsUpdated += OnLocationReceived;

            // Attempt to connect to the server.
            try
            {
                await _session.ConnectAsync();
            }
            catch (Exception ex)
            {
                Disconnect();
                return new($"Unable to establish an initial connection to the Archipelago server @ {Celeste_MultiworldModule.Settings.Address} : {ex.Message}");
            }

            var result = await _session.LoginAsync(
                "Celeste",
                Celeste_MultiworldModule.Settings.SlotName,
                ItemsHandlingFlags.AllItems,
                _supportedArchipelagoVersion,
                uuid: Guid.NewGuid().ToString(),
                password: Celeste_MultiworldModule.Settings.Password
            );

            if (!result.Successful)
            {
                Disconnect();
                return result as LoginFailure;
            }

            // Load randomizer data.
            object value;
            int normalHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_one_dash_hair_color", out value) ? value : 0xdb2c00);
            int twoDashHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_two_dash_hair_color", out value) ? value : 0xfa91ff);
            int noDashHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_no_dash_hair_color", out value) ? value : 0x6ec0ff);
            int featherHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_feather_hair_color", out value) ? value : 0xf2d450);
            Player.NormalHairColor = new Microsoft.Xna.Framework.Color((normalHairInt >> 16) & 0xFF, (normalHairInt >> 8) & 0xFF, (normalHairInt) & 0xFF);
            Player.TwoDashesHairColor = new Microsoft.Xna.Framework.Color((twoDashHairInt >> 16) & 0xFF, (twoDashHairInt >> 8) & 0xFF, (twoDashHairInt) & 0xFF);
            Player.UsedHairColor = new Microsoft.Xna.Framework.Color((noDashHairInt >> 16) & 0xFF, (noDashHairInt >> 8) & 0xFF, (noDashHairInt) & 0xFF);
            Player.FlyPowerHairColor = new Microsoft.Xna.Framework.Color((featherHairInt >> 16) & 0xFF, (featherHairInt >> 8) & 0xFF, (featherHairInt) & 0xFF);

            StrawberriesRequired = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("strawberries_required", out value) ? value : 100);
            DeathLinkActive = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("death_link", out value) ? value : false);
            DeathLinkAmnesty = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("death_link_amnesty", out value) ? value : 10);
            Roomsanity = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("roomsanity", out value) ? value : false);
            IncludeGoldens = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_goldens", out value) ? value : false);
            IncludeCore = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_core", out value) ? value : false);
            IncludeFarewell = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_farewell", out value) ? value : false);
            IncludeBSides = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_b_sides", out value) ? value : false);
            IncludeCSides = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_c_sides", out value) ? value : false);
            bool DeathLinkEnabled = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("death_link", out value) ? value : false);

            // Initialize DeathLink service.
            _deathLinkService = _session.CreateDeathLinkService();
            _deathLinkService.OnDeathLinkReceived += OnDeathLink;
            if (DeathLinkEnabled)
            {
                _deathLinkService.EnableDeathLink();
            }

            // TODO: Wrap this and only do if active
            //AddPlayerListCallback($"Celeste_OtherPlayers_List", PlayerListUpdated);

            // Build dictionary of locations with item information for fast lookup.
            await BuildLocationDictionary();

            // Return null to signify no error.
            Ready = true;
            return null;
        }

        public void Disconnect()
        {
            Ready = false;
            SentLocations.Clear();

            // Clear DeathLink events.
            if (_deathLinkService != null)
            {
                _deathLinkService.OnDeathLinkReceived -= OnDeathLink;
                _deathLinkService = null;
            }

            // Clear events and session object.
            if (_session != null)
            {
                _session.Socket.ErrorReceived -= OnError;
                _session.Items.ItemReceived -= OnItemReceived;
                _session.Locations.CheckedLocationsUpdated -= OnLocationReceived;
                _session.Socket.PacketReceived -= OnPacketReceived;
                _session.Socket.DisconnectAsync(); // It'll disconnect on its own time.
                _session = null;
            }
        }

        private void OnDeathLink(DeathLink deathLink)
        {
            // If we receive a DeathLink that is after our last death, let's set it.
            if (!IsDeathLinkSafe && DateTime.Compare(deathLink.Timestamp, _lastDeath) > 0)
            {
                DeathLinkData = deathLink;
            }
        }

        public void ClearDeathLink()
        {
            DeathLinkData = null;
            DeathsCounted = 0;
        }

        public void SendDeathLinkIfEnabled(string cause)
        {
            // Do not send any DeathLink messages if it's not enabled.
            if (!DeathLink)
            {
                return;
            }

            DeathsCounted = DeathsCounted + 1;
            if (DeathsCounted < DeathLinkAmnesty)
            {
                return;
            }

            DeathsCounted = 0;

            // Log our current time so we can make sure we ignore our own DeathLink.
            _lastDeath = DateTime.Now;
            cause = $"{_session.Players.GetPlayerAlias(Slot)} {cause}.";

            try
            {
                _deathLinkService.SendDeathLink(new(_session.Players.GetPlayerAlias(Slot), cause));
            }
            catch (ArchipelagoSocketClosedException)
            {
                // TODO: Send a message to the client that connection has been dropped.
                Disconnect();
            }

            ClearDeathLink();
        }

        public void CheckLocations(long[] locations)
        {
            foreach (var locationID in locations)
            {
                SentLocations.Add(locationID);
            }

            try
            {
                _session.Locations.CompleteLocationChecks(locations);
            }
            catch (ArchipelagoSocketClosedException)
            {
                // TODO: Send a message to the client that connection has been dropped.
                Disconnect();
            }
        }
        public void UpdateGameStatus(ArchipelagoClientState state)
        {
            SendPacket(new StatusUpdatePacket { Status = state });
        }

        public string GetPlayerName(int slot)
        {
            if (slot == 0)
            {
                return "Archipelago";
            }

            var name = _session.Players.GetPlayerAlias(slot);
            return string.IsNullOrEmpty(name) ? $"Unknown Player {slot}" : name;
        }

        public string GetLocationName(long location)
        {
            var name = _session.Locations.GetLocationNameFromId(location);
            return string.IsNullOrEmpty(name) ? $"Unknown Location {location}" : name;
        }

        public string GetItemName(long item)
        {
            var name = _session.Items.GetItemName(item);
            return string.IsNullOrEmpty(name) ? $"Unknown Item {item}" : name;
        }

        public void EnableDeathLink()
        {
            _deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink()
        {
            _deathLinkService.DisableDeathLink();
        }

        public int LocationsCheckedCount()
        {
            return _session.Locations.AllLocationsChecked.Count();
        }

        private void SendPacket(ArchipelagoPacketBase packet)
        {
            try
            {
                _session.Socket.SendPacket(packet);
            }
            catch (ArchipelagoSocketClosedException)
            {
                // TODO: Send a message to the client that connection has been dropped.
                Disconnect();
            }
        }

        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            var i = helper.Index;
            while (helper.Any())
            {
                ItemQueue.Add(new(i++, helper.DequeueItem()));
            }
        }

        private void OnLocationReceived(ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (var newLoc in newCheckedLocations)
            {
                CollectedLocations.Add(newLoc);
            }
        }

        private async Task BuildLocationDictionary()
        {
            var locations = await _session.Locations.ScoutLocationsAsync(false, _session.Locations.AllLocations.ToArray());

            foreach (var item in locations)
            {
                LocationDictionary[item.Key] = item.Value;
            }
        }

        private void OnMessageReceived(LogMessage message)
        {
            switch (message)
            {
                case ItemSendLogMessage:
                    ItemSendLogMessage itemSendMessage = (ItemSendLogMessage)message;

                    if (itemSendMessage.IsRelatedToActivePlayer && !itemSendMessage.IsReceiverTheActivePlayer)
                    {
                        MessageLog.Add(new ArchipelagoMessage(message.ToString()));
                    }
                    break;
                case CountdownLogMessage:
                case GoalLogMessage:
                    MessageLog.Add(new ArchipelagoMessage(message.ToString()));
                    break;
            }
        }

        private static void OnError(Exception exception, string message)
        {
            Logger.Error("AP", message);
        }

        public void CheckReceivedItemQueue()
        {
            SaveData.Instance.TotalStrawberries_Safe = Celeste_MultiworldModule.SaveData.Strawberries;
            int audioGuard = 0;
            if (Celeste_MultiworldModule.SaveData == null)
            {
                return;
            }

            for (int index = Celeste_MultiworldModule.SaveData.ItemRcv; index < ItemQueue.Count; index++)
            {
                var item = ItemQueue[index].Item2;

                Logger.Info("AP", $"Received {Items.APItemData.ItemIDToString[item.ItemId]} from {GetPlayerName(item.Player)}.");
                MessageLog.Add(new ArchipelagoMessage($"Received {Items.APItemData.ItemIDToString[item.ItemId]} from {GetPlayerName(item.Player)}."));

                switch (item.ItemId)
                {
                    case 0xCA1000:
                    {
                        Celeste_MultiworldModule.SaveData.Strawberries += 1;
                        break;
                    }
                    case long id when id >= 0xCA1400 && id < 0xCA1500:
                    {
                        Items.CheckpointItemData cp_data = Items.APItemData.CheckpointData[id];
                        SaveData.Instance.Areas_Safe[cp_data.Area].Modes[cp_data.Mode].Checkpoints.Add(cp_data.Room);

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_07_checkpointconfetti);
                        }
                        break;
                    }
                    case long id when id >= 0xCA1200 && id <= 0xCA1230:
                    {
                        Celeste_MultiworldModule.SaveData.Interactables[id] = true;

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_gen_secret_revealed);
                        }
                        break;
                    }
                }

                Celeste_MultiworldModule.SaveData.ItemRcv = index + 1;
            }
        }

        public void CheckLocationsToSend()
        {
            List<long> locationsToCheck = new List<long>();
            foreach (KeyValuePair<string, long> checkpointIDPair in Locations.APLocationData.CheckpointStringToID)
            {
                if (Celeste_MultiworldModule.SaveData.CheckpointLocations.Contains(checkpointIDPair.Key))
                {
                    long locationID = checkpointIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> strawberryIDPair in Locations.APLocationData.StrawberryIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.StrawberryLocations.Contains(strawberryIDPair.Key))
                {
                    long locationID = strawberryIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> roomIDPair in Locations.APLocationData.RoomNameToAP)
            {
                if (Celeste_MultiworldModule.SaveData.RoomLocations.Contains(roomIDPair.Key))
                {
                    long locationID = roomIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }

            CheckLocations(locationsToCheck.ToArray());
        }

        public void HandleCollectedLocations()
        {
            foreach (long newLoc in CollectedLocations)
            {
                if (Locations.APLocationData.CheckpointIDToString.ContainsKey(newLoc))
                {
                    string checkpointLocString = Locations.APLocationData.CheckpointIDToString[newLoc];

                    Celeste_MultiworldModule.SaveData.CheckpointLocations.Add(checkpointLocString);
                }

                if (Locations.APLocationData.StrawberryAPToID.ContainsKey(newLoc))
                {
                    string strawberryLocString = Locations.APLocationData.StrawberryAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.StrawberryLocations.Add(strawberryLocString);

                    string[] area_mode_levelEntityID = strawberryLocString.Split(new char[]{'_'}, 3);
                    int area = Int32.Parse(area_mode_levelEntityID[0]);
                    int mode = Int32.Parse(area_mode_levelEntityID[1]);
                    string[] level_EntityID = area_mode_levelEntityID[2].Split(":");
                    int ID = Int32.Parse(level_EntityID[1]);

                    // TODO: This count is getting doubled on a single-session (finish level, all berries counted twice)
                    SaveData.Instance.Areas_Safe[area].Modes[mode].TotalStrawberries += 1;
                    SaveData.Instance.Areas_Safe[area].Modes[mode].Strawberries.Add(new EntityID(level_EntityID[0], ID));
                }

                if (Locations.APLocationData.APToRoomName.ContainsKey(newLoc))
                {
                    string roomLocString = Locations.APLocationData.APToRoomName[newLoc];

                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(roomLocString);
                }
            }

            CollectedLocations.Clear();
        }

        //public void HandleMessageQueue(Batcher batch, SpriteFont font, Rect bounds)
        //{
        //    for (int i = Math.Min(Math.Max(4, MessageLog.Count - 1), 4); i >= 0; i--)
        //    {
        //        if (MessageLog.Count > i)
        //        {
        //            batch.Text(font, Game.Instance.ArchipelagoManager.MessageLog[i].Text, bounds.BottomLeft, new Vec2(0, 5 - i), new Foster.Framework.Color(0xF5, 0x42, 0xC8, 0xFF));
        //            ArchipelagoMessage updatedMessage = Game.Instance.ArchipelagoManager.MessageLog[i];
        //            updatedMessage.RemainingTime -= 1;
        //            if (updatedMessage.RemainingTime <= 0)
        //            {
        //                Game.Instance.ArchipelagoManager.MessageLog.RemoveAt(i);
        //            }
        //            else
        //            {
        //                Game.Instance.ArchipelagoManager.MessageLog[i] = updatedMessage;
        //            }
        //        }
        //    }
        //}

        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            if (packet.PacketType == ArchipelagoPacketType.Retrieved)
            {
                //if (_connectionInfo.SeeGhosts)
                {
                    RetrievedPacket retPacket = packet as RetrievedPacket;

                    foreach (KeyValuePair<string, JToken> entry in retPacket.Data)
                    {
                        if (entry.Key.StartsWith("Celeste_OtherPlayer_"))
                        {
                            //PlayerUpdated(entry.Key, entry.Value);
                        }
                    }
                }
            }
        }
    }
}
