using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitonPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }
    [Header("== 传送信息 ==")]
    public string sceneName;//不同场景时获取场景名字
    public TransitionType transitionType;
    public TransitonDestination.DestinationTag destinationTag;

    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&canTrans)
        {
            SceneController.Instance.TransitonToDestination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
