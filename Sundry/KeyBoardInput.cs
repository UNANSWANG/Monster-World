using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : IUserInput
{
    [Header("== 键盘输入 ==")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyRigiht = "d";
    public string KeyLeft = "a";
    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;
    public string KeyE;
    public string KeyF;
    //控制摄像头方向
    public string CKeyUp;
    public string CKeyDown;
    public string CKeyRight;
    public string CKeyLeft;

    [Header("== 鼠标控制 ==")]
    public static bool mouseEnable=true;
    public bool lastMouseEnable=false;
    public float mouseSensitivityX=1.0f;
    public float mouseSensitivityY=1.0f;
    [Header("== 按钮设置 ==")]
    public MyButton KeyAButton = new MyButton();
    public MyButton KeyBButton = new MyButton();
    public MyButton KeyCButton = new MyButton();
    public MyButton KeyDButton = new MyButton();
    public MyButton KeyEButton = new MyButton();
    public MyButton KeyFButton = new MyButton();
    void Update()
    {
        //按钮检测
        KeyAButton.Tick(Input.GetKey(KeyA));
        KeyBButton.Tick(Input.GetKey(KeyB));
        KeyCButton.Tick(Input.GetKey(KeyC));
        KeyDButton.Tick(Input.GetKey(KeyD));
        KeyEButton.Tick(Input.GetKey(KeyE));
        KeyFButton.Tick(Input.GetKey(KeyF));

        targetUp = (Input.GetKey(KeyUp) ? 1 : 0) - (Input.GetKey(KeyDown) ? 1 : 0);
        targetRight = (Input.GetKey(KeyRigiht) ? 1 : 0) - (Input.GetKey(KeyLeft) ? 1 : 0);
        if (mouseEnable)
        {
            if (mouseEnable!=lastMouseEnable)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            cUp = Input.GetAxis("Mouse Y")*mouseSensitivityY;
            cRight = Input.GetAxis("Mouse X")*mouseSensitivityX;
        }
        else
        {
            if (mouseEnable!=lastMouseEnable)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            cUp = (Input.GetKey(CKeyUp) ? 1 : 0) - (Input.GetKey(CKeyDown)?1:0);//摄像头上下控制键
            cRight = (Input.GetKey(CKeyRight) ? 1 : 0) - (Input.GetKey(CKeyLeft)?1:0);//摄像头左右控制键
        }
        lastMouseEnable = mouseEnable;//改变鼠标上次是否开启该模式
        pUp = Mathf.SmoothDamp(pUp, targetUp, ref velocityUp, 0.1f);//给向前的方向设置缓冲值
        pRight = Mathf.SmoothDamp(pRight, targetRight, ref velocityRight, 0.1f);//给向前的方向设置缓冲值

        if (!inputEnable)
        {
            pUp = 0;
            pRight = 0;
        }
        Vector2 vec = SquareToCircle(new Vector2(pRight, pUp));//用公式将方转圆
        float pRight2 = vec.x;
        float pUp2 = vec.y;

        pMeg = Mathf.Sqrt(pRight2 * pRight2 + pUp2 * pUp2);//角色在这个方向的大小
        pVec = pRight2 * transform.right + pUp2 * transform.forward;//角色的方向

        //按钮检测
        run = (KeyAButton.isPressing && !KeyAButton.isDelay) || KeyAButton.isExstending;//按下时不马上触发，松开后不马上结束
        jump = KeyBButton.onPress;//按下后过会再按一下
        roll = KeyAButton.onRelese && KeyAButton.isDelay;//连按两下
        lockOn = KeyEButton.onPress;
        attack = KeyCButton.onPress;
    }
}
