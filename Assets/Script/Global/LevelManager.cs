using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;

    private float _target;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    public async void LoadScene(string sceneName, int levelID)
    {
        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do 
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);

        while (scene.progress < 0.95f)
        {
            await Task.Delay(100);
        }

        TilemapManager.instance._groundMap = GameObject.Find("Grid/Ground").GetComponent<Tilemap>();
        if (sceneName == "StartMenu")
        {
            StartCoroutine(GameObject.Find("Canvas").GetComponent<ButtonGlobal_contorller>().CloseStartOne());
            TilemapManager.instance.RefreshTheTileOfLevelID();
            GameObject.Find("Player").transform.position = TilemapManager.instance.findTheTileOfSpecificLevelID().transform.position + Vector3.up * 1.5f;
            AudioManager.instance.PlayMusic(MusicType.Background_ChooseMenu, gameObject);
        }
        else
        {
            AudioManager.instance.PlayMusic(MusicType.Background_InGame, gameObject);
        }
        
        if (levelID != 100)
        {
            TilemapManager.instance._levelIndex = levelID;
            StartCoroutine(TilemapManager.instance.LoadMapDynamic());
        }
        
    }

}
