using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;


public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;
    public float appearUpRange;   // 出现时上升的距离
    public float appearTime;    // 出现所需时间
    public float intervalTime;  // 每个方块生成的间隙
    public float intervalTimeOfDelete;  // 删除时每个方块的间隙
    public Tilemap _groundMap;
    public int _levelIndex;

    public Dictionary<int, bool> levelHavePass;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            levelHavePass = new Dictionary<int, bool>(){{0, false}, {1, false}, {2, false}, {3, false}, {4, false}};
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _groundMap = GameObject.Find("Grid/Ground").GetComponent<Tilemap>();
        // 测试
        // Debug.Log(Player_Global.SearchTheStartPos());
        // ClearMap();
        // StartCoroutine(LoadMapDynamic());
    }

    private void Update()
    {
        // 测试
        // Debug.Log("是否完成：" + Player_Global.hasFinishWalking());
    }
    #if UNITY_EDITOR
    public void SaveMap()
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.levelIndex = _levelIndex;
        newLevel.name = $"Level {_levelIndex}";

        newLevel.groundTiles = GetTilesFromMap(_groundMap).ToList();


        ScriptableObjectUtility.SaveLevelFile(newLevel);

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        {
            for (int i = 0; i < map.transform.childCount; i++)
            {
                GameObject go = map.transform.GetChild(i).gameObject;
                Vector3 pos = go.transform.position;
                Quaternion rotation = go.transform.rotation;
                TileType ty = go.GetComponent<Tile_Controller>().tileType;
                yield return new SavedTile()
                {
                    m_position = pos,
                    m_rotation = rotation,
                    m_tile = ty
                };
            }
        }
    }
    #endif

    public GameObject findTheTileOfSpecificLevelID()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        GameObject ans = null;
        bool hasEnd = false;
        foreach (var tilemap in maps)
        {
            for (int i = 0; i < tilemap.transform.childCount; i++)
            {
                if (tilemap.transform.GetChild(i).name.Contains("Choose"))
                {
                    if (tilemap.transform.GetChild(i).GetChild(0).GetComponent<Tile_EnterCheck_Contorller>().m_levelID == _levelIndex)
                    {
                        ans = tilemap.transform.GetChild(i).gameObject;
                        hasEnd = true;
                    }
                }
                if (hasEnd)
                    break;
            }
            if (hasEnd)
                break;
        }
        return ans;
    }

    public void RefreshTheTileOfLevelID()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach (var tilemap in maps)
        {
            for (int i = 0; i < tilemap.transform.childCount; i++)
            {
                if (tilemap.transform.GetChild(i).name.Contains("Choose"))
                {
                    tilemap.transform.GetChild(i).GetChild(0).GetComponent<Tile_EnterCheck_Contorller>().hasDone = levelHavePass[tilemap.transform.GetChild(i).GetChild(0).GetComponent<Tile_EnterCheck_Contorller>().m_levelID];
                }
               
            }
        }
    }

    public void ClearMap()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        GameObject.Find("Canvas").transform.Find("Setting").GetComponent<SettingBtn_controller>().hasStart = false;

        foreach (var tilemap in maps)
        {
            for (int i = tilemap.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tilemap.transform.GetChild(i).gameObject);
            }
        }
    }

    public IEnumerator ClearMapDynamic()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in maps)
        {
            for (int i = tilemap.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tilemap.transform.GetChild(i).gameObject);
                AudioManager.instance.PlayMusic(MusicType.Map_TileDelete, gameObject);
                yield return new WaitForSeconds(intervalTimeOfDelete);
            }
        }
    }

    public void LoadMap()
    {
        var level = Resources.Load<ScriptableLevel>($"Levels/Level {_levelIndex}");
        if (level == null)
        {
            Debug.LogError($"Level {_levelIndex} doesn't exist!");
            return;
        }

        ClearMap();

        foreach (var savedTileOne in level.groundTiles)
        {
            GameObject go;
            switch (savedTileOne.m_tile)
            {
                case TileType.Player:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Role/Player"));
                    break;
                case TileType.Role_1:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Role/role_1"));
                    break;

                case TileType.CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_high"));
                    break;
                case TileType.CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_middle"));
                    break;
                case TileType.CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_low"));
                    break;

                case TileType.CanWalkOnce_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_high"));
                    break;
                case TileType.CanWalkOnce_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_middle"));
                    break;
                case TileType.CanWalkOnce_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_low"));
                    break;

                case TileType.CanWalkMany_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_high"));
                    break;
                case TileType.CanWalkMany_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_middle"));
                    break;
                case TileType.CanWalkMany_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_low"));
                    break;
                
                case TileType.Start_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Start_middle"));
                    break;
                case TileType.End_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_End_middle"));
                    break;
                case TileType.Choose_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Choose_middle"));
                    break;
                
                case TileType.Tree_CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_high"));
                    break;
                case TileType.Tree_CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_middle"));
                    break;
                case TileType.Tree_CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_low"));
                    break;
                
                case TileType.Rock_CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_high"));
                    break;
                case TileType.Rock_CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_middle"));
                    break;
                case TileType.Rock_CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_low"));
                    break;
                
                case TileType.CinemaCenter:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Center"));
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            go.transform.SetParent(_groundMap.transform);
            go.transform.position = savedTileOne.m_position;
            go.transform.rotation = savedTileOne.m_rotation;
        }
    }

    /// <summary> 动态加载地图，带淡入效果，为协程 </summary>
    public IEnumerator LoadMapDynamic()
    {
        Player_Global.sumOfCurrentQueue = 1;
        var level = Resources.Load<ScriptableLevel>($"Levels/Level {_levelIndex}");
        if (level == null)
        {
            Debug.LogError($"Level {_levelIndex} doesn't exist!");
            yield break;
        }

        ClearMap();

        foreach (var savedTileOne in level.groundTiles)
        {
            bool hasFind = false;
            Vector3 pos = new Vector3();
            switch (savedTileOne.m_tile)
            {
                case TileType.CinemaCenter:
                    pos = savedTileOne.m_position;
                    hasFind = true;
                    break;
            }

            if (hasFind)
            {
                string cm_1 = "CM vcam";
                GameObject vcam0_1 = GameObject.Find("CM").transform.Find(cm_1 + (0).ToString()).gameObject;
                GameObject vcam1_1 = GameObject.Find("CM").transform.Find(cm_1 + (1).ToString()).gameObject;
                GameObject vcam2_1 = GameObject.Find("CM").transform.Find(cm_1 + (2).ToString()).gameObject;
                GameObject vcam3_1 = GameObject.Find("CM").transform.Find(cm_1 + (3).ToString()).gameObject;
                float x_1 = pos.x;
                float y_1 = pos.y;
                float z_1 = pos.z;
                float delta_1 = 16;
                float height_1 = 15;
                Vector3 front_1 = new Vector3(x_1, y_1 + height_1, z_1 - delta_1);
                Vector3 left_1 = new Vector3(x_1 - delta_1, y_1 + height_1, z_1);
                Vector3 behind_1 = new Vector3(x_1, y_1 + height_1, z_1 + delta_1);
                Vector3 right_1 = new Vector3(x_1 + delta_1, y_1 + height_1, z_1);
                vcam0_1.transform.position = front_1;
                vcam1_1.transform.position = left_1;
                vcam2_1.transform.position = behind_1;
                vcam3_1.transform.position = right_1;
                break;
            }
                
        }

        foreach (var savedTileOne in level.groundTiles)
        {
            GameObject go;
            bool isPlayer = false;
            switch (savedTileOne.m_tile)
            {
                case TileType.Player:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Role/Player"));
                    isPlayer = true;
                    break;
                case TileType.Role_1:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Role/role_1"));
                    isPlayer = true;
                    break;

                case TileType.CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_high"));
                    break;
                case TileType.CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_middle"));
                    break;
                case TileType.CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_low"));
                    break;

                case TileType.CanWalkOnce_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_high"));
                    break;
                case TileType.CanWalkOnce_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_middle"));
                    break;
                case TileType.CanWalkOnce_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkOnce_low"));
                    break;

                case TileType.CanWalkMany_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_high"));
                    break;
                case TileType.CanWalkMany_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_middle"));
                    break;
                case TileType.CanWalkMany_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkMany_low"));
                    break;
                
                case TileType.Start_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Start_middle"));
                    break;
                case TileType.End_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_End_middle"));
                    break;
                case TileType.Choose_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Choose_middle"));
                    break;

                case TileType.Tree_CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_high"));
                    break;
                case TileType.Tree_CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_middle"));
                    break;
                case TileType.Tree_CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Tree_CanWalkNone_low"));
                    break;
                
                case TileType.Rock_CanWalkNone_high:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_high"));
                    break;
                case TileType.Rock_CanWalkNone_middle:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_middle"));
                    break;
                case TileType.Rock_CanWalkNone_low:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Rock_CanWalkNone_low"));
                    break;

                case TileType.CinemaCenter:
                    go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_Center"));
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            go.transform.SetParent(_groundMap.transform);
            go.transform.rotation = savedTileOne.m_rotation;
            if (!isPlayer)
            {
                go.transform.position = savedTileOne.m_position + Vector3.down * appearUpRange;
                // LeanTween.alpha(go, 1, appearTime);
                LeanTween.move(go, savedTileOne.m_position, appearTime).setEase(LeanTweenType.easeOutElastic);
                AudioManager.instance.PlayMusic(MusicType.Map_TileCreate, gameObject);
            }
            else
            {
                go.transform.position = savedTileOne.m_position;
            }
            
            yield return new WaitForSeconds(intervalTime);
        }
        GameObject.Find("Canvas").transform.Find("Setting").GetComponent<SettingBtn_controller>().player = GameObject.Find("Grid/Ground").transform.Find("Player(Clone)").gameObject;
        GameObject.Find("Canvas").transform.Find("Setting").GetComponent<SettingBtn_controller>().hasStart = true;

        /**
        zhh added
        */
        // GameObject centerPoint = null;
        // if (GameObject.Find("Grid/Ground").transform.Find("Tile_Center(Clone)").gameObject != null) {
        //     centerPoint = GameObject.Find("Grid/Ground").transform.Find("Tile_Center(Clone)").gameObject;
        //     Debug.Log("centerPoint", centerPoint);
        // }
        // string cm = "CM vcam";
        // GameObject vcam0 = GameObject.Find("CM").transform.Find(cm + (0).ToString()).gameObject;
        // GameObject vcam1 = GameObject.Find("CM").transform.Find(cm + (1).ToString()).gameObject;
        // GameObject vcam2 = GameObject.Find("CM").transform.Find(cm + (2).ToString()).gameObject;
        // GameObject vcam3 = GameObject.Find("CM").transform.Find(cm + (3).ToString()).gameObject;
        // float x = centerPoint.transform.position.x;
        // float y = centerPoint.transform.position.y;
        // float z = centerPoint.transform.position.z;
        // float delta = 16;
        // float height = 15;
        // Vector3 front = new Vector3(x, y + height, z - delta);
        // Vector3 left = new Vector3(x - delta, y + height, z);
        // Vector3 behind = new Vector3(x, y + height, z + delta);
        // Vector3 right = new Vector3(x + delta, y + height, z);
        // vcam0.transform.position = front;
        // vcam1.transform.position = left;
        // vcam2.transform.position = behind;
        // vcam3.transform.position = right;
    }   
}

# if UNITY_EDITOR

public static class ScriptableObjectUtility
{
    public static void SaveLevelFile(ScriptableLevel level)
    {
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

# endif