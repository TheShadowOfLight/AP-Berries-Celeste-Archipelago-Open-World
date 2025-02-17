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

    public bool Springs { get; set; } = false;
    public bool TrafficBlocks { get; set; } = false;
    public bool DreamBlocks { get; set; } = false;
    public bool MovingPlatforms { get; set; } = false;
    #endregion
}
