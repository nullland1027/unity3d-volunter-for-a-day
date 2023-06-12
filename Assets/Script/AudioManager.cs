using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("背景音乐")]
    [SerializeField] private AudioClip backgroundClip_StartMenu;
    [SerializeField] private AudioClip backgroundClip_ChooseMenu;
    [SerializeField] private AudioClip backgroundClip_InGame;
    [Header("UI相关")]
    [SerializeField] private AudioClip clickClip_UI;
    [Header("地图相关")]
    [SerializeField] private AudioClip tileCreateClip_Map;
    [SerializeField] private AudioClip tileDeleteClip_Map;
    [Header("人物移动相关")]
    [SerializeField] private AudioClip jumpClip_Role;
    [SerializeField] private AudioClip walkClip_0_Role;
    [SerializeField] private AudioClip walkClip_1_Role;
    [SerializeField] private AudioClip walkClip_2_Role;
    [SerializeField] private AudioClip walkClip_3_Role;
    [SerializeField] private AudioClip walkClip_4_Role;
    [SerializeField] private AudioClip blockClip_Role;


    public List<AudioGroup> audioSource_One = new List<AudioGroup>();
    public List<AudioGroup> audioSource_Background = new List<AudioGroup>();
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }


    public void PlayMusic(MusicType musicType, GameObject target)
    {
        AudioSource tempS;
        AudioGroup tempAG;
        switch (musicType)
        {
            /* UI相关 */
            case MusicType.UI_Click:
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = clickClip_UI;
                tempS.Play();
                tempS.loop = false;
                tempS.volume = .5f;
                tempAG = new AudioGroup(tempS, target);
                audioSource_One.Add(tempAG);
                StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                break;
            /* 地图相关 */
            case MusicType.Map_TileCreate:
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = tileCreateClip_Map;
                tempS.Play();
                tempS.loop = false;
                tempS.volume = .5f;
                tempAG = new AudioGroup(tempS, target);
                audioSource_One.Add(tempAG);
                StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                break;
            case MusicType.Map_TileDelete:
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = tileDeleteClip_Map;
                tempS.Play();
                tempS.loop = false;
                tempS.volume = .5f;
                tempAG = new AudioGroup(tempS, target);
                audioSource_One.Add(tempAG);
                StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                break;
            
            /* 人物 */
            case MusicType.Role_Block:
                if (!CheckWhetherIsPlay(blockClip_Role, target))
                {
                    tempS = gameObject.AddComponent<AudioSource>();
                    tempS.clip = blockClip_Role;
                    tempS.Play();
                    tempS.loop = false;
                    tempS.volume = .5f;
                    tempAG = new AudioGroup(tempS, target);
                    audioSource_One.Add(tempAG);
                    StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                }
                break;
            case MusicType.Role_Jump:
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = jumpClip_Role;
                tempS.Play();
                tempS.loop = false;
                tempS.volume = .4f;
                tempAG = new AudioGroup(tempS, target);
                audioSource_One.Add(tempAG);
                StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                break;
            case MusicType.Role_Walk:
                tempS = gameObject.AddComponent<AudioSource>();
                int num = Random.Range(0,5);
                switch(num)
                {
                    case 0:
                        tempS.clip = walkClip_0_Role;
                        break;
                    case 1:
                        tempS.clip = walkClip_1_Role;
                        break;
                    case 2:
                        tempS.clip = walkClip_2_Role;
                        break;
                    case 3:
                        tempS.clip = walkClip_3_Role;
                        break;
                    case 4:
                        tempS.clip = walkClip_4_Role;
                        break;
                }
                tempS.Play();
                tempS.loop = false;
                tempS.volume = 1f;
                tempAG = new AudioGroup(tempS, target);
                audioSource_One.Add(tempAG);
                StartCoroutine(DeleteAudioAfterPlay(tempAG, audioSource_One));
                break;

            /* 背景音乐 */
            case MusicType.Background_StartMenu:
                DeleteAllBackgroundAudioImmediately();
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = backgroundClip_StartMenu;
                tempS.Play();
                tempS.volume = 0.5f;
                tempS.loop = true;
                tempAG = new AudioGroup(tempS, target);
                audioSource_Background.Add(tempAG);
                break;
            case MusicType.Background_ChooseMenu:
                DeleteAllBackgroundAudioImmediately();
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = backgroundClip_ChooseMenu;
                tempS.Play();
                tempS.volume = 0.5f;
                tempS.loop = true;
                tempAG = new AudioGroup(tempS, target);
                audioSource_Background.Add(tempAG);
                break;
            case MusicType.Background_InGame:
                DeleteAllBackgroundAudioImmediately();
                tempS = gameObject.AddComponent<AudioSource>();
                tempS.clip = backgroundClip_InGame;
                tempS.Play();
                tempS.volume = 0.5f;
                tempS.loop = true;
                tempAG = new AudioGroup(tempS, target);
                audioSource_Background.Add(tempAG);
                break;
        }
    }


    IEnumerator DeleteAudioAfterPlay(AudioGroup ag, List<AudioGroup> agList)
    {
        yield return new WaitForSeconds(ag.audioSource.clip.length);
        agList.Remove(ag);
        Destroy(ag.audioSource);
    }

    public void DeleteAllBackgroundAudioImmediately()
    {
        foreach(AudioGroup ag in audioSource_Background)
        {
            
            Destroy(ag.audioSource);
        }
        audioSource_Background.Clear();
    }

    public bool CheckWhetherIsPlay(AudioClip m_audioClip, GameObject m_target)
    {
        bool ans = false;
        for(int i = 0; i < audioSource_One.Count; i++)
        {
            if (audioSource_One[i].target == m_target && audioSource_One[i].audioSource.clip == m_audioClip)
            {
                ans = true;
                break;
            }
        }
        return ans;
    }

}

public enum MusicType
{
    Background_StartMenu, Background_ChooseMenu, Background_InGame,
    UI_Click,
    Map_TileCreate, Map_TileDelete,
    Role_Walk, Role_Jump, Role_Block
}

public class AudioGroup
{
    public AudioSource audioSource;
    public GameObject target;
    
    public AudioGroup()
    {
    }

    public AudioGroup(AudioSource audioS, GameObject t)
    {
        audioSource = audioS;
        target = t;
    }
}