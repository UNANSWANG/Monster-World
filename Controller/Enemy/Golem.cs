using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyControllers
{
    [Header("== ¼¼ÄÜ ==")]
    public float kickForce = 25;
    public GameObject rockPrefab;
    public Transform handPos;

    public void KickOff()
    {
        if (attackTarget!=null&&transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<Rigidbody>().AddForce(direction*kickForce,ForceMode.Impulse);
            attackTarget.GetComponent<PlayerController>().SetTrigger("hit");
            data.MosterTakeDamage(targetStats);
        }
    }

    public void ThorwRock()
    {
        if (attackTarget!=null)
        {
            var rock = Instantiate(rockPrefab,handPos.position,Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
