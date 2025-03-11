using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modSpring : modItemBase
    {
        static Microsoft.Xna.Framework.Color originalColor;
        static bool setColor = false;

        public override void Load()
        {
            On.Celeste.Spring.Render += modSpring_Render;
            On.Celeste.Spring.OnCollide += modSpring_OnCollide;
        }

        public override void Unload()
        {
            On.Celeste.Spring.Render -= modSpring_Render;
            On.Celeste.Spring.OnCollide -= modSpring_OnCollide;
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200, out haveReceived);
            return haveReceived;
        }

        private static void modSpring_Render(On.Celeste.Spring.orig_Render orig, Spring self)
        {
            orig(self);

            if (!setColor)
            {
                originalColor = self.sprite.Color;
                setColor = true;
            }

            if (!HaveReceived())
            {
                self.sprite.Color = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.sprite.Color = originalColor;
            }
        }

        private static void modSpring_OnCollide(On.Celeste.Spring.orig_OnCollide orig, Spring self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }
    }
}
