using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOne_controller : MonoBehaviour
{
    public Text levelName_text;
    public Text content_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(bool hasDone, int levelID)
    {
        levelName_text.text = "Level" + levelID.ToString();
        if (hasDone)
        {
            levelName_text.color = new Color32(56, 91, 74, 255);
            content_text.text = "You have passed this level.\n\nPress 'R' to try again if you want.";
            content_text.color = new Color32(76, 125, 78, 255);
        }
        else
        {
            levelName_text.color = new Color32(91, 81, 56, 255);
            content_text.text = "You haven't passed this level yet.\n\nPress 'R' to try if you want.";
            content_text.color = new Color32(152, 121, 81, 255);
        }
    }
}
