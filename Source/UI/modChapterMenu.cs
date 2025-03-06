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
            On.Celeste.OuiChapterPanel.Start += modOuiChapterPanel_Start;
            On.Celeste.OuiChapterPanel.Swap += modOuiChapterPanel_Swap;
            On.Celeste.OuiChapterPanel.UpdateStats += modOuiChapterPanel_UpdateStats;
            On.Celeste.OuiChapterPanel.GetModeHeight += modOuiChapterPanel_GetModeHeight;

            On.Celeste.SaveData.SetCheckpoint += modSaveData_SetCheckpoint;
        }

        private void modOuiChapterPanel_Start(On.Celeste.OuiChapterPanel.orig_Start orig, OuiChapterPanel self, string checkpoint)
        {
            if (self.Area.Mode == AreaMode.BSide && !ArchipelagoManager.Instance.IncludeBSides)
            {
                Audio.Play("event:/ui/main/button_back");
            }
            else
            {
                orig(self, checkpoint);
            }
        }

        private void modOuiChapterPanel_Swap(On.Celeste.OuiChapterPanel.orig_Swap orig, OuiChapterPanel self)
        {
            if (self.Area.Mode == AreaMode.BSide && !ArchipelagoManager.Instance.IncludeBSides)
            {
                Audio.Play("event:/ui/main/button_back");
            }
            else
            {
                orig(self);
            }
        }

        private bool modSaveData_SetCheckpoint(On.Celeste.SaveData.orig_SetCheckpoint orig, SaveData self, AreaKey area, string level)
        {
            string checkpointString = area.ID.ToString() + "_" + ((int)area.Mode).ToString() + "_" + level;

            if (Celeste_MultiworldModule.SaveData.CheckpointLocations.Contains(checkpointString))
            {
                return false;
            }

            Celeste_MultiworldModule.SaveData.CheckpointLocations.Add(checkpointString);
            return true;
        }

        private HashSet<string> modSaveData_GetCheckpoints(On.Celeste.SaveData.orig_GetCheckpoints orig, SaveData self, AreaKey area)
        {
            HashSet<string> checkpoints = new HashSet<string>();

            ModeProperties modeProperties = AreaData.Areas[area.ID].Mode[(int)area.Mode];
            if (modeProperties.Checkpoints != null)
            {
                foreach (CheckpointData checkpointData in modeProperties.Checkpoints)
                {
                    checkpoints.Add(checkpointData.Level);
                }
            }
            checkpoints.RemoveWhere((string a) => !modeProperties.Checkpoints.Any((CheckpointData b) => b.Level == a));

            return checkpoints;
        }

        public bool modOuiChapterPanel_IsStart(On.Celeste.OuiChapterPanel.orig_IsStart orig, OuiChapterPanel self, Overworld overworld, Overworld.StartMode start)
        {
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
                self.AddRemixButton();
            }
            if (true && self.option >= self.options.Count && self.options.Count == 2)
            {
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
            if (!self.Data.Interlude_Safe && self.Area.ID < 10)
            {
                flag = ArchipelagoManager.Instance.IncludeBSides || ArchipelagoManager.Instance.IncludeCSides;
            }
            bool flag2 = !self.Data.Interlude_Safe && Celeste.PlayMode != Celeste.PlayModes.Event && self.Area.ID < 10 && ArchipelagoManager.Instance.IncludeCSides;
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

        private void modOuiChapterPanel_UpdateStats(On.Celeste.OuiChapterPanel.orig_UpdateStats orig, OuiChapterPanel self, bool wiggle, bool? overrideStrawberryWiggle, bool? overrideDeathWiggle, bool? overrideHeartWiggle)
        {
            // TODO: Make this orig function use AP Location berries instead of the save data ones

            orig(self, wiggle, overrideStrawberryWiggle, overrideDeathWiggle, overrideHeartWiggle);

            self.strawberries.ShowOutOf = self.Area.Mode == AreaMode.Normal;
            self.strawberries.Visible = !self.Data.Interlude_Safe;
            self.deaths.Visible = !self.Data.Interlude_Safe;
        }

        private int modOuiChapterPanel_GetModeHeight(On.Celeste.OuiChapterPanel.orig_GetModeHeight orig, OuiChapterPanel self)
        {
            AreaModeStats areaModeStats = self.RealStats.Modes[(int)self.Area.Mode];
            bool flag = self.Data.Interlude_Safe;
            if (!self.Data.Interlude_Safe && ((areaModeStats.Deaths > 0 && self.Area.Mode != AreaMode.Normal) || areaModeStats.Completed || areaModeStats.HeartGem))
            {
                flag = false;
            }
            if (!flag)
            {
                return 540;
            }
            return 300;
        }
    }
}
