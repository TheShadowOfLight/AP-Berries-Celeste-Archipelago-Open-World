using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.General
{
    internal class modPlayer
    {
        public void Load()
        {
            On.Celeste.Player.Die += modPlayer_Die;
            On.Celeste.Player.Update += modPlayer_Update;
        }

        public void Unload()
        {

        }

        private PlayerDeadBody modPlayer_Die(On.Celeste.Player.orig_Die orig, Player self, Microsoft.Xna.Framework.Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            PlayerDeadBody result = orig(self, direction, evenIfInvincible, registerDeathInStats);

            if (registerDeathInStats)
            {
                ArchipelagoManager.Instance.SendDeathLinkIfEnabled("couldn't climb the mountain");
            }

            return result;
        }

        private void modPlayer_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            orig(self);

            if (ArchipelagoManager.Instance.DeathLinkData != null)
            {
                // TODO: Check for valid status to die
                if (self.InControl)
                {
                    self.Die(Vector2.Zero, true, false);

                    ArchipelagoManager.Instance.ClearDeathLink();
                }
            }
        }
    }
}
