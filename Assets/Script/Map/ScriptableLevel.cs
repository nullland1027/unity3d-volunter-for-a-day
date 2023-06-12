using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableLevel : ScriptableObject
{
    public int levelIndex;
    public List<SavedTile> groundTiles;
    
}

[System.Serializable]
public class SavedTile
{
    public Vector3 m_position;
    public Quaternion m_rotation;
    public TileType m_tile;
}

[System.Serializable]
public enum TileType
{
    CanWalkOnce_low, CanWalkOnce_middle, CanWalkOnce_high,
    CanWalkNone_low, CanWalkNone_middle, CanWalkNone_high,
    CanWalkMany_low, CanWalkMany_middle, CanWalkMany_high,
    Start_middle,
    End_middle,
    Choose_middle,
    Tree_CanWalkNone_low, Tree_CanWalkNone_middle, Tree_CanWalkNone_high,
    Rock_CanWalkNone_low, Rock_CanWalkNone_middle, Rock_CanWalkNone_high,
    Player,
    Role_1,
    CinemaCenter
}
