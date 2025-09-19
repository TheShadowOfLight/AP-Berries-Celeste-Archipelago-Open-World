﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modStrawberrySeeds : modItemBase
    {
        public override void Load()
        {
            On.Celeste.StrawberrySeed.Awake += modStrawberrySeed_Awake;
            On.Celeste.StrawberrySeed.Update += modStrawberrySeed_Update;
            On.Celeste.StrawberrySeed.OnPlayer += modStrawberrySeed_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.StrawberrySeed.Awake -= modStrawberrySeed_Awake;
            On.Celeste.StrawberrySeed.Update -= modStrawberrySeed_Update;
            On.Celeste.StrawberrySeed.OnPlayer -= modStrawberrySeed_OnPlayer;
        }

        private static void modStrawberrySeed_Awake(On.Celeste.StrawberrySeed.orig_Awake orig, StrawberrySeed self, Monocle.Scene scene)
        {
            orig(self, scene);

            self.sprite.OnFrameChange = delegate (string s)
            {
                if (HaveReceived() && self.Visible && self.sprite.CurrentAnimationID == "idle" && self.sprite.CurrentAnimationFrame == 19)
                {
                    Audio.Play("event:/game/general/seed_pulse", self.Position, "count", (float)self.index);
                    self.lightTween.Start();
                    self.level.Displacement.AddBurst(self.Position, 0.6f, 8f, 20f, 0.2f, null, null);
                }
            };
        }

        private static void modStrawberrySeed_Update(On.Celeste.StrawberrySeed.orig_Update orig, StrawberrySeed self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.sprite.Position = self.Position;
            }
        }

        private static void modStrawberrySeed_OnPlayer(On.Celeste.StrawberrySeed.orig_OnPlayer orig, StrawberrySeed self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201F, out haveReceived);
            return haveReceived;
        }
    }
}
