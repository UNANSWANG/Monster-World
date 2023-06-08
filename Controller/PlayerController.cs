using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("== 物品 ==")]
    public GameObject model;
    public CameraController cacon;
    public PlayerManager pm;
    [Header("== 属性设置 ==")]
    public float walkSpeed=1.6f;
    public float runSpeed=2.5f;
    public float floatLerp=0.5f;
    public float verticalSpeed = 4.0f;
    public float rolllSpeed = .0f;
    public bool trackDirection = false;
    public bool canAttack=false;
    public Vector3 yVec;
    public Vector3 deltaPosition;
    public IUserInput pi;
    public Animator anim;

    [Space(10)]
    [Header("== 物理材质 ==")]
    public PhysicMaterial FrictionOne;
    public PhysicMaterial FrictionZero;

    [Space(20)]
    [Header("== 音效 ==")]
    public AudioClip jumpAudio;
    public AudioClip hitAudio;
    public AudioClip rollAudio;
    public AudioClip dieAudio;

    public AudioSource audioSource;
    private Vector3 planeVec;
    private bool lockPlane;
    private CapsuleCollider col;
    Rigidbody rigi;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        pi = GetComponent<IUserInput>();
        anim = model.GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
        audioSource = transform.GetChild(2).GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetTrigger("die");
        }
        //改变状态机的参数
        if (!cacon.lockState)//摄像机不在锁定状态
        {
            anim.SetFloat("forward", pi.pMeg * Mathf.Lerp(anim.GetFloat("forward"),(pi.run?2:1),floatLerp));//判断是走还是跑
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localVec = transform.InverseTransformVector(pi.pVec);
            anim.SetFloat("forward", localVec.z);
            anim.SetFloat("right", localVec.x*(pi.run?2:1));
        }
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if (pi.roll||rigi.velocity.magnitude>7f)//当按下跳跃或者刚体速度大于一定值(空中掉下)就翻滚
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }
        if (canAttack&&pi.attack)
        {
            anim.SetTrigger("attack");
        }
        if (pi.lockOn)
        {
            cacon.LockUnLock();
        }
        if (!cacon.lockState)
        {
            if (pi.inputEnable)
            {
                if (pi.pMeg > 0.1)
                {
                    model.transform.forward = Vector3.Slerp(model.transform.forward, pi.pVec, 0.3f);
                }
            }
            if (!lockPlane)
            {
                planeVec = pi.pVec * walkSpeed*(pi.run?runSpeed:1.0f);//刚体的平移速度
            }
        }
        else
        {
            if (!trackDirection)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planeVec.normalized;
            }
            if (!lockPlane)
            {
                planeVec = pi.pVec * walkSpeed * (pi.run?runSpeed:1.0f);//刚体的平移速度
            }
        }
    }

    private void FixedUpdate()
    {
        rigi.position += deltaPosition;
        rigi.velocity = new Vector3(planeVec.x,rigi.velocity.y,planeVec.z)+yVec;
        yVec = Vector3.zero;
        deltaPosition = Vector3.zero;
    }

    //检测状态
    private bool CheckState(string stateName,string layerName="Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }
    public bool ChenckStateTag(string tagName,string layerName="Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }
    //动画器传导的事件函数
    public void OnJumpEnter()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
        pi.inputEnable = false;
        lockPlane = true;
        yVec = new Vector3(0,verticalSpeed,0);
        trackDirection = true;//方向追踪
    }
    public void IsGround()
    {
        anim.SetBool("isGround",true);
    }
    public void NotGround()
    {
        anim.SetBool("isGround", false)
    }
    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlane = false;
        canAttack = true;
        trackDirection = false;
        col.material = FrictionOne;//在地面时将摩擦力设为1
    }
    public void OnGroundExit()
    {
        col.material = FrictionZero;//不在地面时将摩擦力设为0
    }
    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
    }
    public void OnAttack1hCUpdate()
    {
        yVec = model.transform.forward * anim.GetFloat("attack1hCVelocity");
    }
    public void OnUpdateMove(object _deltaPosition)
    {
        if (CheckState("attack1hC"))
        {
            deltaPosition += (0.8f*deltaPosition+0.2f*(Vector3)_deltaPosition)/1.0f;
        }
    }
    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlane = true;
    }
    public void OnRollEnter()
    {
        audioSource.clip = rollAudio;
        audioSource.Play();
        yVec = new Vector3(0, rolllSpeed, 0);
        pi.inputEnable = false;
        lockPlane = true;
        trackDirection = true;
    }
    public void OnDieEnter()
    {
        audioSource.clip = dieAudio;
        audioSource.Play();
        pi.inputEnable = false;
        planeVec = Vector3.zero;
    }
    public void OnHitEnter()
    {
        audioSource.clip = hitAudio;
        audioSource.Play();
        pi.inputEnable = false;
        planeVec = Vector3.zero;
    }

    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void SetBool(string name,bool value)
    {
        anim.SetBool(name,value);
    }


}
