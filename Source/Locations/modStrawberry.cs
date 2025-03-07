using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    public class modStrawberry : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.Strawberry.OnCollect += modStrawberry_OnCollect;
            On.Celeste.SaveData.CheckStrawberry_EntityID += modSaveData_CheckStrawberry_EntityID;
        }

        public override void Unload()
        {
            On.Celeste.Strawberry.OnCollect -= modStrawberry_OnCollect;
            On.Celeste.SaveData.CheckStrawberry_EntityID -= modSaveData_CheckStrawberry_EntityID;
        }

        private void modStrawberry_OnCollect(On.Celeste.Strawberry.orig_OnCollect orig, Strawberry self)
        {
            orig(self);
            string strawberryString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";

            Celeste_MultiworldModule.SaveData.StrawberryLocations.Add(strawberryString);
            Logger.Error("AP", strawberryString);
        }

        private bool modSaveData_CheckStrawberry_EntityID(On.Celeste.SaveData.orig_CheckStrawberry_EntityID orig, SaveData self, EntityID strawberry)
        {
            string AP_ID = $"{self.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{strawberry}";
            return Celeste_MultiworldModule.SaveData.StrawberryLocations.Contains(AP_ID);
        }
    }
}
