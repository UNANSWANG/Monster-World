using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerController pc;
    [Header("== 物品 ==")]
    public BattleManager bm;
    public WeaponManager wm;
    public CharacterStats stats;
    GameObject model;
    GameObject sensor;
    
    void Awake()
    {
        pc = GetComponent<PlayerController>();
        pc.pm = this;
        stats = GetComponent<CharacterStats>();
        model = pc.model;
        try
        {
            sensor = transform.Find("Sensor").gameObject;
        }
        catch (System.Exception)
        {
            //没找到sensor
        }
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
    }
    private void OnEnable()
    {
        GameManager.Instance.RigisterPlayer(stats);
    }
    private T Bind<T>(GameObject obj)where T:IPlayerManagerInterface
    {
        T temp;
        if (obj == null)
        {
            return null;
        }
        temp = obj.GetComponent<T>();
        if (temp==null)
        {
            temp= obj.AddComponent<T>();
        }
        temp.pm = this;
        return temp;
    }
}
