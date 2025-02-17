using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    public class modChapterMenu
    {
        public void Load()
        {
            On.Celeste.OuiChapterPanel.IsStart += modOuiChapterPanel_IsStart;
            On.Celeste.OuiChapterPanel.Reset += modOuiChapterPanel_Reset;
        }

        public bool modOuiChapterPanel_IsStart(On.Celeste.OuiChapterPanel.orig_IsStart orig, OuiChapterPanel self, Overworld overworld, Overworld.StartMode start)
        {
            Logger.Error("AP", "AAAAAAAAA IS START");
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            if (SaveData.Instance != null && SaveData.Instance.LastArea_Safe.ID == AreaKey.None.ID)
            {
                SaveData.Instance.LastArea_Safe = AreaKey.Default;
                dynamicUI.Set("instantClose", true);
            }
            if (start == Overworld.StartMode.AreaComplete || start == Overworld.StartMode.AreaQuit)
            {
                AreaData areaData = AreaData.Get(SaveData.Instance.LastArea_Safe.ID);
                string text;
                if (areaData == null)
                {
                    text = null;
                }
                else
                {
                    Meta.MapMeta meta = areaData.Meta;
                    text = ((meta != null) ? meta.Parent : null);
                }
                areaData = AreaData.Get(text) ?? areaData;
                if (areaData != null)
                {
                    SaveData.Instance.LastArea_Safe.ID = areaData.ID;
                }
            }
            bool flag = dynamicUI.Invoke<bool>("orig_IsStart", overworld, start);
            if (true && self.option >= self.options.Count && self.options.Count == 1)
            {
                Logger.Error("AP", "AAAAAAAAA B SIDE");
                self.AddRemixButton();
            }
            if (true && self.option >= self.options.Count && self.options.Count == 2)
            {
                Logger.Error("AP", "AAAAAAAAA C SIDE");
                self.options.Add(new OuiChapterPanel.Option
                {
                    Label = Dialog.Clean("overworld_remix2", null),
                    Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/rmx2")],
                    ID = "C",
                    Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")]
                });
            }
            return true;
        }

        private void modOuiChapterPanel_Reset(On.Celeste.OuiChapterPanel.orig_Reset orig, OuiChapterPanel self)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);

            self.Area = SaveData.Instance.LastArea_Safe;
            self.Data = AreaData.Areas[self.Area.ID];
            self.RealStats = SaveData.Instance.Areas_Safe[self.Area.ID];
            if (SaveData.Instance.CurrentSession_Safe != null && SaveData.Instance.CurrentSession_Safe.OldStats != null && SaveData.Instance.CurrentSession_Safe.Area.ID == self.Area.ID)
            {
                self.DisplayedStats = SaveData.Instance.CurrentSession_Safe.OldStats;
                SaveData.Instance.CurrentSession_Safe = null;
            }
            else
            {
                self.DisplayedStats = self.RealStats;
            }
            self.height = (float)self.GetModeHeight();
            self.modes.Clear();
            bool flag = false;
            if (!self.Data.Interlude_Safe)
            {
                flag = true; // TODO: THIS ENABLES B-SIDES
            }
            bool flag2 = !self.Data.Interlude_Safe && Celeste.PlayMode != Celeste.PlayModes.Event;  // TODO: THIS ENABLES C-SIDES
            self.modes.Add(new OuiChapterPanel.Option
            {
                Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")],
                Label = Dialog.Clean(self.Data.Interlude_Safe ? "FILE_BEGIN" : "overworld_normal", null).ToUpper(),
                Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/play")],
                ID = "A"
            });
            if (flag)
            {
                self.AddRemixButton();
            }
            if (flag2)
            {
                self.modes.Add(new OuiChapterPanel.Option
                {
                    Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")],
                    Label = Dialog.Clean("overworld_remix2", null),
                    Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/rmx2")],
                    ID = "C"
                });
            }
            self.selectingMode = true;
            self.UpdateStats(false, null, null, null);
            self.SetStatsPosition(false);
            for (int i = 0; i < self.options.Count; i++)
            {
                self.options[i].SlideTowards(i, self.options.Count, true);
            }
            self.chapter = Dialog.Get("area_chapter", null).Replace("{x}", self.Area.ChapterIndex.ToString().PadLeft(2));
            self.contentOffset = new Vector2(440f, 120f);
            self.initialized = true;
        }
    }
}
