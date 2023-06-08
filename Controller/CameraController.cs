using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : SingleTon<CameraController>
{
    [Header("== 手动匹配的物件 ==")]
    public IUserInput pi;
    public Image lockImage;
    [Header("== 自动匹配的物件 ==")]
    public GameObject playerHandel;
    public GameObject cameraHandel;
    public GameObject model;
    public GameObject mainCamera;
    [Header("== 数据设置 ==")]
    public float horizantalSpeed = 100f;
    public float verticalSpeed = 80f;
    public float cameraDampSpeed=0.05f;
    public float cameraUpMax=40;
    public float cameraUpMin=-30;
    public bool lockState = false;
    public bool isAI = false;//是否为机器人AI

    [SerializeField]
    private LockTarget lockTarget;

    private float eulerX;
    private Vector3 cameraVeocity;
    private void Start()
    {
        cameraHandel = transform.parent.gameObject;
        playerHandel = cameraHandel.transform.parent.gameObject;
        PlayerController pc = playerHandel.GetComponent<PlayerController>();
        model = pc.model;
        pi = pc.pi;
        if (!isAI)
        {
            mainCamera = Camera.main.gameObject;
            //lockImage.enabled = false;
        }     
    }
    private void Update()
    {
        if (lockTarget!=null&&lockTarget.obj==null)
        {
            LockObj(null, false, false, isAI);
        }
        if (lockTarget!=null)
        {
            /*if (!isAI)
            {
                lockImage.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position +
                new Vector3(0, lockTarget.halfHeight, 0));
            }*/
            //距离超过10就自动断开锁定
            if (Vector3.Distance(model.transform.position,lockTarget.obj.transform.position)>10f)
            {
                LockObj(null,false,false,isAI);
            }
            //死亡时断开(未写)
            /*if (die)
            {
                LockObj(null,false,false,isAI);
            }*/
        }
    }
    
    private void FixedUpdate()
    {
        if (lockTarget==null)
        {
            Vector3 modelEuler = model.transform.eulerAngles;
            //改变摄像机左右朝向
            playerHandel.transform.Rotate(Vector3.up, horizantalSpeed * pi.cRight * Time.fixedDeltaTime);
            //改变摄像机上下朝向
            eulerX -= verticalSpeed * Time.fixedDeltaTime * pi.cUp;
            eulerX = Mathf.Clamp(eulerX, cameraUpMin, cameraUpMax);
            cameraHandel.transform.localEulerAngles = new Vector3(eulerX, 0, 0);
            //模型的方向不发生改变
            model.transform.eulerAngles = modelEuler;
        }
        else
        {
            Vector3 teamFoward = lockTarget.obj.transform.position - model.transform.position;//向量算法(终点-起点)
            teamFoward.y = 0;
            playerHandel.transform.forward = teamFoward;
            cameraHandel.transform.LookAt(lockTarget.obj.transform);
        }
        if (!isAI)
        {
            //摄像机位置改变
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref cameraVeocity, cameraDampSpeed);
            mainCamera.transform.LookAt(cameraHandel.transform);
        }  
    }

    public void LockUnLock()
    {
        Vector3 boxCenter = model.transform.position + Vector3.up + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter,new Vector3(0.5f,0.5f,5.0f),model.transform.rotation,LayerMask.GetMask(isAI?"player":"enemy"));
        if (cols.Length==0)
        {
            LockObj(null,false,false,isAI);
        }
        else
        {
            foreach (var item in cols)
            {
                if (lockTarget!=null&&item.gameObject==lockTarget.obj)
                {
                    LockObj(null,false,false,isAI);
                }
                else
                {
                    LockObj(new LockTarget(item.gameObject,item.bounds.extents.y),true,true,isAI);
                }
            }
        }
    }

    private void LockObj(LockTarget _lockTarget,bool _imageEnable,bool _lockState,bool ISAI)
    {
        lockTarget = _lockTarget;
        /*if (!ISAI)
        {
            lockImage.enabled=_imageEnable;
        }*/
        lockState = _lockState;
    }
    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}
