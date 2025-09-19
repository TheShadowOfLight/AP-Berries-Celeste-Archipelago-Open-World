using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modIntroCar : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.IntroCar.Update += modIntroCar_Update;
        }

        public override void Unload()
        {
            On.Celeste.IntroCar.Update -= modIntroCar_Update;
        }

        private static void modIntroCar_Update(On.Celeste.IntroCar.orig_Update orig, IntroCar self)
        {
            orig(self);

            if (self.HasPlayerRider())
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";

                if (ArchipelagoManager.Instance.Carsanity)
                {
                    Celeste_MultiworldModule.SaveData.CarLocations.Add(AP_ID);
                }
            }
        }
    }
}
