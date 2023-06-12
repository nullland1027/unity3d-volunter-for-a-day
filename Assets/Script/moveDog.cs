using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class moveDog : MonoBehaviour
{
    public float moveDistance;
    public  Vector3 lastPosition;
    public LayerMask ignoreLayer;
    public LayerMask blockLayer;
    public GameObject target;

    public ParticleSystem particle;
    public ParticleSystem particleJump;
    public ParticleSystem particleJumpDown;

    public GameObject mainCamera;

    public bool canNotMove;


    private bool isJump;
    private bool isMove;
    private float interpolation_AddOnce = 0.025f;
    private float interpolation_IntervalTime = 0.005f;
    private float interpolation_Count;
    private Vector3 tempPos_Pervious;
    private Vector3 tempPos_Goal;
    private Vector3 tempPos_Current;
    private Animator animator;
    private bool isPlayer = false;
    private int activeNow = 0;

    private bool hasDoneJumpParticle;

 
    void Start()
    { 
        // 通过 name 判断，不能通过 Tag， Tag 将被用于代表谁是最后一个
        if(this.gameObject.name.Contains("Player")){
            isPlayer = true;
        }

        // 初始化
        isMove = false;   
        isJump = false;
        animator = GetComponent<Animator>();
        particle = transform.Find("Particle").gameObject.GetComponent<ParticleSystem>();
        particleJump = transform.Find("ParticleJump").gameObject.GetComponent<ParticleSystem>();
        particleJumpDown = transform.Find("ParticleJumpDown").gameObject.GetComponent<ParticleSystem>();
        mainCamera = GameObject.Find("CM");
        particle.Stop();
        particleJump.Stop();
        particleJumpDown.Stop();
        hasDoneJumpParticle = false;

        // 区分 Player 与 role
        if(isPlayer){
            // initialPlayerPosition();
        }
    }

    void Update()
    {
        // Debug.Log(canNotMove);

        // AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        // 优化动画切换
        if(isMove){
            if(isJump){
                // animator.Play("Jump");
                animator.SetBool("isJump", true);
                animator.SetBool("isRun", false);
                particle.Stop();
                // if (!hasDoneJumpParticle)
                // {
                //     particleJump.Play();
                //     hasDoneJumpParticle = true;
                // }
                
            } else {
                // animator.Play("Run");
                animator.SetBool("isJump", false);
                animator.SetBool("isRun", true);
                particle.Play();
                particleJump.Stop();
                particleJumpDown.Stop();
                hasDoneJumpParticle = false;
            }
            
        } else {
            animator.SetBool("isJump", false);
            animator.SetBool("isRun", false);
            particle.Stop();
            particleJump.Stop();
            particleJumpDown.Stop();
            hasDoneJumpParticle = false;
            // animator.Play("Base Layer.Idle",0);
        }
        
        if(isPlayer){
            GetInput();
        } else {
            // 判断当前 Role 是否被激活
            checkRoleTarget();
        }
    }

    // 子物体上的 rolePlayerCollision 脚本检测碰撞到 Player ，触发此脚本：设置 Target
    void setTarget(Collider other){
        if(target == null){
            // By Tag
            if(other.CompareTag("Player")){
                target = other.gameObject;
            }
        }
    }

    void GetInput(){
        if (canNotMove)
        {
            return;
        }

        if(isMove){
            return;
        }
        // Debug.Log(Direction.forward);

        if (Input.GetKey(KeyCode.W) && judgeWalk(Direction.forward))    //getKeyDown仅按下一瞬间为true；GetKey按下时一直为true
            StartCoroutine(Move_Up());
        if (Input.GetKey(KeyCode.S) && judgeWalk(Direction.back))
            StartCoroutine(Move_Back());
        if (Input.GetKey(KeyCode.A) && judgeWalk(Direction.left))
            StartCoroutine(Move_Left());
        if (Input.GetKey(KeyCode.D) && judgeWalk(Direction.right))
            StartCoroutine(Move_Right());
    }

    // 初始化人物位置到起点
    void initialPlayerPosition(){
        Transform a = Player_Global.SearchTheStartPos();
        this.gameObject.transform.position = a.position + Vector3.up*1.5f;
        Debug.Log("初始化人物位置： " + this.gameObject.transform.position.x + " " + this.gameObject.transform.position.y + " " + this.gameObject.transform.position.z);
    }

    // 检测游客是否被激活、是否进行移动
    void checkRoleTarget(){

        // 已被激活（ Role 四周碰撞体已碰撞过 Tag 为 Player（队列的最后一个人） 的物体）
        if(target != null){
            if(activeNow == 0){
                // 激活时：修改自身 Tag 为 Player，修改 Target‘s Tag 为 Role。
                // Debug.Log("修改TAG");
                this.transform.tag = "Player";
                target.GetComponent<moveDog>().tag = "Role";

                Player_Global.addSumOfCurrentQueue();
                Debug.Log("sumOfPeople: " + Player_Global.getSumOfCurrentQueue());
                activeNow = 1;
            }
            // 与 target 的距离 > 2.3 即 target 已经走向下一个格子
            if(distanceGreater()){
                if(!isMove){
                    isMove = true;
                    float x_gap = target.GetComponent<moveDog>().lastPosition.x - this.gameObject.transform.position.x;
                    float z_gap = target.GetComponent<moveDog>().lastPosition.z - this.gameObject.transform.position.z;
                    Vector3 dir = new Vector3(x_gap, 0, z_gap);
                    move(dir);
                }
            }
        }
    }

    // Role 的移动前处理
    void move(Vector3 dir){
        // if(isMove){
        //     return;
        // }
        // isMove = true;
        // 大于 0.4 是为了避免 role/play 间位置有较小误差影响判断
        if(dir.x > 0.4){
            if (judgeWalk(Vector3.right))
                StartCoroutine(Move_Right());
        } else if (dir.x < -0.4){
            if (judgeWalk(Vector3.left))
                StartCoroutine(Move_Left());
        } else if(dir.z > 0.4){
            if (judgeWalk(Vector3.forward))
                StartCoroutine(Move_Up());
        } else if(dir.z < -0.4){
            if (judgeWalk(Vector3.back))
                StartCoroutine(Move_Back());
        }
    }


    bool judgeWalk(Vector3 dir) {
        if(Player_Global.canWalk(this.gameObject, dir, ignoreLayer) != null) {
            GameObject nex = Player_Global.nextStand(this.gameObject, dir, ignoreLayer);
            GameObject cur = Player_Global.currentStand(this.gameObject, ignoreLayer);
            string cur_name = cur.name;
            string nex_name = nex.name;

            // 阻止行走（已踩过的方块）
            if (nex_name.Contains("WalkNone") && isPlayer){
                StartCoroutine(mainCamera.GetComponent<CameraShake>().Shake(0.15f, 0.2f));
                AudioManager.instance.PlayMusic(MusicType.Role_Block, gameObject);
                return false;
            }

            if (isPlayer && Player_Global.nextStandPeople(this.gameObject, dir) != null){
                // Debug.Log(Player_Global.nextStandPeople(this.gameObject, dir));
                StartCoroutine(mainCamera.GetComponent<CameraShake>().Shake(0.15f, 0.2f));
                AudioManager.instance.PlayMusic(MusicType.Role_Block, gameObject);
                return false;
            }

            int sumPeopleInQueue = Player_Global.getSumOfCurrentQueue();
            // hasFinishWalking 判断：剩余未被踩过的砖数 == 人物数
            // 阻止行走（终点）
            if(nex_name.Contains("End") && !Player_Global.hasFinishWalking(sumPeopleInQueue) && isPlayer){
                // 下一步为终点且未踩过所有 Once 型砖块时，阻止行动
                Debug.Log("你还没完成，别往终点走！");
                StartCoroutine(mainCamera.GetComponent<CameraShake>().Shake(0.15f, 0.2f));
                AudioManager.instance.PlayMusic(MusicType.Role_Block, gameObject);
                return false;
            }

            // 确定会走向下一步，更新 lastPosition
            lastPosition = this.gameObject.transform.position;

            // 通关判断
            if(nex_name.Contains("End") && Player_Global.hasFinishWalking(sumPeopleInQueue) && isPlayer) {
                Debug.Log("sumPeopleInQueue:" + sumPeopleInQueue);
                Debug.Log("本关通过！！！");        //
                canNotMove = true;
                GameObject.Find("Canvas").transform.Find("WinOne").gameObject.SetActive(true);
            }

            // 角色转向
            this.gameObject.transform.LookAt(this.gameObject.transform.position + dir);

            if(cur_name.Contains("low") && nex_name.Contains("high") || cur_name.Contains("middle") && nex_name.Contains("high")){    
                // low/middle => high
                StartCoroutine(jumpLowMidlletoHigh(dir, nex));
                AudioManager.instance.PlayMusic(MusicType.Role_Jump, gameObject);
                return false; 

            } else if(cur_name.Contains("low") && nex_name.Contains("middle")){   
                // low => middle
                StartCoroutine(jumpLowtoMiddle(dir, nex));
                AudioManager.instance.PlayMusic(MusicType.Role_Jump, gameObject);
                return false;

            } else if(cur_name.Contains("middle") && nex_name.Contains("low") || cur_name.Contains("high") && nex_name.Contains("low")){
                // middle => low / high => low
                StartCoroutine(jumpMiddleHightoLow(dir, nex));
                AudioManager.instance.PlayMusic(MusicType.Role_Jump, gameObject);
                return false; 

            } else if (cur_name.Contains("high") && nex_name.Contains("middle")){
                // high => middle
                StartCoroutine(jumpHightoMiddle(dir, nex));
                AudioManager.instance.PlayMusic(MusicType.Role_Jump, gameObject);
                return false; 
            } else {
                AudioManager.instance.PlayMusic(MusicType.Role_Walk, gameObject);
                return true;
            }
        } else {
            // 阻止行走（虚空）
            Debug.Log("我不能往前走！");
            StartCoroutine(mainCamera.GetComponent<CameraShake>().Shake(0.15f, 0.2f));
            AudioManager.instance.PlayMusic(MusicType.Role_Block, gameObject);
            return false;
        }
    }

    // Role/Player 通用跳跃
    IEnumerator jumpLowMidlletoHigh(Vector3 dir, GameObject nex) {
        isMove = true;
        isJump = true;
        float keepJump = 0.6f;
        Vector3 pos1 = this.gameObject.transform.position;
        Vector3 pos2 = this.gameObject.transform.position + dir*0.05f + Vector3.up*1.5f;
        Vector3 pos3 = this.gameObject.transform.position + dir*0.8f + Vector3.up*2f;
        Vector3 pos4 = nex.transform.position + Vector3.up*2f;


        Vector3[] path_toHigh = {pos1, pos2, pos3, pos4};

        particleJump.Play();

        LeanTween.move(this.gameObject, path_toHigh, keepJump);
        yield return new WaitForSeconds(keepJump);
        
        particleJumpDown.Play();

        isMove = false;
        isJump = false;
    }

    IEnumerator jumpLowtoMiddle(Vector3 dir, GameObject nex) {
        isMove = true;
        isJump = true;
        float keepJump = 0.6f;
        Vector3 pos1 = this.gameObject.transform.position;
        Vector3 pos2 = this.gameObject.transform.position + dir*0.05f + Vector3.up*2f;
        Vector3 pos3 = this.gameObject.transform.position + dir*0.8f + Vector3.up*2.5f;
        Vector3 pos4 = nex.transform.position + Vector3.up*1.5f;
        Vector3[] path_toHigh = {pos1, pos2, pos3, pos4};

        particleJump.Play();

        LeanTween.move(this.gameObject, path_toHigh, keepJump);

                yield return new WaitForSeconds(keepJump);

        particleJumpDown.Play();

        isMove = false;
        isJump = false;
    }

    IEnumerator jumpMiddleHightoLow(Vector3 dir, GameObject nex) {
        isMove = true;
        isJump = true;

        float keepJump = 0.5f;
        Vector3 pos1 = this.gameObject.transform.position;
        Vector3 pos2 = this.gameObject.transform.position + dir*1.3f + Vector3.up*1f;
        Vector3 pos3 = this.gameObject.transform.position + dir*1.8f + Vector3.up*0.8f;
        Vector3 pos4 = nex.transform.position + Vector3.up*1f;
        Vector3[] path_toHigh = {pos1, pos2, pos3, pos4};

        particleJump.Play();

        LeanTween.move(this.gameObject, path_toHigh, keepJump);

                yield return new WaitForSeconds(keepJump);

        particleJumpDown.Play();

        isMove = false;
        isJump = false;
    }

    IEnumerator jumpHightoMiddle(Vector3 dir, GameObject nex) {
        isMove = true;
        isJump = true;

        float keepJump = 0.5f;
        Vector3 pos1 = this.gameObject.transform.position;
        Vector3 pos2 = this.gameObject.transform.position + dir*1.3f + Vector3.up*1f;
        Vector3 pos3 = this.gameObject.transform.position + dir*1.8f + Vector3.up*0.8f;
        Vector3 pos4 = nex.transform.position + Vector3.up*1.5f;
        Vector3[] path_toHigh = {pos1, pos2, pos3, pos4};

        particleJump.Play();

        LeanTween.move(this.gameObject, path_toHigh, keepJump);

                yield return new WaitForSeconds(keepJump);

        particleJumpDown.Play();

        isMove = false;
        isJump = false;
    }

    // 用于 Role
    bool distanceGreater(){
        // 与目标人物距离超过2.3：跟上
        float dis_x = Math.Abs(this.gameObject.transform.position.x - target.transform.position.x);
        float dis_z = Math.Abs(this.gameObject.transform.position.z - target.transform.position.z);
        if(Math.Sqrt(Mathf.Pow(dis_x, 2f) + Mathf.Pow(dis_z, 2f)) > 2.3){
            return true;
        } else {
            return false;
        }
    }




    // Player/Role 通用平地移动
    IEnumerator Move_Up()
    {
        isMove = true;
        if(isPlayer){
            transform.forward = Direction.forward;
        }else{
            transform.forward = new Vector3(0.0f, 0.0f, 1.0f);
        }
 

        interpolation_Count = 0;
        tempPos_Pervious = this.gameObject.transform.position;
        if(isPlayer){
            tempPos_Goal = new Vector3(transform.position.x + moveDistance * transform.forward.x, transform.position.y + moveDistance * transform.forward.y, transform.position.z + moveDistance * transform.forward.z);
        } else {
            tempPos_Goal = new Vector3(transform.position.x, transform.position.y , transform.position.z + moveDistance );
        }

        while (interpolation_Count + 0.0001f < 1)
        {
            interpolation_Count += interpolation_AddOnce;
            transform.position = (1 - interpolation_Count) * tempPos_Pervious + interpolation_Count * tempPos_Goal; //线性插值
            yield return new WaitForSeconds(interpolation_IntervalTime);
        }

        isMove = false;
    }

    IEnumerator Move_Back()
    {
        isMove = true;
        if(isPlayer){
                    transform.forward = Direction.back;

        }else{
                transform.forward = new Vector3(0.0f, 0.0f, -1.0f);

        }

        interpolation_Count = 0f;
        tempPos_Pervious = this.gameObject.transform.position;
        if(isPlayer){
                    tempPos_Goal = new Vector3(transform.position.x + moveDistance * transform.forward.x, transform.position.y + moveDistance * transform.forward.y, transform.position.z + moveDistance * transform.forward.z);

        }else{
                                tempPos_Goal = new Vector3(transform.position.x , transform.position.y , transform.position.z - moveDistance );

        }

        while (interpolation_Count + 0.0001f < 1)
        {
            interpolation_Count += interpolation_AddOnce;
            transform.position = (1 - interpolation_Count) * tempPos_Pervious + interpolation_Count * tempPos_Goal;

            yield return new WaitForSeconds(interpolation_IntervalTime);
        }
        isMove = false;
    }

    IEnumerator Move_Left()
    {
        isMove = true;
        if(isPlayer){
                    transform.forward = Direction.left;

        }else{
                            transform.forward = new Vector3(-1.0f, 0.0f, 0.0f);

        }

        interpolation_Count = 0;
        tempPos_Pervious = this.gameObject.transform.position;
        if(isPlayer){
            tempPos_Goal = new Vector3(transform.position.x + moveDistance * transform.forward.x, transform.position.y + moveDistance * transform.forward.y, transform.position.z + moveDistance * transform.forward.z);

        }else{
            tempPos_Goal = new Vector3(transform.position.x - moveDistance , transform.position.y , transform.position.z );

        }
        

        while (interpolation_Count + 0.0001f < 1)
        {
            interpolation_Count += interpolation_AddOnce;
            
            transform.position = (1 - interpolation_Count) * tempPos_Pervious + interpolation_Count * tempPos_Goal;

            yield return new WaitForSeconds(interpolation_IntervalTime);
        }
        isMove = false;
    }

    IEnumerator Move_Right()
    {
        isMove = true;
        if(isPlayer){
                    transform.forward = Direction.right;

        }else{
                            transform.forward = new Vector3(1.0f, 0.0f, 0.0f);

        }

        interpolation_Count = 0;
        tempPos_Pervious = this.gameObject.transform.position;
        if(isPlayer){
                    tempPos_Goal = new Vector3(transform.position.x + moveDistance * transform.forward.x, transform.position.y + moveDistance * transform.forward.y, transform.position.z + moveDistance * transform.forward.z);

        }else{
                                tempPos_Goal = new Vector3(transform.position.x + moveDistance , transform.position.y , transform.position.z );

        }

        while (interpolation_Count + 0.0001f < 1)
        {
            interpolation_Count += interpolation_AddOnce;
            transform.position = (1 - interpolation_Count) * tempPos_Pervious + interpolation_Count * tempPos_Goal;
            yield return new WaitForSeconds(interpolation_IntervalTime);
        }
        isMove = false;
    }

   
}
