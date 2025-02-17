namespace Celeste.Mod.Celeste_Multiworld;

public class Celeste_MultiworldModuleSettings : EverestModuleSettings
{
    [SettingIgnore]
    public string Address { get; set; } = "archipelago.gg";
    [SettingIgnore]
    [SettingMinLength(1)]
    [SettingMaxLength(16)]
    public string SlotName { get; set; } = "Maddy";
    [SettingIgnore]
    public string Password { get; set; } = "";
}