using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinemachine : MonoBehaviour
{
    
    public GameObject front;
    public GameObject left;
    public GameObject behind;
    public GameObject right;
    public GameObject top;
    public static int current_position;
    public GameObject center;

    private bool isLock;

    void Start()
    {   
        GameObject cm = GameObject.Find("CM");
        if ( cm != null) {
            if (cm.transform.Find("CM vcam0") != null)
            {
                front = cm.transform.Find("CM vcam0").gameObject;
                left = cm.transform.Find("CM vcam1").gameObject;
                behind = cm.transform.Find("CM vcam2").gameObject;
                right = cm.transform.Find("CM vcam3").gameObject;
                top = cm.transform.Find("CM vcamTop").gameObject;
            }
            
        }
        
        
        isLock = false;
        current_position = 0;  //0代表前面，1代表左边，2代表后面，3代表右边

    }

    // Update is called once per frame

    /**
    // void Update()
    // {
        
    //     if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 0)) {
    //         current_position++;
    //         StartCoroutine(ChangeDirOfCameraClockwise(current_position));
    //         front.SetActive(false);
    //         left.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 1)) {
    //         current_position++;
    //         left.SetActive(false);
    //         behind.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 2)) {
    //         current_position++;
    //         behind.SetActive(false);
    //         right.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 3)) {
    //         current_position++;
    //         right.SetActive(false);
    //         front.SetActive(true);
    //     }

    //     if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 0)) {
    //         current_position += 3;
    //         front.SetActive(false);
    //         right.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 1)){
    //         current_position += 3;
    //         left.SetActive(false);
    //         front.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 2)){
    //         current_position += 3;
    //         behind.SetActive(false);
    //         left.SetActive(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 3)){
    //         current_position += 3;
    //         right.SetActive(false);
    //         behind.SetActive(true);
    //     }
    // }
    */
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 0)) {
            StartCoroutine(ChangeDirOfCameraClockwise(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 1)) {
            StartCoroutine(ChangeDirOfCameraClockwise(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 2)) {
            StartCoroutine(ChangeDirOfCameraClockwise(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.Q) && (current_position % 4 == 3)) {
            StartCoroutine(ChangeDirOfCameraClockwise(current_position));
        }

        if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 0)) {
            StartCoroutine(ChangeDirOfCameraReverse(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 1)){
            StartCoroutine(ChangeDirOfCameraReverse(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 2)){
           StartCoroutine(ChangeDirOfCameraReverse(current_position));
        }
        else if (Input.GetKeyDown(KeyCode.E) && (current_position % 4 == 3)){
            StartCoroutine(ChangeDirOfCameraReverse(current_position));
        }


        /**
        顶层试图
        */

        if (Input.GetKey(KeyCode.Space)) {
            if (center == null)
                center = GameObject.Find("Grid/Ground").transform.Find("Tile_Center(Clone)").gameObject;
            Vector3 centerPosition = center.transform.position;
            top.transform.position = new Vector3(centerPosition.x, centerPosition.y + 15, centerPosition.z);
            if (current_position % 4 == 0) {
                front.SetActive(false);
                // if (!top.activeSelf)
                //     top.transform.Rotate(new Vector3(0, 0, 0));
                top.SetActive(true);
            }
            else if (current_position % 4 == 1) {
                left.SetActive(false);
                // if (!top.activeSelf)
                //     top.transform.Rotate(new Vector3(0, 90, 0));
                top.SetActive(true);
            }
            else if (current_position % 4 == 2) {
                behind.SetActive(false);
                // if (!top.activeSelf)
                //     top.transform.Rotate(new Vector3(0, 90, 0));
                top.SetActive(true);
            }
            else if (current_position % 4 == 3) {
                right.SetActive(false);
                // if (!top.activeSelf)
                //     top.transform.Rotate(new Vector3(0, 90, 0));
                top.SetActive(true); 
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            if (current_position % 4 == 0) {
                top.SetActive(false);
                front.SetActive(true);
            }
            if (current_position % 4 == 1) {
                top.SetActive(false);
                left.SetActive(true);
            }
            if (current_position % 4 == 2) {
                top.SetActive(false);
                behind.SetActive(true);
            }
            if (current_position % 4 == 3) {
                top.SetActive(false);
                right.SetActive(true);
            }
        }
        

        if (current_position % 4 == 0) {
            Direction.forward = new Vector3(0, 0, 1);
            Direction.left = new Vector3(-1, 0, 0);
            Direction.back = new Vector3(0, 0, -1);
            Direction.right = new Vector3(1, 0, 0);
        }
        if (current_position % 4 == 1) {
            Direction.forward = new Vector3(1, 0, 0);
            Direction.left = new Vector3(0, 0, 1);
            Direction.back = new Vector3(-1, 0, 0);
            Direction.right = new Vector3(0, 0, -1);
        }
        if (current_position % 4 == 2) {
            Direction.forward = new Vector3(0, 0, -1);
            Direction.left = new Vector3(1, 0, 0);
            Direction.back = new Vector3(0, 0, 1);
            Direction.right = new Vector3(-1, 0, 0);
        }
        if (current_position % 4 == 3) {
            Direction.forward = new Vector3(-1, 0, 0);
            Direction.left = new Vector3(0, 0, -1);
            Direction.back = new Vector3(1, 0, 0);
            Direction.right = new Vector3(0, 0, 1);
        }

        // Direction.Show();
    }
    IEnumerator ChangeDirOfCameraClockwise(int currentP)
    {
        if (isLock)
            yield break;
        
        isLock = true;
        if (currentP % 4 == 0) { //0 -> 1
            front.SetActive(false);
            left.SetActive(true);
            current_position++;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, -90));

        } else if (currentP % 4 == 1) { //1 -> 2
            left.SetActive(false);
            behind.SetActive(true);
            current_position++;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 180));
        } else if (currentP % 4 == 2) {
            behind.SetActive(false);
            right.SetActive(true);
            current_position++;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 90));
        } else if (currentP % 4 == 3) {
            right.SetActive(false);
            front.SetActive(true);
            current_position++;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 0));
        }
        yield return new WaitForSeconds(1.5f);
        isLock = false;
    }

    IEnumerator ChangeDirOfCameraReverse(int currentP)
    {
        if (isLock)
            yield break;
        
        isLock = true;
        if (currentP % 4 == 0) { // 0 -> 3
            front.SetActive(false);
            right.SetActive(true);
            current_position += 3;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 90));
        } else if (currentP % 4 == 1) { // 1 -> 0
            left.SetActive(false);
            front.SetActive(true);
            current_position += 3;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 0));
        } else if (currentP % 4 == 2) { // 2 -> 1
            behind.SetActive(false);
            left.SetActive(true);
            current_position += 3;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, -90));
        } else if (currentP % 4 == 3) { // 3 -> 2
            right.SetActive(false);
            behind.SetActive(true);
            current_position += 3;
            top.transform.rotation = Quaternion.identity;
            if (!top.activeSelf)
                top.transform.Rotate(new Vector3(90, 0, 180));
        }
        yield return new WaitForSeconds(1.5f);
        isLock = false;
    }


}
