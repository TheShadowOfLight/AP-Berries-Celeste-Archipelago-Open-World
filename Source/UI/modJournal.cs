using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static MonoMod.InlineRT.MonoModRule;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    internal class modJournal
    {
        Dictionary<int, string> mode_to_str { get; set; } = new Dictionary<int, string>
        {
            { 0, "a" },
            { 1, "b" },
            { 2, "c" },
        };

        public void Load()
        {
            On.Celeste.OuiJournalProgress.ctor += modOuiJournalProgress_ctor;
        }

        public void Unload()
        {
            On.Celeste.OuiJournalProgress.ctor -= modOuiJournalProgress_ctor;
        }

        private static void modOuiJournalProgress_ctor(On.Celeste.OuiJournalProgress.orig_ctor orig, OuiJournalProgress self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("LOCATIONS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false)).AddColumn(new OuiJournalPage.EmptyCell(20f)).AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(100f))
                .AddColumn(new OuiJournalPage.IconCell("strawberry", 150f))
                .AddColumn(new OuiJournalPage.IconCell("checkpoint", 100f))
                .AddColumn(new OuiJournalPage.IconCell("key", 100f))
                .AddColumn(new OuiJournalPage.IconCell("binoculars", 100f))
                .AddColumn(new OuiJournalPage.IconCell("house", 100f));

            int AllLocationsDone = ArchipelagoManager.Instance.LocationsCheckedCount();
            int AllLocationsTotal = ArchipelagoManager.Instance.LocationsTotalCount();

            foreach (AreaStats areaStats in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Get(areaStats.ID_Safe);
                if (!areaData.Interlude_Safe)
                {
                    if (!ArchipelagoManager.Instance.ActiveLevels.Contains($"{areaStats.ID}a"))
                    {
                        continue;
                    }

                    int CheckpointDone = 0;
                    int KeyDone = 0;
                    int BinocularsDone = 0;
                    int RoomDone = 0;
                    int CheckpointTotal = 0;
                    int KeyTotal = 0;
                    int BinocularsTotal = 0;
                    int RoomTotal = 0;

                    foreach (string level in ArchipelagoManager.Instance.ActiveLevels)
                    {
                        string level_area = level.TrimEnd(level[level.Length - 1]);
                        if (level_area == $"{areaStats.ID}")
                        {
                            int mode = 0;

                            if (level.EndsWith("b"))
                            {
                                mode = 1;
                            }
                            else if (level.EndsWith("c"))
                            {
                                mode = 2;
                            }

                            CheckpointDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Checkpoint, areaStats.ID, mode);
                            KeyDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Key, areaStats.ID, mode);
                            BinocularsDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Binoculars, areaStats.ID, mode);
                            RoomDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Room, areaStats.ID, mode);

                            CheckpointTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Checkpoint, areaStats.ID, mode);
                            KeyTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Key, areaStats.ID, mode);
                            BinocularsTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Binoculars, areaStats.ID, mode);
                            RoomTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Room, areaStats.ID, mode);
                        }
                    }

                    // Strawberries
                    string text;
                    if (areaData.Mode[0].TotalStrawberries > 0 || areaStats.TotalStrawberries > 0)
                    {
                        text = areaStats.TotalStrawberries.ToString();
                        if (areaStats.Modes[0].Completed)
                        {
                            text = text + "/" + areaData.Mode[0].TotalStrawberries;
                        }
                    }
                    else
                    {
                        text = "-";
                    }

                    // Level Clear
                    string completionIcon = "dot";
                    if (Celeste_MultiworldModule.SaveData.LevelClearLocations.Contains($"{areaStats.ID}_{0}_Clear"))
                    {
                        if (!AreaData.Get(areaStats.ID_Safe).CanFullClear)
                        {
                            // Farewell
                            completionIcon = "beat";
                        }
                        else
                        {
                            completionIcon = "clear";
                        }
                    }
                    OuiJournalPage.IconsCell iconsCell;
                    OuiJournalPage.Row row = self.table.AddRow().Add(new OuiJournalPage.TextCell(Dialog.Clean(areaData.Name, null), new Vector2(1f, 0.5f), 0.6f, self.TextColor, 0f, false)).Add(null)
                        .Add(iconsCell = new OuiJournalPage.IconsCell(completionIcon));

                    // Crystal Hearts
                    List<string> list = new List<string>();
                    if (areaStats.Modes[0].HeartGem)
                    {
                        list.Add("heartgem" + 0);
                    }
                    for (int i = 1; i < areaStats.Modes.Length; i++)
                    {
                        if (Celeste_MultiworldModule.SaveData.LevelClearLocations.Contains($"{areaStats.ID}_{i}_Clear"))
                        {
                            list.Add("heartgem" + i);
                        }
                    }
                    if (list.Count <= 0)
                    {
                        list.Add("dot");
                    }

                    // Cassette
                    if (areaData.CanFullClear)
                    {
                        row.Add(new OuiJournalPage.IconsCell(new string[] { areaStats.Cassette ? "cassette" : "dot" }));
                        row.Add(new OuiJournalPage.IconsCell(-32f, list.ToArray()));
                    }
                    else
                    {
                        iconsCell.SpreadOverColumns = 3;
                        row.Add(null).Add(null);
                    }
                    row.Add(new OuiJournalPage.TextCell(text, self.TextJustify, 0.5f, self.TextColor, 0f, false));

                    if (CheckpointTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{CheckpointDone} / {CheckpointTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (KeyTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{KeyDone} / {KeyTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (ArchipelagoManager.Instance.Binosanity && BinocularsTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{BinocularsDone} / {BinocularsTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (ArchipelagoManager.Instance.Roomsanity && RoomTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{RoomDone} / {RoomTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }
                }
            }

            // Totals Row
            if (self.table.Rows > 1)
            {
                self.table.AddRow();
                OuiJournalPage.Row row2 = self.table.AddRow().Add(new OuiJournalPage.TextCell(Dialog.Clean("journal_totals", null), new Vector2(1f, 0.5f), 0.7f, self.TextColor, 0f, false)).Add(null)
                    .Add(new OuiJournalPage.TextCell($"{AllLocationsDone} / {AllLocationsTotal}", self.TextJustify, 0.6f, self.TextColor, 100f, true)
                    {
                        SpreadOverColumns = 3
                    });

                for (int l = 0; l < 4; l++)
                {
                    row2.Add(null);
                }

                row2.Add(new OuiJournalPage.TextCell("DEATHS", self.TextJustify, 0.6f, self.TextColor, 70f, true)
                {
                    SpreadOverColumns = 2
                });

                row2.Add(null);

                row2.Add(new OuiJournalPage.TextCell(Dialog.Deaths(SaveData.Instance.TotalDeaths), self.TextJustify, 0.6f, self.TextColor, 0f, false));

                self.table.AddRow();
            }
        }
    }
}
