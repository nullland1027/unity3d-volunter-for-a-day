using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject map;
    public GameObject tile_perfab;
    public MapData mapContainer;
    public GameObject trace;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map");
        CreateMap();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMap()
    {
        foreach(Vector3 b in mapContainer.container)
        {
            GameObject temp = Instantiate<GameObject>(tile_perfab);
            //GameObject flower=Instantiate<GameObject>(trace);
            //flower.transform.position=new Vector3(b.x,b.y+0.4f,b.z);
            temp.transform.position =b ;
            //flower.GetComponent<Renderer>().enabled = false;
            //flower.transform.SetParent(temp.transform);
            temp.transform.SetParent(map.transform);
        }
    }


    
}

