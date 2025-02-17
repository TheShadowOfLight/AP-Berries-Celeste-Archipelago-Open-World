using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modTrafficBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.ZipMover.Render += modZipMover_Render;
            On.Celeste.ZipMover.Update += modZipMover_Update;
        }

        public override void Unload()
        {
            On.Celeste.ZipMover.Render -= modZipMover_Render;
            On.Celeste.ZipMover.Update -= modZipMover_Update;
        }

        public override bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201, out haveReceived);
            return haveReceived;
        }

        private void modZipMover_Render(On.Celeste.ZipMover.orig_Render orig, ZipMover self)
        {
            orig(self);

            self.Visible = HaveReceived();
        }

        private void modZipMover_Update(On.Celeste.ZipMover.orig_Update orig, ZipMover self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
            else
            {
                //Audio.Play(SFX.game_03_forcefield_bump);
            }
        }
    }
}
