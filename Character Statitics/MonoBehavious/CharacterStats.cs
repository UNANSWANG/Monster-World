using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack; 
    public CharacterData_SO tempCharactor_SO;
    public CharacterData_SO character_SO;
    public int level;
    public AttackData_SO attack_SO;
    private PlayerController pc;

    [HideInInspector]
    public bool isCritical;//是否暴击

    private AreaFinishController ar;
    private void Awake()
    {
        if (tempCharactor_SO!=null)
        {
            character_SO = Instantiate(tempCharactor_SO);
        }
        pc = GetComponent<PlayerController>();
        character_SO.SetLevel(level);
    }
    #region 读取 Data_SO中数据
    public int MaxHealth
    {
        get { return character_SO != null ? character_SO.maxHealth : 0; }
        set { character_SO.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { return character_SO != null ? character_SO.currentHealth : 0; }
        set { character_SO.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { return character_SO != null ? character_SO.baseDefence : 0; }
        set { character_SO.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { return character_SO != null ? character_SO.currentDefence : 0; }
        set { character_SO.currentDefence = value; }
    }
    #endregion
    #region "攻击动作"
    //怪物造成的伤害
    public void MosterTakeDamage(CharacterStats defence)
    {
        int damage = Mathf.Max(CurrentDamage()-defence.CurrentDefence,0);
        defence.CurrentHealth = Mathf.Max(defence.CurrentHealth-damage,0);
        if (isCritical)
            defence.pc.SetTrigger("hit");
        //UI更新
        defence.UpdateHealthBarOnAttack?.Invoke(defence.CurrentHealth,defence.MaxHealth);
        //经验更新
        if (defence.CurrentHealth <= 0)
        {
            defence.pc.SetTrigger("die");
            defence.GetComponent<Rigidbody>().isKinematic = true;
            defence.GetComponent<Collider>().isTrigger = true;
            Destroy(defence.gameObject,4f);
            StartCoroutine(EndGame());
        }
    }
    //角色造成的伤害
    public void PlayerTakeDamage(CharacterStats defence)
    {
        int currentDamage = Mathf.Max(CurrentDamage() - defence.CurrentDefence, 0);
        if (pc.ChenckStateTag("cut"))
        {
            currentDamage *= 2;
        }
        defence.CurrentHealth = Mathf.Max(defence.CurrentHealth-currentDamage,0);
        if (isCritical)
            defence.GetComponent<Animator>().SetTrigger("hit");
        defence.UpdateHealthBarOnAttack?.Invoke(defence.CurrentHealth,defence.MaxHealth);
        //经验更新
        if (defence.CurrentHealth <= 0)
        {
            character_SO.UpdateExp(defence.character_SO.killPoint);
            defence.GetComponent<Animator>().SetTrigger("die");
            defence.GetComponent<Collider>().isTrigger = true;
            Destroy(defence.gameObject, 2f);
        }
    }
    //怪物造成的直接数值伤害
    public void TakeDamage(int damage,CharacterStats defence)
    {
        int currentDamage = Mathf.Max(damage-defence.CurrentDefence,0);
        defence.CurrentHealth = Mathf.Max(defence.CurrentHealth-damage,0);
        defence.UpdateHealthBarOnAttack?.Invoke(defence.CurrentHealth, defence.MaxHealth);
        if (defence.CurrentHealth<=0)
        {
            defence.pc.SetTrigger("die");
            defence.GetComponent<Rigidbody>().isKinematic = true;
            defence.GetComponent<Collider>().isTrigger = true;
            Destroy(defence.gameObject,4f);
        }
    }
    //角色造成的直接数值伤害
    public void PlayerTakeDamageNum(int damage, CharacterStats defence)
    {
        int currentDamage = Mathf.Max(damage - defence.CurrentDefence, 0);
        defence.CurrentHealth = Mathf.Max(defence.CurrentHealth - damage, 0);
        defence.UpdateHealthBarOnAttack?.Invoke(defence.CurrentHealth, defence.MaxHealth);
        if (defence.CurrentHealth <= 0)
        {
            defence.GetComponent<Animator>().SetTrigger("die");
            defence.GetComponent<Collider>().isTrigger = true;
            Destroy(defence.gameObject, 4f);
        }
    }
    public int CurrentDamage()
    {
        float damage = UnityEngine.Random.Range(attack_SO.minDamge,attack_SO.maxDamge);
        if (isCritical)
        {
            damage *= attack_SO.criticalMultiplier;
        }
        return (int)damage;
    }
    #endregion
    public void SetAreaFinishi(AreaFinishController areaFIinish)
    {
        ar = areaFIinish;
    }

    private void OnDisable()
    {
        if (ar!=null)
        {
            ar.monsters.Remove(this.transform);
        }
    }

    IEnumerator EndGame()
    {
        GameManager.Instance.NotifyObserve();
        yield return new WaitForSeconds(4f);
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToMain();
    }
}
