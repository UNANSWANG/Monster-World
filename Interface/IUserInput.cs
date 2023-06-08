using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("== 基础输入 ==")]
    public float pUp;//人物的前后
    public float pRight;//人物的左右
    public float cUp;//摄像机的前后
    public float cRight;//摄像机的左右
    public float pMeg;//方换圆方程后得到的强度
    public Vector3 pVec;//方换圆方程后得到的向量
    [Header("== 其他设置 ==")]
    public bool inputEnable;
    public float targetUp;
    public float targetRight;
    public float velocityUp;
    public float velocityRight;
    public bool run = false;
    public bool jump = false;
    public bool roll = false;
    public bool lockOn = false;
    public bool attack = false;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1-(input.y*input.y)/2.0f);
        output.y = input.y * Mathf.Sqrt(1-(input.x*input.x)/2.0f);
        return output;
    }
}
