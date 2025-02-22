using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modDreamBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.DreamBlock.Render += modDreamBlock_Render;
            On.Celeste.DreamBlock.Update += modDreamBlock_Update;
            On.Celeste.Player.DreamDashCheck += modPlayer_DreamDashCheck;
        }

        public override void Unload()
        {
            On.Celeste.DreamBlock.Render -= modDreamBlock_Render;
            On.Celeste.DreamBlock.Update -= modDreamBlock_Update;
        }

        public override bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1204, out haveReceived);
            return haveReceived;
        }

        private void modDreamBlock_Render(On.Celeste.DreamBlock.orig_Render orig, DreamBlock self)
        {
            orig(self);

            self.DisableLightsInside = !HaveReceived();
        }

        private void modDreamBlock_Update(On.Celeste.DreamBlock.orig_Update orig, DreamBlock self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
        }

        private bool modDreamBlock_BlockedCheck(On.Celeste.DreamBlock.orig_BlockedCheck orig, DreamBlock self)
        {
            if (HaveReceived())
            {
                return orig(self);
            }
            else
            {
                return false;
            }
        }

        private bool modPlayer_DreamDashCheck(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Microsoft.Xna.Framework.Vector2 dir)
        {
            if (HaveReceived())
            {
                return orig(self, dir);
            }
            else
            {
                return false;
            }
        }
    }
}
