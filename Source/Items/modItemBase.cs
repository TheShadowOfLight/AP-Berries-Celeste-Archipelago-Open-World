using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public abstract class modItemBase
    {
        public abstract void Load();
        public abstract void Unload();

        public abstract bool HaveReceived();
    }
}
