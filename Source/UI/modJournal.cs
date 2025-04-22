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
            On.Celeste.OuiJournalSpeedrun.ctor += modOuiJournalSpeedrun_ctor;
            On.Celeste.OuiJournalDeaths.ctor += modOuiJournalDeaths_ctor;
        }

        public void Unload()
        {
            On.Celeste.OuiJournalProgress.ctor -= modOuiJournalProgress_ctor;
            On.Celeste.OuiJournalSpeedrun.ctor -= modOuiJournalSpeedrun_ctor;
            On.Celeste.OuiJournalDeaths.ctor -= modOuiJournalDeaths_ctor;
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

        private static void modOuiJournalSpeedrun_ctor(On.Celeste.OuiJournalSpeedrun.orig_ctor orig, OuiJournalSpeedrun self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("ITEMS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f));

            bool haveSpring = false;
            bool haveTrafficBlock = false;
            bool haveDashRefill = false;
            bool haveDoubleDashRefill = false;
            bool havePinkCassetteBlock = false;
            bool haveBlueCassetteBlock = false;
            bool haveYellowCassetteBlock = false;
            bool haveGreenCassetteBlock = false;

            bool haveDream = false;
            bool haveCoins = false;
            bool haveSeeds = false;
            bool haveSinking = false;
            bool haveMoving = false;
            bool haveBlueBooster = false;
            bool haveRedBooster = false;
            bool haveMoveBlock = false;

            bool haveBlueCloud = false;
            bool havePinkCloud = false;
            bool haveWhiteBlock = false;
            bool haveSwapBlock = false;
            bool haveDashSwitch = false;
            bool haveSeekers = false;
            bool haveTheo = false;
            bool haveTorches = false;

            bool haveFeather = false;
            bool haveBumper = false;
            bool haveKevin = false;
            bool haveBadeline = false;

            bool haveCoreBlock = false;
            bool haveCoreToggle = false;
            bool haveFireIce = false;

            bool havePuffer = false;
            bool haveJelly = false;
            bool haveBreaker = false;
            bool haveBird = false;

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12000, out haveSpring);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12001, out haveTrafficBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12018, out haveDashRefill);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12019, out haveDoubleDashRefill);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12002, out havePinkCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12003, out haveBlueCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201A, out haveYellowCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201B, out haveGreenCassetteBlock);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12004, out haveDream);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12005, out haveCoins);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201F, out haveSeeds);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12020, out haveSinking);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12006, out haveMoving);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12007, out haveBlueBooster);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200B, out haveRedBooster);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12009, out haveMoveBlock);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12008, out haveBlueCloud);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12010, out havePinkCloud);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12021, out haveWhiteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200A, out haveSwapBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201C, out haveDashSwitch);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201D, out haveSeekers);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200C, out haveTheo);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12022, out haveTorches);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200D, out haveFeather);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200E, out haveBumper);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200F, out haveKevin);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12011, out haveBadeline);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12014, out haveCoreBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12013, out haveCoreToggle);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12012, out haveFireIce);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12015, out havePuffer);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12016, out haveJelly);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12017, out haveBreaker);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12023, out haveBird);

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            OuiJournalPage.Row row1 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSpring            ? "spring" : "spring_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTrafficBlock      ? "traffic" : "traffic_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDashRefill        ? "dash" : "dash_outline"))
                .Add(new OuiJournalPage.IconsCell(havePinkCassetteBlock ? "pink_cassette" : "pink_cassette_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueCassetteBlock ? "blue_cassette" : "blue_cassette_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDream             ? "dream" : "dream_outline"))
                .Add(new OuiJournalPage.IconsCell(haveCoins             ? "coin" : "coin_outline"))
                .Add(new OuiJournalPage.IconsCell(haveSeeds             ? "seed" : "seed_outline"));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            OuiJournalPage.Row row2 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSinking     ? "sinking" : "sinking_outline"))
                .Add(new OuiJournalPage.IconsCell(haveMoving      ? "moving" : "moving_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueCloud   ? "cloud_blue" : "cloud_blue_outline"))
                .Add(new OuiJournalPage.IconsCell(havePinkCloud   ? "cloud_pink" : "cloud_pink_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueBooster ? "booster_blue" : "booster_blue_outline"))
                .Add(new OuiJournalPage.IconsCell(haveMoveBlock   ? "move_block" : "move_block_outline"))
                .Add(new OuiJournalPage.IconsCell(haveWhiteBlock  ? "white_block" : "white_block_outline"));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            OuiJournalPage.Row row3 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSwapBlock  ? "swap_block" : "swap_block_outline"))
                .Add(new OuiJournalPage.IconsCell(haveRedBooster ? "booster_red" : "booster_red_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDashSwitch ? "dash_switch" : "dash_switch_outline"))
                .Add(new OuiJournalPage.IconsCell(haveSeekers    ? "seeker" : "seeker_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTheo       ? "theo" : "theo_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTorches    ? "torch" : "torch_outline"));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            OuiJournalPage.Row row6 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveFeather  ? "feather" : "feather_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBumper   ? "bumper" : "bumper_outline"))
                .Add(new OuiJournalPage.IconsCell(haveKevin    ? "kevin" : "kevin_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBadeline ? "badeline" : "badeline_outline"));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("9a"))
            {
                self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

                OuiJournalPage.Row row8 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.IconsCell(haveCoreBlock  ? "core_block" : "core_block_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveCoreToggle ? "core_toggle" : "core_toggle_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveFireIce    ? "ice_ball" : "ice_ball_outline"));
            }

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("10a"))
            {
                self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

                OuiJournalPage.Row row9 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.IconsCell(haveDoubleDashRefill    ? "double_dash" : "double_dash_outline"))
                    .Add(new OuiJournalPage.IconsCell(havePuffer              ? "puffer" : "puffer_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveJelly               ? "jelly" : "jelly_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveBreaker             ? "breaker" : "breaker_outline"));

                if (ArchipelagoManager.Instance.ActiveLevels.Contains("10b"))
                {
                    row9.Add(new OuiJournalPage.IconsCell(haveBird                ? "bird" : "bird_outline"))
                        .Add(new OuiJournalPage.IconsCell(haveYellowCassetteBlock ? "yellow_cassette" : "yellow_cassette_outline"))
                        .Add(new OuiJournalPage.IconsCell(haveGreenCassetteBlock  ? "green_cassette" : "green_cassette_outline"));
                }
            }
        }

        private static void modOuiJournalDeaths_ctor(On.Celeste.OuiJournalDeaths.orig_ctor orig, OuiJournalDeaths self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("KEYS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            bool have_3_1 = false;
            bool have_3_2 = false;
            bool have_3_3 = false;
            bool have_3_4 = false;
            bool have_3_5 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16000, out have_3_1);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16001, out have_3_2);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16002, out have_3_3);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16003, out have_3_4);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16004, out have_3_5);

            OuiJournalPage.Row row3 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Celestial Resort A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_3_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_2 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_3 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_4 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_5 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            bool have_5_1 = false;
            bool have_5_2 = false;
            bool have_5_3 = false;
            bool have_5_4 = false;
            bool have_5_5 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16005, out have_5_1);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16006, out have_5_2);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16007, out have_5_3);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16008, out have_5_4);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16009, out have_5_5);

            OuiJournalPage.Row row5 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Mirror Temple A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_5_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_2 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_3 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_4 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_5 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("5b"))
            {
                bool have_5b_1 = false;
                bool have_5b_2 = false;

                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600A, out have_5b_1);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600B, out have_5b_2);

                OuiJournalPage.Row row5b = self.table.AddRow()
                    .Add(new OuiJournalPage.TextCell($"Mirror Temple B", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                    .Add(new OuiJournalPage.IconsCell(have_5b_1 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_5b_2 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f));

                self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));
            }

            bool have_7_1 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600C, out have_7_1);

            OuiJournalPage.Row row7 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"The Summit A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_7_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            bool have_gem_1 = false;
            bool have_gem_2 = false;
            bool have_gem_3 = false;
            bool have_gem_4 = false;
            bool have_gem_5 = false;
            bool have_gem_6 = false;

            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A00, out have_gem_1);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A01, out have_gem_2);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A02, out have_gem_3);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A03, out have_gem_4);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A04, out have_gem_5);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A05, out have_gem_6);

            OuiJournalPage.Row rowGem = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Gems", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_gem_1 ? "gem1" : "gem1_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_2 ? "gem2" : "gem2_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_3 ? "gem3" : "gem3_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_4 ? "gem4" : "gem4_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_5 ? "gem5" : "gem5_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_6 ? "gem6" : "gem6_outline"));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("10a"))
            {
                self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

                bool have_10_1 = false;
                bool have_10_2 = false;
                bool have_10_3 = false;
                bool have_10_4 = false;
                bool have_10_5 = false;

                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600D, out have_10_1);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600E, out have_10_2);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600F, out have_10_3);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16010, out have_10_4);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16011, out have_10_5);

                OuiJournalPage.Row row10 = self.table.AddRow()
                    .Add(new OuiJournalPage.TextCell($"Farewell", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                    .Add(new OuiJournalPage.IconsCell(have_10_1 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_2 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_3 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_4 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_5 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.EmptyCell(64f));
            }
        }
    }
}
