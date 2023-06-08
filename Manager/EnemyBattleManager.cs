using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleManager : MonoBehaviour
{
    public CharacterStats stats;
    private void Start()
    {
        stats = GetComponent<CharacterStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        WeaponController tempWC = other.GetComponentInParent<WeaponController>();
        if (tempWC == null)
        {
            return;
        }
        CharacterStats attacker = tempWC.wm.pm.stats;
        CharacterStats reciver = stats;

        if (other.tag == "weapon")
        {
            attacker.PlayerTakeDamage(reciver);
        }
    }
}
