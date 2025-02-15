using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modSpring: modItemBase
    {
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

        public override bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.Springs;
        }

        private void modSpring_Render(On.Celeste.Spring.orig_Render orig, Spring self)
        {
            orig(self);

            HandleSprite(self.sprite);
        }

        private void modSpring_OnCollide(On.Celeste.Spring.orig_OnCollide orig, Spring self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
            else
            {
                //Audio.Play(SFX.game_03_forcefield_bump);
            }
        }
    }
}
