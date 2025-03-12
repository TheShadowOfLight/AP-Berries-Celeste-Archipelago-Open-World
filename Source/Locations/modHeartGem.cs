using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modHeartGem : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.HeartGem.Collect += modHeartGem_Collect;
        }

        public override void Unload()
        {
            On.Celeste.HeartGem.Collect -= modHeartGem_Collect;
        }

        private void modHeartGem_Collect(On.Celeste.HeartGem.orig_Collect orig, HeartGem self, Player player)
        {
            orig(self, player);

            if (SaveData.Instance.CurrentSession_Safe.Area.Mode == AreaMode.Normal)
            {
                string crystalHeartString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_CrystalHeart";

                Celeste_MultiworldModule.SaveData.CrystalHeartLocations.Add(crystalHeartString);
            }
        }
    }
}
