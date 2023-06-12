using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour 
{
  public static Vector3 forward = new Vector3(0f, 0f, 1f);
  public static Vector3 left = new Vector3(-1f, 0f, 0f);
  public static Vector3 back = new Vector3(0f, 0f, -1f);
  public static Vector3 right  = new Vector3(1f, 0f, 0f);

  public static void Show()
  {
    Debug.Log("forward: " + forward.ToString());
    Debug.Log("left: " + left.ToString());
    Debug.Log("back: " + back.ToString());
    Debug.Log("right: " + right.ToString());
  }
}