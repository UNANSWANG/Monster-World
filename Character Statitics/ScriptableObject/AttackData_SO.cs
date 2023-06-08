using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName = "Character Stats/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//攻击距离
    public float skillRange;//远程攻击的攻击距离
    public float coolDown;//CD（冷却时间）
    public int minDamge;//最小攻击数值
    public int maxDamge;//最大攻击数值

    public float criticalMultiplier;//倍率乘区，用以计算暴击伤害
    public float criticalChance;//暴击率
}
