using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningButton_controller : MonoBehaviour
{
    public GameObject settingOne;

    public void ReturnToMainMenu()
    {
        settingOne.SetActive(false);
        TilemapManager.instance.ClearMap();
        LevelManager.instance.LoadScene("StartMenu", 100);
    }

    public void WinGame()
    {
        settingOne.SetActive(false);
        TilemapManager.instance.ClearMap();
        TilemapManager.instance.levelHavePass[TilemapManager.instance._levelIndex] = true;
        Debug.Log(TilemapManager.instance.levelHavePass[TilemapManager.instance._levelIndex]);
        LevelManager.instance.LoadScene("StartMenu", 100);
    }

    public void RestartGame()
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        settingOne.GetComponent<CanvasGroup>().alpha = 0;
        settingOne.GetComponent<CanvasGroup>().interactable = false;
        for (int i = 0; i < GameObject.Find("CM").transform.childCount; i++)
        {
            Transform tr = GameObject.Find("CM").transform.GetChild(i);
            tr.gameObject.SetActive(false);
        }
        GameObject.Find("CM").transform.Find("CM vcam0").gameObject.SetActive(true);

        yield return StartCoroutine(TilemapManager.instance.ClearMapDynamic());
        yield return StartCoroutine(TilemapManager.instance.LoadMapDynamic());
        settingOne.GetComponent<CanvasGroup>().alpha = 1;
        settingOne.GetComponent<CanvasGroup>().interactable = true;
        settingOne.SetActive(false);
    }
}
