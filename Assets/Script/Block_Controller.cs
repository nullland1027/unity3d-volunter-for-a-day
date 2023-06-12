using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    public GameObject Trace;
    // Start is called before the first frame update
    void Start()
    {
        Trace=GameObject.Find("flowersLow");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

  bool check_WhetherCanWalk(Vector3 v3)
  {
      RaycastHit hit;
      Ray ray=new Ray(v3,Vector3.down);
      if(Physics.Raycast(ray,out hit))
      {
          return true;
      }
      else
      {
          return false;
      }
      
  } 

  private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("进入");
            GameObject flower=Instantiate<GameObject>(Trace);
            flower.transform.position=new Vector3(transform.position.x,transform.position.y+0.4f,transform.position.z);
            flower.transform.SetParent(transform);
        }
    }
            
}
