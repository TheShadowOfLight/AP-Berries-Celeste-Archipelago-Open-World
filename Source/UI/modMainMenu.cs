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
            On.Celeste.OuiTitleScreen.ctor += modOuiTitleScreen_ctor;
            On.Celeste.OuiMainMenu.Enter += modOuiMainMenu_Enter;
            On.Celeste.MainMenuClimb.Render += modMainMenuClimb_Render;
            On.Celeste.MainMenuClimb.Confirm += modMainMenuClimb_Confirm;
        }

        public void Unload()
        {
            On.Celeste.OuiTitleScreen.ctor -= modOuiTitleScreen_ctor;
            On.Celeste.OuiMainMenu.Enter -= modOuiMainMenu_Enter;
            On.Celeste.MainMenuClimb.Render -= modMainMenuClimb_Render;
            On.Celeste.MainMenuClimb.Confirm -= modMainMenuClimb_Confirm;
        }

        private static void modOuiTitleScreen_ctor(On.Celeste.OuiTitleScreen.orig_ctor orig, OuiTitleScreen self)
        {
            orig(self);

            int major = ArchipelagoManager._modVersion / 10000;
            int minor = (ArchipelagoManager._modVersion / 100) % 100;
            int bugfix = ArchipelagoManager._modVersion % 100;
            self.version += string.Format("\nArchipelago Open World v{0}.{1}.{2}", major, minor, bugfix);
        }

        private static System.Collections.IEnumerator modOuiMainMenu_Enter(On.Celeste.OuiMainMenu.orig_Enter orig, OuiMainMenu self, Oui from)
        {
            ArchipelagoManager.Instance.Disconnect(false);

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
