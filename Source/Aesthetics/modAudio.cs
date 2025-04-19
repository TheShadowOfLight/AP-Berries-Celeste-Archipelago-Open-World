using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Aesthetics
{
    public class modAudio
    {
        static List<string> shuffledSongs = new List<string>
        {
            "event:/music/lvl0/intro",
            "event:/music/lvl1/main",
            "event:/music/lvl1/theo",
            "event:/music/lvl2/beginning",
            "event:/music/lvl2/mirror",
            "event:/music/lvl2/evil_madeline",
            "event:/music/lvl2/chase",
            "event:/music/lvl2/awake",
            "event:/music/lvl3/intro",
            "event:/music/lvl3/explore",
            "event:/music/lvl3/clean",
            "event:/music/lvl3/oshiro_theme",
            "event:/music/lvl3/oshiro_chase",
            "event:/music/lvl4/main",
            "event:/music/lvl4/heavy_winds",
            "event:/music/lvl4/minigame",
            "event:/music/lvl5/normal",
            "event:/music/lvl5/middle_temple",
            "event:/music/lvl5/mirror",
            "event:/music/lvl6/madeline_and_theo",
            "event:/music/lvl6/starjump",
            "event:/music/lvl6/the_fall",
            "event:/music/lvl6/badeline_fight",
            "event:/music/lvl6/badeline_glitch",
            "event:/music/lvl6/badeline_acoustic",
            "event:/music/lvl6/main",
            "event:/music/lvl6/secret_room",
            "event:/music/lvl7/main",
            "event:/music/lvl7/final_ascent",
            "event:/music/lvl8/main",
            "event:/music/lvl9/main",
            "event:/new_content/music/lvl10/part01",
            "event:/new_content/music/lvl10/part02",
            "event:/new_content/music/lvl10/part03",
            "event:/new_content/music/lvl10/intermission_heartgroove",
            "event:/new_content/music/lvl10/reconciliation",
            "event:/new_content/music/lvl10/final_run",
            "event:/new_content/music/lvl10/cinematic/end",
            "event:/new_content/music/lvl10/granny_farewell",
            "event:/new_content/music/lvl10/golden_room",
            "event:/music/remix/01_forsaken_city",
            "event:/music/remix/02_old_site",
            "event:/music/remix/03_resort",
            "event:/music/remix/04_cliffside",
            "event:/music/remix/05_mirror_temple",
            "event:/music/remix/06_reflection",
            "event:/music/remix/07_summit",
            "event:/music/remix/09_core"
        };


        public void Load()
        {
            On.Celeste.Audio.CreateInstance += modAudio_CreateInstance;
            On.Celeste.Audio.SetMusic += modAudio_SetMusic;
        }

        public void Unload()
        {
            On.Celeste.Audio.CreateInstance -= modAudio_CreateInstance;
            On.Celeste.Audio.SetMusic -= modAudio_SetMusic;
        }

        private static FMOD.Studio.EventInstance modAudio_CreateInstance(On.Celeste.Audio.orig_CreateInstance orig, string path, Microsoft.Xna.Framework.Vector2? position)
        {
            // TODO: Use this for SFX shuffle down the road
            return orig(path, position);
        }

        private static bool modAudio_SetMusic(On.Celeste.Audio.orig_SetMusic orig, string path, bool startPlaying = true, bool allowFadeOut = true)
        {
            if (ArchipelagoManager.Instance.MusicShuffle == 0 || !shuffledSongs.Contains(path))
            {
                Logger.Warn("AP", path);
                return orig(path, startPlaying, allowFadeOut);
            }
            else
            {
                Logger.Error("AP", path);
                int index = shuffledSongs.IndexOf(path);

                int shuffledIndex = ArchipelagoManager.Instance.MusicMap[index];

                if (shuffledIndex >= shuffledSongs.Count())
                {
                    shuffledIndex = 0;
                }

                Logger.Error("AP", shuffledSongs[shuffledIndex]);
                return orig(shuffledSongs[shuffledIndex], startPlaying, allowFadeOut);
            }
        }
    }
}
