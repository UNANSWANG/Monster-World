using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("== 角色信息统计 ==")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
    [Header("== 死亡分数 ==")]
    public int killPoint;
    [Header("== 等级 ==")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;//等级增幅

    public float LevelMutiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Min(level,maxLevel);
        baseExp = (int)(baseExp * LevelMutiplier);
        currentExp = 0;
        maxHealth = (int)(maxHealth* LevelMutiplier);
        currentHealth = maxHealth;
        killPoint = (int)(killPoint* LevelMutiplier);
    }
    public void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel+1,0,maxLevel);
        baseExp = (int)(baseExp*LevelMutiplier);
        maxHealth = (int)(maxHealth*LevelMutiplier);
        currentHealth = maxHealth;
        currentExp = 0;
        Debug.Log("升级咯！！ " + currentLevel + "最大生命 " + maxHealth);
    }
}
