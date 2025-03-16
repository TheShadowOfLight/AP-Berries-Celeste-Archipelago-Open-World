using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modLockBlock : modLocationBase
    {
        static Key animKey = null;

        static Dictionary<string, string> lockToKey = new Dictionary<string, string>()
        {
            { "3_0_s3:16", "3_0_s3:15" },
            { "3_0_02-a:9", "3_0_02-b:32" },
            { "3_0_07-a:11", "3_0_07-b:2" },
            { "3_0_09-b:2", "3_0_09-b:13" },
            { "3_0_04-c:17", "3_0_02-c:1" },
        };

        public override void Load()
        {
            On.Celeste.LockBlock.OnPlayer += modLockBlock_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.LockBlock.OnPlayer -= modLockBlock_OnPlayer;
        }

        private static void modLockBlock_OnPlayer(On.Celeste.LockBlock.orig_OnPlayer orig, LockBlock self, Player player)
        {
            if (!self.opening)
            {
                string Lock_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";
                Logger.Error("AP", Lock_ID);

                if (lockToKey.ContainsKey(Lock_ID) && Celeste_MultiworldModule.SaveData.KeyLocations.Contains(lockToKey[Lock_ID]))
                {
                    if (modLockBlock.animKey == null || modLockBlock.animKey.IsUsed)
                    {
                        modLockBlock.animKey = new Key(player, new EntityID("0", 0));
                    }
                    self.SceneAs<Level>().Add(animKey);
                    self.SceneAs<Level>().Session.Keys.Add(animKey.ID);

                    self.TryOpen(player, animKey.follower);
                }
            }
        }
    }
}
