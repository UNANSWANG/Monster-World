using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController :IUserInput
{
    [Header("== 物品 ==")]
    public GameObject model;
    public Animator anim;
    private void Awake()
    {
        anim = model.GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(AI());
    }
    IEnumerator AI()
    {
        while (true)
        {
            attack = true;
            yield return null;
        }
    }

}
