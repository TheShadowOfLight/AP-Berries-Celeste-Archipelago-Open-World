using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    public class modMainMenu
    {
        public void Load()
        {
            On.Celeste.OuiMainMenu.Enter += modOuiMainMenu_Enter;
            On.Celeste.MainMenuClimb.Render += modMainMenuClimb_Render;
            On.Celeste.MainMenuClimb.Confirm += modMainMenuClimb_Confirm;
        }

        public void Unload()
        {
            On.Celeste.OuiMainMenu.Enter -= modOuiMainMenu_Enter;
            On.Celeste.MainMenuClimb.Render -= modMainMenuClimb_Render;
            On.Celeste.MainMenuClimb.Confirm -= modMainMenuClimb_Confirm;
        }

        private static System.Collections.IEnumerator modOuiMainMenu_Enter(On.Celeste.OuiMainMenu.orig_Enter orig, OuiMainMenu self, Oui from)
        {
            ArchipelagoManager.Instance.Disconnect();

            yield return orig(self, from);
        }

        private static void modMainMenuClimb_Render(On.Celeste.MainMenuClimb.orig_Render orig, MainMenuClimb self)
        {
            orig(self);
            self.label = "Connect";
        }

        private static void modMainMenuClimb_Confirm(On.Celeste.MainMenuClimb.orig_Confirm orig, MainMenuClimb self)
        {
            (self.Scene as Overworld).Goto<OuiConnection>();
        }
    }
}
