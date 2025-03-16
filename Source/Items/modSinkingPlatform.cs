using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modSinkingPlatform : modItemBase
    {
        public override void Load()
        {
            On.Celeste.SinkingPlatform.Update += modSinkingPlatform_Update;
        }

        public override void Unload()
        {
            On.Celeste.SinkingPlatform.Update -= modSinkingPlatform_Update;
        }

        private static void modSinkingPlatform_Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
        }

        public static bool HaveReceived()
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1220, out haveReceived);
            return haveReceived;
        }
    }
}
