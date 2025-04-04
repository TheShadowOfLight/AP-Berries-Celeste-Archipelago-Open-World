using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modKevinBlock : modItemBase
    {
        private static bool bNeedResetFace = false;

        public override void Load()
        {
            On.Celeste.CrushBlock.Render += modCrushBlock_Render;
            On.Celeste.CrushBlock.CanActivate += modCrushBlock_CanActivate;
        }

        public override void Unload()
        {
            On.Celeste.CrushBlock.Render -= modCrushBlock_Render;
            On.Celeste.CrushBlock.CanActivate -= modCrushBlock_CanActivate;
        }

        private static void modCrushBlock_Render(On.Celeste.CrushBlock.orig_Render orig, CrushBlock self)
        {
            if (!HaveReceived())
            {
                self.face.Play("hurt", false, false);
                bNeedResetFace = true;
            }

            if (HaveReceived() && bNeedResetFace)
            {
                self.face.Play("idle", false, false);
                bNeedResetFace = false;
            }

            orig(self);
        }

        private static bool modCrushBlock_CanActivate(On.Celeste.CrushBlock.orig_CanActivate orig, CrushBlock self, Microsoft.Xna.Framework.Vector2 direction)
        {
            if (!HaveReceived())
            {
                return false;
            }
            else
            {
                return orig(self, direction);
            }
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA120F, out haveReceived);
            return haveReceived;
        }
    }
}
