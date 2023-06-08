using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyControllers
{
    [Header("== 技能 ==")]
    public float kickForce = 10;//推力

    public void KickOff()
    {
        if (attackTarget!=null)
        {
            transform.LookAt(attackTarget.transform);
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<Rigidbody>().AddForce(direction*kickForce,ForceMode.Impulse);
            attackTarget.GetComponent<PlayerController>().SetTrigger("hit");

        }
    }
}
