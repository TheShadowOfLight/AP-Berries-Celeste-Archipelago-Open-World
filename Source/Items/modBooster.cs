using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modBooster : modItemBase
    {
        internal enum BoosterColor
        {
            Blue = 0xCA1207,
            Red = 0xCA120B,
        }

        public override void Load()
        {
            On.Celeste.Booster.Render += modBooster_Render;
            On.Celeste.Booster.Update += modBooster_Update;
        }

        public override void Unload()
        {
            On.Celeste.Booster.Render -= modBooster_Render;
            On.Celeste.Booster.Update -= modBooster_Update;
        }

        private static void modBooster_Render(On.Celeste.Booster.orig_Render orig, Booster self)
        {
            if (!HaveReceived(self.red ? BoosterColor.Red : BoosterColor.Blue))
            {
                self.sprite.Visible = false;
                self.outline.Visible = true;
                self.respawnTimer = 1.0f;
            }

            orig(self);
        }

        private static void modBooster_Update(On.Celeste.Booster.orig_Update orig, Booster self)
        {
            orig(self);

            if (HaveReceived(self.red ? BoosterColor.Red : BoosterColor.Blue))
            {
                self.Collidable = true;
            }
            else
            {
                self.Collidable = false;
            }
        }

        public static bool HaveReceived(BoosterColor color)
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue((int)color, out haveReceived);
            return haveReceived;
        }
    }
}
