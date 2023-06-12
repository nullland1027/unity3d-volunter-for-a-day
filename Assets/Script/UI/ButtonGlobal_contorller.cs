using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGlobal_contorller : MonoBehaviour
{
    public static bool hasDone = false;
    public float startOneCloseTime;
    public GameObject startOne;
    public GameObject chooseOne;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasDone)
        {
            AudioManager.instance.PlayMusic(MusicType.Background_StartMenu, gameObject);
            hasDone = true;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (startOne.activeSelf)
            player.GetComponent<moveDog>().canNotMove = true;
        else
            player.GetComponent<moveDog>().canNotMove = false;
    }

    /// <summary> 关闭开始界面 </summary>
    public IEnumerator CloseStartOne()
    {
        startOne.SetActive(false);
        yield return null;
        chooseOne.SetActive(true);
        AudioManager.instance.PlayMusic(MusicType.Background_ChooseMenu, gameObject);
    }

    /// <summary> 恢复开始界面 </summary>
    public IEnumerator OpenStartOne()
    {
        chooseOne.SetActive(false);
        yield return null;
        startOne.SetActive(true);
        AudioManager.instance.PlayMusic(MusicType.Background_StartMenu, gameObject);
    }

    public void OnClick_start()
    {
        StartCoroutine(CloseStartOne());
    }

    public void OnClick_chooseBack()
    {
        StartCoroutine(OpenStartOne());
    }

    public void OnClick_exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        return;
    }

    public void OnClick_playMusic()
    {
        AudioManager.instance.PlayMusic(MusicType.UI_Click, gameObject);
    }
}
