using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class APItemData
    {
        public static List<modItemBase> modItems { get; } = new List<modItemBase>()
        {
            new modSpring(),
        };

        public static Dictionary<long, string> ItemIDToString { get; set; } = new Dictionary<long, string>
        {
            { 0xCA1000, "Strawberry" },
            { 0xCA1200, "Springs" },
        };
    }
}
