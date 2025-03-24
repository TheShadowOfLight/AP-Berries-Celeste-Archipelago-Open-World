using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public struct CheckpointItemData
    {
        public int Area;
        public int Mode;
        public string Room;

        public CheckpointItemData(int area, int mode, string room)
        {
            Area = area;
            Mode = mode;
            Room = room;
        }
    };

    internal class APItemData
    {
        public static List<modItemBase> modItems { get; } = new List<modItemBase>()
        {
            new modSpring(),
            new modTrafficBlock(),
            new modRefill(),
            new modCassetteBlock(),

            new modDreamBlock(),
            new modCoin(),
            new modStrawberrySeeds(),

            new modSinkingPlatform(),
            new modMovingPlatform(),

            new modCloud(),
            new modBooster(),
            new modMoveBlock(),
            new modWhiteBlock(),

            new modSwapBlock(),
            new modDashSwitch(),
            new modTorch(),
            new modSeeker(),
            new modTheoCrystal(),
        };

        public static Dictionary<long, string> ItemIDToString { get; set; } = new Dictionary<long, string>
        {
            { 0xCA1000, "Strawberry" },
            { 0xCA1001, "Raspberry" },


            { 0xCA1400, "Forsaken City A - Crossing" },
            { 0xCA1401, "Forsaken City A - Chasm" },
            { 0xCA1402, "Forsaken City B - Contraption" },
            { 0xCA1403, "Forsaken City B - Scrap Pit" },
            { 0xCA1404, "Old Site A - Intervention" },
            { 0xCA1405, "Old Site A - Awake" },
            { 0xCA1406, "Old Site B - Combination Lock" },
            { 0xCA1407, "Old Site B - Dream Altar" },
            { 0xCA1408, "Celestial Resort A - Huge Mess" },
            { 0xCA1409, "Celestial Resort A - Elevator Shaft" },
            { 0xCA140A, "Celestial Resort A - Presidential Suite" },
            { 0xCA140B, "Celestial Resort B - Staff Quarters" },
            { 0xCA140C, "Celestial Resort B - Library" },
            { 0xCA140D, "Celestial Resort B - Rooftop" },
            { 0xCA140E, "Golden Ridge A - Shrine" },
            { 0xCA140F, "Golden Ridge A - Old Trail" },
            { 0xCA1410, "Golden Ridge A - Cliff Face" },
            { 0xCA1411, "Golden Ridge B - Stepping Stones" },
            { 0xCA1412, "Golden Ridge B - Gusty Canyon" },
            { 0xCA1413, "Golden Ridge B - Eye of the Storm" },
            { 0xCA1414, "Mirror Temple A - Depths" },
            { 0xCA1415, "Mirror Temple A - Unravelling" },
            { 0xCA1416, "Mirror Temple A - Search" },
            { 0xCA1417, "Mirror Temple A - Rescue" },
            { 0xCA1418, "Mirror Temple B - Central Chamber" },
            { 0xCA1419, "Mirror Temple B - Through the Mirror" },
            { 0xCA141A, "Mirror Temple B - Mix Master" },


            { 0xCA1200, "Springs" },
            { 0xCA1201, "Traffic Blocks" },
            { 0xCA1202, "Pink Cassette Blocks" },
            { 0xCA1203, "Blue Cassette Blocks" },
            { 0xCA1204, "Dream Blocks" },
            { 0xCA1205, "Coins" },
            { 0xCA1206, "Moving Platforms" },
            { 0xCA1207, "Blue Boosters" },
            { 0xCA1208, "Blue Clouds" },
            { 0xCA1209, "Move Blocks" },
            { 0xCA120A, "Swap Blocks" },
            { 0xCA120B, "Red Boosters" },
            { 0xCA120C, "Theo Crystal" },
            { 0xCA120D, "Feathers" },
            { 0xCA120E, "Bumpers" },
            { 0xCA120F, "Kevins" },
            { 0xCA1210, "Pink Clouds" },
            { 0xCA1211, "Badeline Boosters" },
            { 0xCA1212, "Fire and Ice Balls" },
            { 0xCA1213, "Core Toggles" },
            { 0xCA1214, "Core Blocks" },
            { 0xCA1215, "Pufferfish" },
            { 0xCA1216, "Jellyfish" },
            { 0xCA1217, "Breaker Boxes" },
            { 0xCA1218, "Dash Refills" },
            { 0xCA1219, "Double Dash Refills" },
            { 0xCA121A, "Yellow Cassette Blocks" },
            { 0xCA121B, "Green Cassette Blocks" },
            { 0xCA121C, "Dash Switches" },
            { 0xCA121D, "Seekers" },
            { 0xCA121F, "Strawberry Seeds" },
            { 0xCA1220, "Sinking Platforms" },
            { 0xCA1221, "White Block" },
            { 0xCA1222, "Torches" },
            { 0xCA1223, "Bird" },

            { 0xCA1600, "Celestial Resort A - Front Door Key" },
            { 0xCA1601, "Celestial Resort A - Hallway Key 1" },
            { 0xCA1602, "Celestial Resort A - Hallway Key 2" },
            { 0xCA1603, "Celestial Resort A - Huge Mess Key" },
            { 0xCA1604, "Celestial Resort A - Presidential Suite Key" },
            { 0xCA1605, "Mirror Temple A - Entrance Key" },
            { 0xCA1606, "Mirror Temple A - Depths Key" },
            { 0xCA1607, "Mirror Temple A - Search Key 1" },
            { 0xCA1608, "Mirror Temple A - Search Key 2" },
            { 0xCA1609, "Mirror Temple A - Search Key 3" },
            { 0xCA160A, "Mirror Temple B - Central Chamber Key 1" },
            { 0xCA160B, "Mirror Temple B - Central Chamber Key 2" },
        };

        public static Dictionary<long, CheckpointItemData> CheckpointData { get; set; } = new Dictionary<long, CheckpointItemData>
        {
            { 0xCA1400, new CheckpointItemData(1, 0, "6") },
            { 0xCA1401, new CheckpointItemData(1, 0, "9b") },

            { 0xCA1402, new CheckpointItemData(1, 1, "04") },
            { 0xCA1403, new CheckpointItemData(1, 1, "08") },

            { 0xCA1404, new CheckpointItemData(2, 0, "3") },
            { 0xCA1405, new CheckpointItemData(2, 0, "end_3") },

            { 0xCA1406, new CheckpointItemData(2, 1, "03") },
            { 0xCA1407, new CheckpointItemData(2, 1, "08b") },

            { 0xCA1408, new CheckpointItemData(3, 0, "08-a") },
            { 0xCA1409, new CheckpointItemData(3, 0, "09-d") },
            { 0xCA140A, new CheckpointItemData(3, 0, "00-d") },

            { 0xCA140B, new CheckpointItemData(3, 1, "06") },
            { 0xCA140C, new CheckpointItemData(3, 1, "11") },
            { 0xCA140D, new CheckpointItemData(3, 1, "16") },

            { 0xCA140E, new CheckpointItemData(4, 0, "b-00") },
            { 0xCA140F, new CheckpointItemData(4, 0, "c-00") },
            { 0xCA1410, new CheckpointItemData(4, 0, "d-00") },

            { 0xCA1411, new CheckpointItemData(4, 1, "b-00") },
            { 0xCA1412, new CheckpointItemData(4, 1, "c-00") },
            { 0xCA1413, new CheckpointItemData(4, 1, "d-00") },

            { 0xCA1414, new CheckpointItemData(5, 0, "b-00") },
            { 0xCA1415, new CheckpointItemData(5, 0, "c-00") },
            { 0xCA1416, new CheckpointItemData(5, 0, "d-00") },
            { 0xCA1417, new CheckpointItemData(5, 0, "e-00") },

            { 0xCA1418, new CheckpointItemData(5, 1, "b-00") },
            { 0xCA1419, new CheckpointItemData(5, 1, "c-00") },
            { 0xCA141A, new CheckpointItemData(5, 1, "d-00") },

            { 0xCA141B, new CheckpointItemData(6, 0, "04") },
            { 0xCA141C, new CheckpointItemData(6, 0, "b-00") },
            { 0xCA141D, new CheckpointItemData(6, 0, "boss-00") },
            { 0xCA141E, new CheckpointItemData(6, 0, "after-00") },

            { 0xCA141F, new CheckpointItemData(6, 1, "b-00") },
            { 0xCA1420, new CheckpointItemData(6, 1, "c-00") },
            { 0xCA1421, new CheckpointItemData(6, 1, "d-00") },

            { 0xCA1422, new CheckpointItemData(7, 0, "b-00") },
            { 0xCA1423, new CheckpointItemData(7, 0, "c-00") },
            { 0xCA1424, new CheckpointItemData(7, 0, "d-00") },
            { 0xCA1425, new CheckpointItemData(7, 0, "e-00b") },
            { 0xCA1426, new CheckpointItemData(7, 0, "f-00") },
            { 0xCA1427, new CheckpointItemData(7, 0, "g-00") },

            { 0xCA1428, new CheckpointItemData(7, 1, "b-00") },
            { 0xCA1429, new CheckpointItemData(7, 1, "c-01") },
            { 0xCA142A, new CheckpointItemData(7, 1, "d-00") },
            { 0xCA142B, new CheckpointItemData(7, 1, "e-00") },
            { 0xCA142C, new CheckpointItemData(7, 1, "f-00") },
            { 0xCA142D, new CheckpointItemData(7, 1, "g-00") },

            { 0xCA142E, new CheckpointItemData(9, 0, "a-00") },
            { 0xCA142F, new CheckpointItemData(9, 0, "c-00") },
            { 0xCA1430, new CheckpointItemData(9, 0, "d-00") },

            { 0xCA1431, new CheckpointItemData(9, 1, "a-00") },
            { 0xCA1432, new CheckpointItemData(9, 1, "b-00") },
            { 0xCA1433, new CheckpointItemData(9, 1, "c-01") },

            { 0xCA1434, new CheckpointItemData(10, 0, "a-00") },
            { 0xCA1435, new CheckpointItemData(10, 0, "c-00") },
            { 0xCA1436, new CheckpointItemData(10, 0, "e-00z") },
            { 0xCA1437, new CheckpointItemData(10, 0, "f-door") },
            { 0xCA1438, new CheckpointItemData(10, 0, "h-00b") },
            { 0xCA1439, new CheckpointItemData(10, 0, "i-00") },
            { 0xCA143A, new CheckpointItemData(10, 0, "j-00") },
            { 0xCA143B, new CheckpointItemData(10, 0, "j-16") },
        };
    }
}
