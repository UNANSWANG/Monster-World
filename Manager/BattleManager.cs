using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IPlayerManagerInterface
{
    private CapsuleCollider col;
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        col.center = Vector3.up;//要稍微大点增加判定范围
        col.height = 2f;
        col.radius = 0.45f;
        col.isTrigger = true;
    }

 
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        WeaponController tempWC = other.GetComponentInParent<WeaponController>();
        if (tempWC==null)
        {
            return;
        }
        //攻击者拿handle，被攻击者拿模型
        CharacterStats attacker=tempWC.wm.pm.stats;
        CharacterStats reciver =pm.stats;

        if (other.tag=="weapon")
        {
            attacker.MosterTakeDamage(reciver);
        }
    }
}
