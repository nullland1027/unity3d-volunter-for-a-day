using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn_controller : MonoBehaviour
{
    public bool hasStart;
    public GameObject player;
    public GameObject settingOne;
    // Start is called before the first frame update
    void Start()
    {
        hasStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStart)
        {
            if (player != null)
            {
                if (settingOne.activeSelf)
                    player.GetComponent<moveDog>().canNotMove = true;
                else
                    player.GetComponent<moveDog>().canNotMove = false;
            }
        }
        
    }

    public void OnClick_playMusic()
    {
        AudioManager.instance.PlayMusic(MusicType.UI_Click, gameObject);
    }
}
