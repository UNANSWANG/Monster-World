using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider col;
    public float offsert=0.3f;

    private float radius;
    private Vector3 point1;
    private Vector3 point2;
    
    private void Awake()
    {
        radius = col.radius - 0.15f;
    }

    void Update()
    {
        point1 = transform.position + Vector3.up * (radius - offsert);//胶囊体下面那个圆的中心点 
        point2 = transform.position + Vector3.up * (col.height - offsert-radius);//上面那个圆的中心点
        Collider[] cols = Physics.OverlapCapsule(point1,point2,radius,LayerMask.GetMask("ground"));
        if (cols.Length!=0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("NotGround");
        }
    }
}
