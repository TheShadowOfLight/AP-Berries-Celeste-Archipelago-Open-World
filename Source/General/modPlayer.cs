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
            On.Celeste.Level.LoadLevel += modLevel_LoadLevel;
            On.Celeste.Level.End += modLevel_End;
        }

        public void Unload()
        {
            On.Celeste.Player.Die -= modPlayer_Die;
            On.Celeste.Player.Update -= modPlayer_Update;
            On.Celeste.Level.LoadLevel -= modLevel_LoadLevel;
            On.Celeste.Level.End -= modLevel_End;
        }

        private static PlayerDeadBody modPlayer_Die(On.Celeste.Player.orig_Die orig, Player self, Microsoft.Xna.Framework.Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            PlayerDeadBody result = orig(self, direction, evenIfInvincible, registerDeathInStats);

            if (registerDeathInStats && !SaveData.Instance.Assists.Invincible)
            {
                ArchipelagoManager.Instance.SendDeathLinkIfEnabled("couldn't climb the mountain");
            }

            return result;
        }

        private static void modPlayer_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            orig(self);

            if (modPlayer.HairLength != 4 && self.Sprite != null)
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

            if (ArchipelagoManager.Instance.Roomsanity)
            {
                if (self.InControl && !self.Dead)
                {
                    string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";
                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(AP_ID);
                }
            }
        }

        private static void modLevel_LoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            orig(self, playerIntro, isFromLoader);

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
        }

        private static void modLevel_End(On.Celeste.Level.orig_End orig, Level self)
        {
            orig(self);

            string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_Clear";
            Celeste_MultiworldModule.SaveData.LevelClearLocations.Add(AP_ID);
        }
    }
}
