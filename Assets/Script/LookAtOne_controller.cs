using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtOne_controller : MonoBehaviour
{
    public GameObject mainCarema;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCarema.transform.position + offset;
    }
}
