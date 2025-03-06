using Celeste.Mod.Celeste_Multiworld.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class APLocationData
    {
        public static List<modLocationBase> modLocations { get; } = new List<modLocationBase>()
        {
            new modStrawberry(),
        };

        public static Dictionary<string, long> CheckpointStringToID { get; set; } = new Dictionary<string, long>
        {
            // Level_Side_Room
            { "1_0_6",        0xCA1400 },
            { "1_0_9b",       0xCA1401 },
                              
            { "1_1_04",       0xCA1402 },
            { "1_1_08",       0xCA1403 },
                              
            { "2_0_3",        0xCA1404 },
            { "2_0_end_3",    0xCA1405 },
                              
            { "2_1_03",       0xCA1406 },
            { "2_1_08b",      0xCA1407 },
                              
            { "3_0_08-a",     0xCA1408 },
            { "3_0_09-d",     0xCA1409 },
            { "3_0_00-d",     0xCA140A },
                              
            { "3_1_06",       0xCA140B },
            { "3_1_11",       0xCA140C },
            { "3_1_16",       0xCA140D },
                              
            { "4_0_b-00",     0xCA140E },
            { "4_0_c-00",     0xCA140F },
            { "4_0_d-00",     0xCA1410 },
                              
            { "4_1_b-00",     0xCA1411 },
            { "4_1_c-00",     0xCA1412 },
            { "4_1_d-00",     0xCA1413 },
                              
            { "5_0_b-00",     0xCA1414 },
            { "5_0_c-00",     0xCA1415 },
            { "5_0_d-00",     0xCA1416 },
            { "5_0_e-00",     0xCA1417 },
                              
            { "5_1_b-00",     0xCA1418 },
            { "5_1_c-00",     0xCA1419 },
            { "5_1_d-00",     0xCA141A },

            { "6_0_00",       0xCA141B },
            { "6_0_04",       0xCA141C },
            { "6_0_b-00",     0xCA141D },
            { "6_0_boss-00",  0xCA141E },
            { "6_0_after-00", 0xCA141F },

            { "6_1_b-00",     0xCA1420 },
            { "6_1_c-00",     0xCA1421 },
            { "6_1_d-00",     0xCA1422 },

            { "7_0_b-00",     0xCA1423 },
            { "7_0_c-00",     0xCA1424 },
            { "7_0_d-00",     0xCA1425 },
            { "7_0_e-00b",    0xCA1426 },
            { "7_0_f-00",     0xCA1427 },
            { "7_0_g-00",     0xCA1428 },

            { "7_1_b-00",     0xCA1429 },
            { "7_1_c-01",     0xCA142A },
            { "7_1_d-00",     0xCA142B },
            { "7_1_e-00",     0xCA142C },
            { "7_1_f-00",     0xCA142D },
            { "7_1_g-00",     0xCA142E },

            { "9_0_a-00",     0xCA142F },
            { "9_0_c-00",     0xCA1430 },
            { "9_0_d-00",     0xCA1431 },

            { "9_1_a-00",     0xCA1432 },
            { "9_1_b-00",     0xCA1433 },
            { "9_1_c-01",     0xCA1434 },

            { "10_0_a-00",    0xCA1435 },
            { "10_0_c-00",    0xCA1436 },
            { "10_0_e-00z",   0xCA1437 },
            { "10_0_f-door",  0xCA1438 },
            { "10_0_h-00b",   0xCA1439 },
            { "10_0_i-00",    0xCA143A },
            { "10_0_j-00",    0xCA143B },
            { "10_0_j-16",    0xCA143C },
        };
        public static Dictionary<long, string> CheckpointIDToString { get; set; } = new Dictionary<long, string>
        {
            // Level_Side_Room
            { 0xCA1400,  "1_0_6" },
            { 0xCA1401,  "1_0_9b" },

            { 0xCA1402,  "1_1_04" },
            { 0xCA1403,  "1_1_08" },

            { 0xCA1404,  "2_0_3" },
            { 0xCA1405,  "2_0_end_3" },

            { 0xCA1406,  "2_1_03" },
            { 0xCA1407,  "2_1_08b" },

            { 0xCA1408,  "3_0_08-a" },
            { 0xCA1409,  "3_0_09-d" },
            { 0xCA140A,  "3_0_00-d" },

            { 0xCA140B,  "3_1_06" },
            { 0xCA140C,  "3_1_11" },
            { 0xCA140D,  "3_1_16" },

            { 0xCA140E,  "4_0_b-00" },
            { 0xCA140F,  "4_0_c-00" },
            { 0xCA1410,  "4_0_d-00" },

            { 0xCA1411,  "4_1_b-00" },
            { 0xCA1412,  "4_1_c-00" },
            { 0xCA1413,  "4_1_d-00" },

            { 0xCA1414,  "5_0_b-00" },
            { 0xCA1415,  "5_0_c-00" },
            { 0xCA1416,  "5_0_d-00" },
            { 0xCA1417,  "5_0_e-00" },

            { 0xCA1418,  "5_1_b-00" },
            { 0xCA1419,  "5_1_c-00" },
            { 0xCA141A,  "5_1_d-00" },

            { 0xCA141B,  "6_0_00" },
            { 0xCA141C,  "6_0_04" },
            { 0xCA141D,  "6_0_b-00" },
            { 0xCA141E,  "6_0_boss-00" },
            { 0xCA141F,  "6_0_after-00" },

            { 0xCA1420,  "6_1_b-00" },
            { 0xCA1421,  "6_1_c-00" },
            { 0xCA1422,  "6_1_d-00" },

            { 0xCA1423,  "7_0_b-00" },
            { 0xCA1424,  "7_0_c-00" },
            { 0xCA1425,  "7_0_d-00" },
            { 0xCA1426,  "7_0_e-00b" },
            { 0xCA1427,  "7_0_f-00" },
            { 0xCA1428,  "7_0_g-00" },

            { 0xCA1429,  "7_1_b-00" },
            { 0xCA142A,  "7_1_c-01" },
            { 0xCA142B,  "7_1_d-00" },
            { 0xCA142C,  "7_1_e-00" },
            { 0xCA142D,  "7_1_f-00" },
            { 0xCA142E,  "7_1_g-00" },

            { 0xCA142F,  "9_0_a-00" },
            { 0xCA1430,  "9_0_c-00" },
            { 0xCA1431,  "9_0_d-00" },

            { 0xCA1432,  "9_1_a-00" },
            { 0xCA1433,  "9_1_b-00" },
            { 0xCA1434,  "9_1_c-01" },

            { 0xCA1435,  "10_0_a-00" },
            { 0xCA1436,  "10_0_c-00" },
            { 0xCA1437,  "10_0_e-00z" },
            { 0xCA1438,  "10_0_f-door" },
            { 0xCA1439,  "10_0_h-00b" },
            { 0xCA143A,  "10_0_i-00" },
            { 0xCA143B,  "10_0_j-00" },
            { 0xCA143C,  "10_0_j-16" },
        };
        public static Dictionary<string, long> StrawberryIDToAP { get; set; } = new Dictionary<string, long>
        {
            // Level_Room:ID
            { "1_2:11",   0xCA1000 },
            { "1_3:9",    0xCA1001 },
            { "1_3b:2",   0xCA1002 },
            { "1_5z:10",  0xCA1003 },
            { "1_5:21",   0xCA1004 },
            { "1_5a:2",   0xCA1005 },
            { "1_6:12",   0xCA1006 },
            { "1_7zb:2",  0xCA1007 },
            { "1_s1:9",   0xCA1008 },
            { "1_7z:3",   0xCA1009 },
            { "1_8zb:1",  0xCA100A },
            { "1_7a:12",  0xCA100B },
            { "1_9z:3",   0xCA100C },
            { "1_8b:1",   0xCA100D },
            { "1_9:14",   0xCA100E },
            { "1_9b:9",   0xCA100F },
            { "1_9c:2",   0xCA1010 },
            { "1_10zb:1", 0xCA1011 },
            { "1_11:9",   0xCA1012 },
            { "1_12z:8",  0xCA1013 },
        };
        public static Dictionary<long, string> StrawberryAPToID { get; set; } = new Dictionary<long, string>
        {
            // Level_Room:ID
            { 0xCA1000, "1_2:11" },
            { 0xCA1001, "1_3:9" },
            { 0xCA1002, "1_3b:2" },
            { 0xCA1003, "1_5z:10" },
            { 0xCA1004, "1_5:21" },
            { 0xCA1005, "1_5a:2" },
            { 0xCA1006, "1_6:12" },
            { 0xCA1007, "1_7zb:2" },
            { 0xCA1008, "1_s1:9" },
            { 0xCA1009, "1_7z:3" },
            { 0xCA100A, "1_8zb:1" },
            { 0xCA100B, "1_7a:12" },
            { 0xCA100C, "1_9z:3" },
            { 0xCA100D, "1_8b:1" },
            { 0xCA100E, "1_9:14" },
            { 0xCA100F, "1_9b:9" },
            { 0xCA1010, "1_9c:2" },
            { 0xCA1011, "1_10zb:1" },
            { 0xCA1012, "1_11:9" },
            { 0xCA1013, "1_12z:8" },
        };
    }
}
