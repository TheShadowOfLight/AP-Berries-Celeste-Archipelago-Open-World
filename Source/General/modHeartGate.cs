using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.General
{
    internal class modHeartGate
    {
        public void Load()
        {
            On.Celeste.HeartGemDoor.Added += modHeartGemDoor_Added;
        }

        public void Unload()
        {
            On.Celeste.HeartGemDoor.Added -= modHeartGemDoor_Added;
        }

        private void modHeartGemDoor_Added(On.Celeste.HeartGemDoor.orig_Added orig, HeartGemDoor self, Monocle.Scene scene)
        {
            (scene as Level).Session.SetFlag("opened_heartgem_door_" + self.Requires);

            orig(self, scene);
        }
    }
}
