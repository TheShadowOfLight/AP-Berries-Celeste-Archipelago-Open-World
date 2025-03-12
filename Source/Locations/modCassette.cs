using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modCassette : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.Cassette.OnPlayer += modCassette_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.Cassette.OnPlayer -= modCassette_OnPlayer;
        }

        private void modCassette_OnPlayer(On.Celeste.Cassette.orig_OnPlayer orig, Cassette self, Player player)
        {
            orig(self, player);

            string cassetteString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_Cassette";

            Celeste_MultiworldModule.SaveData.CassetteLocations.Add(cassetteString);
        }
    }
}
