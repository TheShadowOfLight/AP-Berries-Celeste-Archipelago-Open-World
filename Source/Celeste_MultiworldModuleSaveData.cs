using System.Collections.Generic;

namespace Celeste.Mod.Celeste_Multiworld;

public class Celeste_MultiworldModuleSaveData : EverestModuleSaveData
{
    #region AP General
    public int ItemRcv { get; set; } = 0;
    #endregion

    #region AP Items
    public int Strawberries { get; set; } = 0;

    public Dictionary<long, bool> Interactables { get; set; } = new Dictionary<long, bool>();
    #endregion

    #region AP Locations
    public HashSet<string> LevelClearLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CheckpointLocations { get; set; } = new HashSet<string>();
    public HashSet<string> StrawberryLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CrystalHeartLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CassetteLocations { get; set; } = new HashSet<string>();
    public HashSet<string> BinocularsLocations { get; set; } = new HashSet<string>();
    public HashSet<string> KeyLocations { get; set; } = new HashSet<string>();
    public HashSet<string> RoomLocations { get; set; } = new HashSet<string>();
    #endregion
}
