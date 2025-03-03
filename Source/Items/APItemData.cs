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
            new modCassetteBlock(),

            new modDreamBlock(),
            new modCoin(),

            new modTorch(),
        };

        public static Dictionary<long, string> ItemIDToString { get; set; } = new Dictionary<long, string>
        {
            { 0xCA1000, "Strawberry" },
            { 0xCA1001, "Raspberry" },


            { 0xCA1100, "Forsaken City A - Crossing" },
            { 0xCA1101, "Forsaken City A - Chasm" },

            { 0xCA1108, "Forsaken City B - Contraption" },
            { 0xCA1109, "Forsaken City B - Scrap Pit" },

            { 0xCA1110, "Old Site A - Intervention" },
            { 0xCA1111, "Old Site A - Awake" },

            { 0xCA1118, "Old Site B - Combination Lock" },
            { 0xCA1119, "Old Site B - Dream Altar" },

            { 0xCA1120, "Celestial Resort A - Huge Mess" },
            { 0xCA1121, "Celestial Resort A - Elevator Shaft" },
            { 0xCA1122, "Celestial Resort A - Presidential Suite" },

            { 0xCA1128, "Celestial Resort B - Staff Quarters" },
            { 0xCA1129, "Celestial Resort B - Library" },
            { 0xCA112A, "Celestial Resort B - Rooftop" },

            { 0xCA1130, "Golden Ridge A - Shrine" },
            { 0xCA1131, "Golden Ridge A - Old Trail" },
            { 0xCA1132, "Golden Ridge A - Cliff Face" },

            { 0xCA1138, "Golden Ridge B - Stepping Stones" },
            { 0xCA1139, "Golden Ridge B - Gusty Canyon" },
            { 0xCA113A, "Golden Ridge B - Eye of the Storm" },

            { 0xCA1140, "Mirror Temple A - Depths" },
            { 0xCA1141, "Mirror Temple A - Unravelling" },
            { 0xCA1142, "Mirror Temple A - Search" },
            { 0xCA1143, "Mirror Temple A - Rescue" },

            { 0xCA1148, "Mirror Temple B - Central Chamber" },
            { 0xCA1149, "Mirror Temple B - Through the Mirror" },
            { 0xCA114A, "Mirror Temple B - Mix Master" },

            { 0xCA1150, "Reflection A - Lake" },
            { 0xCA1151, "Reflection A - Hollows" },
            { 0xCA1152, "Reflection A - Reflection" },
            { 0xCA1153, "Reflection A - Rock Bottom" },
            { 0xCA1154, "Reflection A - Resolution" },

            { 0xCA1158, "Reflection B - Reflection" },
            { 0xCA1159, "Reflection B - Rock Bottom" },
            { 0xCA115A, "Reflection B - Reprieve" },

            { 0xCA1160, "The Summit A - 500 M" },
            { 0xCA1161, "The Summit A - 1000 M" },
            { 0xCA1162, "The Summit A - 1500 M" },
            { 0xCA1163, "The Summit A - 2000 M" },
            { 0xCA1164, "The Summit A - 2500 M" },
            { 0xCA1165, "The Summit A - 3000 M" },

            { 0xCA1168, "The Summit B - 500 M" },
            { 0xCA1169, "The Summit B - 1000 M" },
            { 0xCA116A, "The Summit B - 1500 M" },
            { 0xCA116B, "The Summit B - 2000 M" },
            { 0xCA116C, "The Summit B - 2500 M" },
            { 0xCA116D, "The Summit B - 3000 M" },

            { 0xCA1170, "Core A - Into the Core" },
            { 0xCA1171, "Core A - Hot and Cold" },
            { 0xCA1172, "Core A - Heart of the Mountain" },

            { 0xCA1178, "Core B - Into the Core" },
            { 0xCA1179, "Core B - Burning or Freezing" },
            { 0xCA117A, "Core B - Heartbeat" },

            { 0xCA1180, "Farewell - Singular" },
            { 0xCA1181, "Farewell - Power Source" },
            { 0xCA1182, "Farewell - Remembered" },
            { 0xCA1183, "Farewell - Event Horizon" },
            { 0xCA1184, "Farewell - Determination" },
            { 0xCA1185, "Farewell - Stubbornness" },
            { 0xCA1186, "Farewell - Reconcilliation" },
            { 0xCA1187, "Farewell - Farewell" },


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
            { 0xCA121E, "Keys" },
            { 0xCA121F, "Strawberry Seeds" },
            { 0xCA1220, "Sinking Platforms" },
            { 0xCA1221, "White Block" },
            { 0xCA1222, "Torches" },
            { 0xCA1223, "Bird" },
        };

        public static Dictionary<long, CheckpointItemData> CheckpointData { get; set; } = new Dictionary<long, CheckpointItemData>
        {
            { 0xCA1100, new CheckpointItemData(1, 0, "6") },
            { 0xCA1101, new CheckpointItemData(1, 0, "9b") },

            { 0xCA1108, new CheckpointItemData(1, 1, "04") },
            { 0xCA1109, new CheckpointItemData(1, 1, "08") },

            { 0xCA1110, new CheckpointItemData(2, 0, "3") },
            { 0xCA1111, new CheckpointItemData(2, 0, "end_3") },

            { 0xCA1118, new CheckpointItemData(2, 1, "03") },
            { 0xCA1119, new CheckpointItemData(2, 1, "08b") },

            { 0xCA1120, new CheckpointItemData(3, 0, "08-a") },
            { 0xCA1121, new CheckpointItemData(3, 0, "09-d") },
            { 0xCA1122, new CheckpointItemData(3, 0, "00-d") },

            { 0xCA1128, new CheckpointItemData(3, 1, "06") },
            { 0xCA1129, new CheckpointItemData(3, 1, "11") },
            { 0xCA112A, new CheckpointItemData(3, 1, "16") },

            { 0xCA1130, new CheckpointItemData(4, 0, "b-00") },
            { 0xCA1131, new CheckpointItemData(4, 0, "c-00") },
            { 0xCA1132, new CheckpointItemData(4, 0, "d-00") },

            { 0xCA1138, new CheckpointItemData(4, 1, "b-00") },
            { 0xCA1139, new CheckpointItemData(4, 1, "c-00") },
            { 0xCA113A, new CheckpointItemData(4, 1, "d-00") },

            { 0xCA1140, new CheckpointItemData(5, 0, "b-00") },
            { 0xCA1141, new CheckpointItemData(5, 0, "c-00") },
            { 0xCA1142, new CheckpointItemData(5, 0, "d-00") },
            { 0xCA1143, new CheckpointItemData(5, 0, "e-00") },

            { 0xCA1148, new CheckpointItemData(5, 1, "b-00") },
            { 0xCA1149, new CheckpointItemData(5, 1, "c-00") },
            { 0xCA114A, new CheckpointItemData(5, 1, "d-00") },

            { 0xCA1150, new CheckpointItemData(6, 0, "00") },
            { 0xCA1151, new CheckpointItemData(6, 0, "04") },
            { 0xCA1152, new CheckpointItemData(6, 0, "b-00") },
            { 0xCA1153, new CheckpointItemData(6, 0, "boss-00") },
            { 0xCA1154, new CheckpointItemData(6, 0, "after-00") },

            { 0xCA1158, new CheckpointItemData(6, 1, "b-00") },
            { 0xCA1159, new CheckpointItemData(6, 1, "c-00") },
            { 0xCA115A, new CheckpointItemData(6, 1, "d-00") },

            { 0xCA1160, new CheckpointItemData(7, 0, "b-00") },
            { 0xCA1161, new CheckpointItemData(7, 0, "c-00") },
            { 0xCA1162, new CheckpointItemData(7, 0, "d-00") },
            { 0xCA1163, new CheckpointItemData(7, 0, "e-00b") },
            { 0xCA1164, new CheckpointItemData(7, 0, "f-00") },
            { 0xCA1165, new CheckpointItemData(7, 0, "g-00") },

            { 0xCA1168, new CheckpointItemData(7, 1, "b-00") },
            { 0xCA1169, new CheckpointItemData(7, 1, "c-01") },
            { 0xCA116A, new CheckpointItemData(7, 1, "d-00") },
            { 0xCA116B, new CheckpointItemData(7, 1, "e-00") },
            { 0xCA116C, new CheckpointItemData(7, 1, "f-00") },
            { 0xCA116D, new CheckpointItemData(7, 1, "g-00") },

            { 0xCA1170, new CheckpointItemData(9, 0, "a-00") },
            { 0xCA1171, new CheckpointItemData(9, 0, "c-00") },
            { 0xCA1172, new CheckpointItemData(9, 0, "d-00") },

            { 0xCA1178, new CheckpointItemData(9, 1, "a-00") },
            { 0xCA1179, new CheckpointItemData(9, 1, "b-00") },
            { 0xCA117A, new CheckpointItemData(9, 1, "c-01") },

            { 0xCA1180, new CheckpointItemData(10, 0, "a-00") },
            { 0xCA1181, new CheckpointItemData(10, 0, "c-00") },
            { 0xCA1182, new CheckpointItemData(10, 0, "e-00z") },
            { 0xCA1183, new CheckpointItemData(10, 0, "f-door") },
            { 0xCA1184, new CheckpointItemData(10, 0, "h-00b") },
            { 0xCA1185, new CheckpointItemData(10, 0, "i-00") },
            { 0xCA1186, new CheckpointItemData(10, 0, "j-00") },
            { 0xCA1187, new CheckpointItemData(10, 0, "j-16") },
        };
    }
}
