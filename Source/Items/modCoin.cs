using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modCoin : modItemBase
    {
        Microsoft.Xna.Framework.Color originalColor;
        bool setColor = false;

        public override void Load()
        {
            On.Celeste.TouchSwitch.Render += modTouchSwitch_Render;
            On.Celeste.TouchSwitch.OnPlayer += modTouchSwitch_OnPlayer;
            setColor = false;
        }

        private void modTouchSwitch_Render(On.Celeste.TouchSwitch.orig_Render orig, TouchSwitch self)
        {
            orig(self);

            if (!setColor)
            {
                originalColor = self.inactiveColor;
                setColor = true;
            }

            if (!HaveReceived())
            {
                self.inactiveColor = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.inactiveColor = originalColor;
            }
        }

        private void modTouchSwitch_OnPlayer(On.Celeste.TouchSwitch.orig_OnPlayer orig, TouchSwitch self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        public override void Unload()
        {
            On.Celeste.TouchSwitch.Render -= modTouchSwitch_Render;
            On.Celeste.TouchSwitch.OnPlayer -= modTouchSwitch_OnPlayer;
        }

        public override bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1205, out haveReceived);
            return haveReceived;
        }
    }
}
