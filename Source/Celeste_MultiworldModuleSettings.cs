namespace Celeste.Mod.Celeste_Multiworld;

public class Celeste_MultiworldModuleSettings : EverestModuleSettings
{
    #region Connection Info
    [SettingIgnore]
    public string Address { get; set; } = "archipelago.gg";
    [SettingIgnore]
    [SettingMinLength(1)]
    [SettingMaxLength(16)]
    public string SlotName { get; set; } = "Maddy";
    [SettingIgnore]
    public string Password { get; set; } = "";
    #endregion

    #region Send/Receive Messages
    public enum ItemReceiveStyle
    {
        None,
        Non_Strawberry_Progression,
        Progression,
        All
    }
    public ItemReceiveStyle ItemReceiveMessages { get; set; } = ItemReceiveStyle.Non_Strawberry_Progression;

    public enum ItemSendStyle
    {
        None,
        Progression,
        All
    }

    public ItemSendStyle ItemSendMessages { get; set; } = ItemSendStyle.All;

    public bool ChatMessages { get; set; } = true;
    #endregion
}