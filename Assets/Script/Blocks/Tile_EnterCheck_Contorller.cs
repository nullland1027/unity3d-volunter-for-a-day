using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_EnterCheck_Contorller : MonoBehaviour
{
    public int m_levelID;
    public bool hasDone;    // 是否通关
    public bool isLock;     // 能否进入
    public GameObject levelOne;
    public bool isEnter;
    public bool hasChange;

    private void Start()
    {
        levelOne = GameObject.Find("Canvas").transform.Find("LevelOne").gameObject;
        hasChange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("111");
            levelOne.SetActive(true);
            levelOne.GetComponent<LevelOne_controller>().UpdateUI(hasDone, m_levelID);
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isEnter && Input.GetKey(KeyCode.R))
        {
            if (!hasChange)
            {
                // Debug.Log("执行一次");
                LevelManager.instance.LoadScene("SampleScene", m_levelID);
                hasChange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            levelOne.SetActive(false);
            isEnter = false;
        }
    }
}
