using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    public enum State
    {
        IEDE,
        RUN,
        FINISHI
    }
    public State state;

    public float durationTime = 1.0f;
    public float elapseTime=0;

    public void Tick()
    {
        switch (state)
        {
            case State.IEDE:
                break;
            case State.RUN:
                elapseTime += Time.deltaTime;
                if (elapseTime>=durationTime)
                {
                    state = State.FINISHI;
                }
                break;
            case State.FINISHI:
                break;
            default:
                Debug.LogError("Timer处于异常状态");
                break;
        }
    }

    public void Go()
    {
        state = State.RUN;
        elapseTime = 0;
    }
}
