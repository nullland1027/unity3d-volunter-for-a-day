using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Global : MonoBehaviour
{
    public static int sumOfCurrentQueue = 1;

    public static void addSumOfCurrentQueue(){
        sumOfCurrentQueue++;
        Debug.Log("Queue增加，value: " + sumOfCurrentQueue);
    }

    public static int getSumOfCurrentQueue(){
        return sumOfCurrentQueue;
    }

    
    /// <summary>
    /// 判断前方一格方块能否前进，能返回物体，反之返回NULL
    /// </summary>
    public static GameObject canWalk(GameObject playerGO, Vector3 dir, LayerMask ignoreLayer)
    {
        GameObject ans = null;
        RaycastHit hit;
        // Debug.DrawRay(playerGO.transform.position + Vector3.up * 5f + dir, Vector3.down, Color.red, 5f);
        if (Physics.Raycast(playerGO.transform.position + Vector3.up * 5f + dir * 2f, Vector3.down, out hit, 10f, ~ignoreLayer))
        {
            ans = hit.transform.gameObject;
            // Debug.Log(ans.name);
        }
        return ans;
    }

    /// <summary>
    /// 返回当前脚下的物体
    /// </summary>
    public static GameObject currentStand(GameObject playerGO, LayerMask ignoreLayer)
    {
        GameObject ans = null;
        RaycastHit hit;
        Debug.DrawRay(playerGO.transform.position + Vector3.down * 3, Vector3.up, Color.red, 100f);
        if (Physics.Raycast(playerGO.transform.position + Vector3.down * 3, Vector3.up, out hit, 10f, ~ignoreLayer))
        {
            ans = hit.transform.gameObject;
            // Debug.Log(ans.name);
        } else {
            Debug.Log("当前脚下砖块不存在！");
        }
        return ans;
    }

    /// <summary>
    /// 返回物体下一步踩到的物体，反之返回NULL
    /// </summary>
    public static GameObject nextStandNO(GameObject playerGO, Vector3 dir)
    {
        GameObject ans = null;
        BoxCollider tempBC = playerGO.GetComponent<BoxCollider>();
        RaycastHit hit;
        Vector3 pos = tempBC.bounds.center + new Vector3(0f, -tempBC.bounds.extents.y, 0f) + Vector3.up * 0.1f;
        Debug.DrawRay(pos, dir* 2, Color.red, 5f);
        if (Physics.Raycast(pos, dir, out hit, 2f))
        {
            ans = hit.transform.gameObject;
            // Debug.Log(ans.name);
        }
        return ans;
    }

    /// <summary>
    /// 返回物体下一步踩到的物体，反之返回NULL
    /// </summary>
    public static GameObject nextStand(GameObject playerGO, Vector3 dir, LayerMask ignoreLayer)
    {
        GameObject ans = null;
        BoxCollider tempBC = playerGO.GetComponent<BoxCollider>();
        RaycastHit hit;
        Vector3 pos = tempBC.bounds.center + new Vector3(1.8f * dir.x, 1.8f * dir.y, 1.8f * dir.z);
        // Debug.DrawRay(pos+ Vector3.down * 5f, Vector3.up * 5f, Color.red, 100f);
        if (Physics.Raycast(pos + Vector3.down * 5f, Vector3.up, out hit, 5f, ~ignoreLayer))
        {
            ans = hit.transform.gameObject;
            // Debug.Log(ans.name);
        } else {
            Debug.Log("nextStand 没有物体！");
        }
        return ans;
    }

    /// <summary>
    /// 返回前方是否有人，无人为NULL
    /// </summary>
    public static GameObject nextStandPeople(GameObject playerGO, Vector3 dir)
    {
        GameObject ans = null;
        BoxCollider tempBC = playerGO.GetComponent<BoxCollider>();
        Vector3 pos = tempBC.bounds.center;
        Collider[] hit = Physics.OverlapSphere(pos + dir * 1.8f, 0.9f);
        if (hit.Length != 0)
        {
            foreach(Collider c in hit)
            {
                if (c.CompareTag("Role") || c.CompareTag("Player"))
                {
                    ans = c.gameObject;
                    break;
                }
            }
            
        } else {
            Debug.Log("nextStandPeople 没有物体！");
        }
        return ans;
    }

    /// <summary>
    /// 返回当前地图中起点所在的位置
    /// </summary>
    /// <returns> 起始点对应方块的Transform </returns>
    public static Transform SearchTheStartPos()
    {
        Transform ans = null;
        Transform parentOfTiles_TR =  GameObject.Find("Grid").transform.Find("Ground");
        for(int i = 0; i < parentOfTiles_TR.childCount; i++)
        {
            Transform tf = parentOfTiles_TR.GetChild(i);
            if (tf.CompareTag("Tile_Start"))
            {
                ans = tf;
                break;
            }
        }
        return ans;
    }

    /// <summary>
    /// 检测当前地图是否已经走完
    /// </summary>
    /// <returns> 当前是否走完，走完为True，反之为False </returns>
    public static bool hasFinishWalking(int teamLength)
    {
        bool ans = true;
        Transform parentOfTiles_TR =  GameObject.Find("Grid").transform.Find("Ground");
        int num = 0;
        for(int i = 0; i < parentOfTiles_TR.childCount; i++)
        {
            Transform tf = parentOfTiles_TR.GetChild(i);
            if (tf.name.Contains("CanWalkOnce"))
            {
                num++;
                if (num > teamLength)
                {
                    ans = false;
                    break;
                }
                
            }
        }
        return ans;
    }
}
