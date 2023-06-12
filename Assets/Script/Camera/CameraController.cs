using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject cam;
    public GameObject top_cam;
    public GameObject front;
    public GameObject left;
    public GameObject behind;
    public GameObject right;
    public GameObject top;

    // 顺时针的四个方向角度向量
    public Vector3[] front2left;
    public Vector3[] left2behind;
    public Vector3[] behind2right;
    public Vector3[] right2front;

    // 逆时针的四个方向角度向量
    public Vector3[] front2right;
    public Vector3[] right2behind;
    public Vector3[] behind2left;
    public Vector3[] left2front;

    public List<Vector3[]> paths1;  // 顺时针
    public List<Vector3[]> paths2;  // 逆时针
    public List<Vector3> mid1_points;  // 路径上第一个点
    public List<Vector3> mid2_points;  // 路径上第二个点
    public List<GameObject> points;  // 前左后右四个相机位置

    public int cam_position;  // 相机当前的位置 0123
    public List<Vector3[]> up_path;  // 俯视图四条向上路径
    public List<Vector3[]> down_path; // 俯视图四条向下路径
    public float rotate_time;
    public float up_down_time;
    public bool is_top;



    public class InputDir
    {
        public Vector3 up; // vector3.forward(0，0，1)
        public Vector3 left;
        public Vector3 right;
        public Vector3 down;
    }


    void Start() {
        cam_position = 0; // 相机在最初的front位置

        cam = GameObject.Find("CameraHolder"); //相机位置
        top_cam = GameObject.Find("TopCam");
        top_cam.SetActive(false);
        front = GameObject.Find("Front");
        left = GameObject.Find("Left");
        behind = GameObject.Find("Behind");
        right = GameObject.Find("Right");
        top = GameObject.Find("Top");

        rotate_time = 0.3f;
        up_down_time = 0.1f;

        is_top = false;

        paths1 = new List<Vector3[]>{  // 顺时针
            front2left, left2behind, behind2right, right2front
        };
        paths2 = new List<Vector3[]> {  //逆时针
            front2right, right2behind, behind2left, left2front
        };

        points = new List<GameObject>{front, left, behind, right};

        mid1_points = new List<Vector3>{
            new Vector3(-5, 6, -10),
            new Vector3(-10, 6, 5),
            new Vector3(5, 6, 10),
            new Vector3(10, 6, -5)
        };
        mid2_points = new List<Vector3> {
            new Vector3(-10, 6, -5),
            new Vector3(-5, 6, 10),
            new Vector3(10, 6, 5),
            new Vector3(5, 6, -10)
        };

        for (int i = 0; i < 4; i ++) {
            paths1[i] = new Vector3[4];
            paths2[i] = new Vector3[4];
        }

        //顺时针
        paths1[0][0] = points[0].transform.position; //0-1
        paths1[0][1] = mid1_points[0];
        paths1[0][2] = mid2_points[0];
        paths1[0][3] = points[1].transform.position;

        paths1[1][0] = points[1].transform.position; //1-2
        paths1[1][1] = mid1_points[1];
        paths1[1][2] = mid2_points[1];
        paths1[1][3] = points[2].transform.position;

        paths1[2][0] = points[2].transform.position; //2-3
        paths1[2][1] = mid1_points[2];
        paths1[2][2] = mid2_points[2];
        paths1[2][3] = points[3].transform.position;

        paths1[3][0] = points[3].transform.position; //3-0
        paths1[3][1] = mid1_points[3];
        paths1[3][2] = mid2_points[3];
        paths1[3][3] = points[0].transform.position;

        //逆时针
        paths2[0][0] = points[0].transform.position; //0-3
        paths2[0][1] = mid2_points[3];
        paths2[0][2] = mid1_points[3];
        paths2[0][3] = points[3].transform.position;

        paths2[1][0] = points[3].transform.position; //3-2
        paths2[1][1] = mid2_points[2];
        paths2[1][2] = mid1_points[2];
        paths2[1][3] = points[2].transform.position;

        paths2[2][0] = points[2].transform.position; //2-1
        paths2[2][1] = mid2_points[1];
        paths2[2][2] = mid1_points[1];
        paths2[2][3] = points[1].transform.position;

        paths2[3][0] = points[1].transform.position; //1-0
        paths2[3][1] = mid2_points[0];
        paths2[3][2] = mid1_points[0];
        paths2[3][3] = points[0].transform.position;

        //俯视图向上路径
        up_path = new List<Vector3[]>(){null, null, null, null};
        for (int i = 0; i < 4; i ++) {
            up_path[i] = new Vector3[4];
        }
        up_path[0][0] = front.transform.position;
        up_path[0][1] = new Vector3(0, 8, -10);
        up_path[0][2] = new Vector3(0, 10, -5);
        up_path[0][3] = top.transform.position;

        up_path[1][0] = left.transform.position;
        up_path[1][1] = new Vector3(-10, 8, 0);
        up_path[1][2] = new Vector3(-5, 10, 0);
        up_path[1][3] = top.transform.position;

        up_path[2][0] = behind.transform.position;
        up_path[2][1] = new Vector3(0, 8, 10);
        up_path[2][2] = new Vector3(0, 10, 5);
        up_path[2][3] = top.transform.position;

        up_path[3][0] = right.transform.position;
        up_path[3][1] = new Vector3(10, 8, 0);
        up_path[3][2] = new Vector3(5, 10, 0);
        up_path[3][3] = top.transform.position;

        //俯视图向下路径
        down_path = new List<Vector3[]>(){null, null, null, null};
        for (int i = 0; i < 4; i ++) {
            down_path[i] = new Vector3[4];
        }
        down_path[0][0] = top.transform.position;
        down_path[0][1] = new Vector3(0, 10, -5);
        down_path[0][2] = new Vector3(0, 8, -10);
        down_path[0][3] = front.transform.position;

        down_path[1][0] = top.transform.position;
        down_path[1][1] = new Vector3(-5, 10, 0);
        down_path[1][2] = new Vector3(-10, 8, 0);
        down_path[1][3] = left.transform.position;

        down_path[2][0] = top.transform.position;
        down_path[2][1] = new Vector3(0, 10, 5);
        down_path[2][2] = new Vector3(0, 8, 10);
        down_path[2][3] = behind.transform.position;

        down_path[3][0] = top.transform.position;
        down_path[3][1] = new Vector3(5, 10, 0);
        down_path[3][2] = new Vector3(10, 8, 0);
        down_path[3][3] = right.transform.position;
    }

    // Update is called once per frame
    void Update() {
        int[] angles = new int[]{90, 180, 270, 0};
        
        if (Input.GetKeyDown(KeyCode.Q) && !LeanTween.isTweening(cam) && !is_top) {
            LeanTween.move(cam, paths1[cam_position], rotate_time);
            LeanTween.rotate(cam, new Vector3(30, angles[cam_position], 0), rotate_time);
            cam_position = (cam_position + 1) % 4;
        }
        if (Input.GetKeyDown(KeyCode.E) && !LeanTween.isTweening(cam) && !is_top) {
            if (cam_position == 0) {
                LeanTween.move(cam, paths2[0], rotate_time);
                LeanTween.rotate(cam, new Vector3(30, -90, 0), rotate_time);
                // Direction.currentPosition++;
            }
            if (cam_position == 1) {
                LeanTween.move(cam, paths2[3], rotate_time);
                LeanTween.rotate(cam, new Vector3(30, -0, 0), rotate_time);
            }
            if (cam_position == 2) {
                LeanTween.move(cam, paths2[2], rotate_time);
                LeanTween.rotate(cam, new Vector3(30, -270, 0), rotate_time);
            }
            if (cam_position == 3) {
                LeanTween.move(cam, paths2[1], rotate_time);
                LeanTween.rotate(cam, new Vector3(30, -180, 0), rotate_time);
            }
            cam_position = (cam_position + 3) % 4;
        }
        if (Input.GetKey(KeyCode.Space) && !LeanTween.isTweening(cam) && !is_top) {
            if (cam_position == 0) {
                LeanTween.move(cam, up_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(90, 0, 0), up_down_time);
            }
            if (cam_position == 1) {
                LeanTween.move(cam, up_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(90, 90, 0), up_down_time);
            }
            if (cam_position == 2) {
                LeanTween.move(cam, up_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(90, 180, 0), up_down_time);
            }
            if (cam_position == 3) {
                LeanTween.move(cam, up_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(90, 270, 0), up_down_time);
            }
            is_top = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && is_top) {
            if (cam_position == 0) {
                LeanTween.move(cam, down_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(30, 0, 0), up_down_time);
            }
            if (cam_position == 1) {
                LeanTween.move(cam, down_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(30, 90, 0), up_down_time);
            }
            if (cam_position == 2) {
                LeanTween.move(cam, down_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(30, 180, 0), up_down_time);
            }
            if (cam_position == 3) {
                LeanTween.move(cam, down_path[cam_position], up_down_time);
                LeanTween.rotate(cam, new Vector3(30, 270, 0), up_down_time);
            }
            is_top = false;
        }

        if (cam_position % 4 == 0) {
            Direction.forward = new Vector3(0, 0, 1);
            Direction.left = new Vector3(-1, 0, 0);
            Direction.back = new Vector3(0, 0, -1);
            Direction.right = new Vector3(1, 0, 0);
        }
        if (cam_position % 4 == 1) {
            Direction.forward = new Vector3(1, 0, 0);
            Direction.left = new Vector3(0, 0, 1);
            Direction.back = new Vector3(-1, 0, 0);
            Direction.right = new Vector3(0, 0, -1);
        }
        if (cam_position % 4 == 2) {
            Direction.forward = new Vector3(0, 0, -1);
            Direction.left = new Vector3(1, 0, 0);
            Direction.back = new Vector3(0, 0, 1);
            Direction.right = new Vector3(-1, 0, 0);
        }
        if (cam_position % 4 == 3) {
            Direction.forward = new Vector3(-1, 0, 0);
            Direction.left = new Vector3(0, 0, -1);
            Direction.back = new Vector3(1, 0, 0);
            Direction.right = new Vector3(0, 0, 1);
        }
        
    }

}
