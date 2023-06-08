using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFinishController : MonoBehaviour
{
    [Header("== ≈‰÷√ ==")]
    public GameSceneMenu gm;
    public List<Transform> monsters;
    public bool canOpen;
    public bool completeFlag;

    void Awake()
    {
        BindAreaFinish();
        gm = FindObjectOfType<GameSceneMenu>();
    }

    private void Update()
    {
        if (monsters.Count==0)
        {
            canOpen = true;
            if (!completeFlag)
            {
                gm.CompletedBattle();
                completeFlag = true;
            }
        }
    }
    void BindAreaFinish()
    {
        foreach (Transform item in transform)
        {
            CharacterStats temp = item.GetComponent<CharacterStats>();
            if (temp==null)
            {
                continue;
            }
            temp.SetAreaFinishi(this);
            monsters.Add(item);
        }
    }
}
