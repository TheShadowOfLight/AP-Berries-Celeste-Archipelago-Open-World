using System;
using Celeste.Mod.Celeste_Multiworld.Items;

namespace Celeste.Mod.Celeste_Multiworld;

public class Celeste_MultiworldModule : EverestModule
{
    public static Celeste_MultiworldModule Instance { get; private set; }

    public override Type SettingsType => typeof(Celeste_MultiworldModuleSettings);
    public static Celeste_MultiworldModuleSettings Settings => (Celeste_MultiworldModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(Celeste_MultiworldModuleSession);
    public static Celeste_MultiworldModuleSession Session => (Celeste_MultiworldModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(Celeste_MultiworldModuleSaveData);
    public static Celeste_MultiworldModuleSaveData SaveData => (Celeste_MultiworldModuleSaveData) Instance._SaveData;

    public Celeste_MultiworldModule()
    {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(Celeste_MultiworldModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(Celeste_MultiworldModule), LogLevel.Info);
#endif
    }

    public override void Load()
    {
        new ArchipelagoManager(Celeste.Instance);

        // TODO: apply any hooks that should always be active
        spring = new modSpring();
        spring.Load();
    }

    public override void Unload()
    {
        // TODO: unapply any hooks applied in Load()
    }

    modSpring spring;
}
