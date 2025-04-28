using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Celeste.Mod.Celeste_Multiworld.UI;

namespace Celeste.Mod.Celeste_Multiworld.General
{
    internal class modPlayer
    {
        public static int HairLength = 4;

        public void Load()
        {
            On.Celeste.Player.Die += modPlayer_Die;
            On.Celeste.Player.Update += modPlayer_Update;
            On.Celeste.PlayerSeeker.Update += modPlayerSeeker_Update;
            On.Celeste.Level.LoadLevel += modLevel_LoadLevel;
            On.Celeste.Level.CompleteArea_bool_bool_bool += modLevel_CompleteArea_bool_bool_bool;
        }

        public void Unload()
        {
            On.Celeste.Player.Die -= modPlayer_Die;
            On.Celeste.Player.Update -= modPlayer_Update;
            On.Celeste.PlayerSeeker.Update -= modPlayerSeeker_Update;
            On.Celeste.Level.LoadLevel -= modLevel_LoadLevel;
            On.Celeste.Level.CompleteArea_bool_bool_bool -= modLevel_CompleteArea_bool_bool_bool;
        }

        private static PlayerDeadBody modPlayer_Die(On.Celeste.Player.orig_Die orig, Player self, Microsoft.Xna.Framework.Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            PlayerDeadBody result = orig(self, direction, evenIfInvincible, registerDeathInStats);

            if (registerDeathInStats && !SaveData.Instance.Assists.Invincible)
            {
                ArchipelagoManager.Instance.SendDeathLinkIfEnabled("couldn't climb the mountain");
            }

            Items.Traps.TrapManager.Instance.AddDeathToActiveTraps();

            return result;
        }

        private static void modPlayer_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            if (Items.Traps.TrapManager.Instance.IsTrapActive(Items.Traps.TrapType.Stun))
            {
                self.Speed = Vector2.Zero;
                self.StateMachine.state = 0;
            }
            else
            {
                orig(self);
            }

            HandleMessageQueue(self);

            if (Items.Traps.TrapManager.Instance.IsTrapActive(Items.Traps.TrapType.Bald) && self.Sprite != null)
            {
                self.Sprite.HairCount = 0;
            }
            else if ((modPlayer.HairLength != 4 || self.Sprite.HairCount != modPlayer.HairLength) && self.Sprite != null)
            {
                self.Sprite.HairCount = modPlayer.HairLength;
            }

            if (ArchipelagoManager.Instance.DeathLinkData != null)
            {
                if (self.InControl && !self.Dead)
                {
                    self.Die(Vector2.Zero, true, false);

                    ArchipelagoManager.Instance.ClearDeathLink();
                }
            }

            if (self.InControl && !self.Dead)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";
                Items.Traps.TrapManager.Instance.AddScreenToActiveTraps(AP_ID);

                if (ArchipelagoManager.Instance.Roomsanity)
                {
                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(AP_ID);
                }
            }
            else if (!self.InControl)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";

                if (AP_ID == "7_0_credits-summit")
                {
                    ArchipelagoManager.Instance.UpdateGameStatus(Archipelago.MultiClient.Net.Enums.ArchipelagoClientState.ClientGoal);
                }
            }
        }

        private static void modPlayerSeeker_Update(On.Celeste.PlayerSeeker.orig_Update orig, PlayerSeeker self)
        {
            orig(self);

            if (ArchipelagoManager.Instance.Roomsanity)
            {
                if (self.enabled)
                {
                    string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";
                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(AP_ID);
                }
            }
        }

        private static void modLevel_LoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            // Fake the B Side Crystal Hearts so that the Golden Strawberries are spawned
            Dictionary<int, bool> realBSideHearts = new Dictionary<int, bool>();
            foreach (AreaStats area in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Areas[area.ID];

                // TODO: This causes B Side Crystal Hearts to look as Ghosts in-level
                if (areaData.HasMode(AreaMode.BSide) && areaData.Mode[(int)AreaMode.BSide].MapData.DetectedHeartGem)
                {
                    realBSideHearts.Add(area.ID, area.Modes[(int)AreaMode.BSide].HeartGem);
                    area.Modes[(int)AreaMode.BSide].HeartGem = true;
                }
            }

            orig(self, playerIntro, isFromLoader);

            // Undo faked B Side Crystal Hearts
            foreach (AreaStats area in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Areas[area.ID];

                if (areaData.HasMode(AreaMode.BSide) && areaData.Mode[(int)AreaMode.BSide].MapData.DetectedHeartGem)
                {
                    area.Modes[(int)AreaMode.BSide].HeartGem = realBSideHearts[area.ID];
                }
            }


            if (self.Session.Area.ID == 2 && self.Session.Area.Mode == 0)
            {
                // Always start Old Site A with Mirror Pre-Broken, for Logic reasons
                self.Session.Inventory.DreamDash = true;
            }

            // Pause UI Entities
            if (ArchipelagoManager.Instance.DeathLinkActive && self.Entities.FindFirst<DeathDisplay>() == null)
            {
                self.Entities.Add(new DeathDisplay());
            }

            self.SaveQuitDisabled = true;
        }

        private static ScreenWipe modLevel_CompleteArea_bool_bool_bool(On.Celeste.Level.orig_CompleteArea_bool_bool_bool orig, Level self, bool spotlightWipe, bool skipScreenWipe, bool skipCompleteScreen)
        {
            if (SaveData.Instance != null)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_Clear";
                Celeste_MultiworldModule.SaveData.LevelClearLocations.Add(AP_ID);
            }

            return orig(self, spotlightWipe, skipScreenWipe, skipCompleteScreen);
        }

        private static bool ShouldShowMessage(ArchipelagoMessage message)
        {
            if (message.Type == ArchipelagoMessage.MessageType.Literature)
            {
                return true;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.Chat)
            {
                return Celeste_MultiworldModule.Settings.ChatMessages;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.Server)
            {
                return Celeste_MultiworldModule.Settings.ServerMessages;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.ItemReceive)
            {
                Celeste_MultiworldModuleSettings.ItemReceiveStyle style = Celeste_MultiworldModule.Settings.ItemReceiveMessages;

                if (!message.Strawberry && (message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.None)
                {
                    return true;
                }
                else if ((message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.Non_Strawberry_Progression)
                {
                    return true;
                }
                else if (style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.Progression)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (message.Type == ArchipelagoMessage.MessageType.ItemSend)
            {
                Celeste_MultiworldModuleSettings.ItemSendStyle style = Celeste_MultiworldModule.Settings.ItemSendMessages;

                if ((message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemSendStyle.None)
                {
                    return true;
                }
                else if (style > Celeste_MultiworldModuleSettings.ItemSendStyle.Progression)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private static void HandleMessageQueue(Player self)
        {
            if (ArchipelagoManager.Instance.MessageLog.Count > 0)
            {
                if (self.Scene.Tracker.GetEntity<modMiniTextbox>() == null)
                {
                    ArchipelagoMessage message = ArchipelagoManager.Instance.MessageLog[0];
                    ArchipelagoManager.Instance.MessageLog.RemoveAt(0);

                    if (ShouldShowMessage(message))
                    {
                        self.Scene.Add(new modMiniTextbox(message.Text, (message.Type == ArchipelagoMessage.MessageType.Literature)));
                        Logger.Verbose("AP", message.Text);
                    }
                }
            }
        }
    }
}
