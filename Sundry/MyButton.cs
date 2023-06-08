using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public bool isPressing=false;//正在按压
    public bool onPress=false;//被按压
    public bool onRelese=false;//松手
    public bool isDelay=false;//延迟反应时间
    public bool isExstending=false;//后续延续时间

    public float extendTime = 0.15f;
    public float delayTime = 0.15f;

    public bool curState = false;//状态检测
    public bool lastState = false;
    Timer extendTimer = new Timer();
    Timer delayTimer = new Timer();

    public void Tick(bool input)
    {
        extendTimer.Tick();//执行该函数Tick()时同时触发计时器的Tick()
        delayTimer.Tick();

        curState = input;
        isPressing = curState;

        onPress = false;
        onRelese = false;
        isDelay = false;
        isExstending = false;

        if (curState!=lastState)
        {
            if (curState)//按下
            {
                onPress = true;
                StartTimer(delayTimer,delayTime);//因为是按下所以触发延迟
            }
            else//松开
            {
                onRelese = true;
                StartTimer(extendTimer, extendTime);//因为是松开所以触发延时的计时器
            }
        }
        lastState = curState;
        if (extendTimer.state==Timer.State.RUN)
        {
            isExstending = true;
        }
        if (delayTimer.state==Timer.State.RUN)
        {
            isDelay = true;
        }
    }

    private void StartTimer(Timer timer ,float duration)
    {
        timer.durationTime = duration;
        timer.Go();
    }
}
