using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modTheoCrystal : modItemBase
    {
        public override void Load()
        {
            On.Celeste.TheoCrystal.Update += modTheoCrystal_Update;
        }

        public override void Unload()
        {
            On.Celeste.TheoCrystal.Update += modTheoCrystal_Update;
        }

        private static void modTheoCrystal_Update(On.Celeste.TheoCrystal.orig_Update orig, TheoCrystal self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.Collidable = false;
                self.sprite.Color.A = (byte)0.3f;
                self.Hold.cannotHoldTimer = 1.0f;
            }
            else
            {
                self.Collidable = true;
                self.sprite.Color.A = (byte)1.0f;
            }
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA120C, out haveReceived);
            return haveReceived;
        }
    }
}
