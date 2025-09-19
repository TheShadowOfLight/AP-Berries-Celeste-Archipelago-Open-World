﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modJellyfish : modItemBase
    {
        public override void Load()
        {
            On.Celeste.Glider.Update += modGlider_Update;
        }

        public override void Unload()
        {
            On.Celeste.Glider.Update -= modGlider_Update;
        }

        private static void modGlider_Update(On.Celeste.Glider.orig_Update orig, Glider self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.Collidable = false;
                self.sprite.Color.R = (byte)(0.3f * 255.0f);
                self.sprite.Color.G = (byte)(0.3f * 255.0f);
                self.sprite.Color.B = (byte)(0.3f * 255.0f);
                self.sprite.Color.A = (byte)(0.3f * 255.0f);
                self.Hold.cannotHoldTimer = 1.0f;
            }
            else if (!self.destroyed)
            {
                self.Collidable = true;
                self.sprite.Color.R = (byte)255;
                self.sprite.Color.G = (byte)255;
                self.sprite.Color.B = (byte)255;
                self.sprite.Color.A = (byte)255;
            }
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12016, out haveReceived);
            return haveReceived;
        }
    }
}
