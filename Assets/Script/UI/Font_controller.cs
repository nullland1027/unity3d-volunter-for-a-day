using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Font_controller : MonoBehaviour
{
    public Font font1;
    public Font font2;
    public float time;
    public float randomTime;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        randomTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > randomTime)
        {
            time = 0f;
            // randomTime = Random.Range(0.05f, 0.2f);
            if (GetComponent<Text>().font == font1)
            {
                GetComponent<Text>().font = font2;
            }
            else
            {
                GetComponent<Text>().font = font1;
            }
        }
    }
}
