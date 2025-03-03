using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modTorch : modItemBase
    {
        public override void Load()
        {
            On.Celeste.Torch.OnPlayer += modTorch_OnPlayer;
        }

        private void modTorch_OnPlayer(On.Celeste.Torch.orig_OnPlayer orig, Torch self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        public override void Unload()
        {
            On.Celeste.Torch.OnPlayer -= modTorch_OnPlayer;
        }

        public override bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1222, out haveReceived);
            return haveReceived;
        }
    }
}
