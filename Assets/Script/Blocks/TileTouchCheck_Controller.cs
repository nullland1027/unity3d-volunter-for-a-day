using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouchCheck_Controller : MonoBehaviour
{
    private Vector3 pos;
    private void OnTriggerExit(Collider other)
    {
        if (transform.parent.CompareTag("Tile_CanWalkOnce") && other.CompareTag("Player"))
        {
            pos = transform.parent.position;
            Debug.Log(transform.parent.name);
            string levelOfBlock = transform.parent.name.Split('_')[2].Replace("(Clone)", "");
            GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tile_CanWalkNone_" + levelOfBlock));
            go.transform.SetParent(transform.parent.parent);
            go.transform.position = pos;
            Destroy(transform.parent.gameObject);
        }
    }
}
