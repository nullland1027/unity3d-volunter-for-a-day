using System;
using UnityEngine;

public class rolePlayerCollision : MonoBehaviour
{
  void OnTriggerEnter(Collider other){
    // 首次触发有效
    Debug.Log("我的父物体为：" + this.gameObject.transform.parent.gameObject.name);
    this.gameObject.transform.parent.gameObject.SendMessage("setTarget", other);
    // if(moveDog.target == null){
    //   Debug.Log("Trigger 触发！#: " + other.name);
    //   // By Tag
    //   if(other.CompareTag("Player")){
    //     this.gameObject.GetComponent<moveDog>(moveDog).target = other.gameObject;
    //     // moveDog.target = other.gameObject;
    //   }
    // }
  }
}