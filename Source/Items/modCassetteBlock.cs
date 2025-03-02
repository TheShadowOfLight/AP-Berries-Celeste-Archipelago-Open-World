using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modCassetteBlock : modItemBase
    {
        internal enum BlockColor
        {
            Pink = 0xCA1202,
            Blue = 0xCA1203,
            Yellow = 0xCA121A,
            Green = 0xCA121B
        }

        public override void Load()
        {
            On.Celeste.CassetteBlock.Update += modCassetteBlock_Update;
        }

        private void modCassetteBlock_Update(On.Celeste.CassetteBlock.orig_Update orig, CassetteBlock self)
        {
            BlockColor bc = BlockColor.Pink;
            if (self.color.R == 240)
            {
                bc = BlockColor.Pink;
            }
            else if (self.color.R == 73)
            {
                bc = BlockColor.Blue;
            }
            else if (self.color.R == 252)
            {
                bc = BlockColor.Yellow;
            }
            else if (self.color.R == 56)
            {
                bc = BlockColor.Green;
            }

            if (HaveReceived(bc))
            {
                orig(self);
            }
            else
            {
                if (self.Activated)
                {
                    self.ShiftSize(-1);
                    self.SetActivatedSilently(false);
                }
            }
        }

        public override void Unload()
        {
            On.Celeste.CassetteBlock.Update -= modCassetteBlock_Update;
        }
        public override bool HaveReceived()
        {
            bool haveReceived = false;
            return haveReceived;
        }

        internal bool HaveReceived(BlockColor color)
        {
            bool haveReceived = false;
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue((long)color, out haveReceived);
            return haveReceived;
        }
    }
}
