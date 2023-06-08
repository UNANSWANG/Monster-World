using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IPlayerManagerInterface
{
    [Header("== 物品绑定 ==")]
    public GameObject WHR;
    public Collider colR;
    public WeaponController wc;
    
    void Start()
    {
        try
        {
            WHR = transform.DeepFind("WeaponHandleR").gameObject;
            wc = BindWeaponController(WHR);
            colR = WHR.GetComponentInChildren<Collider>();
        }
        catch (System.Exception)
        {

        }
        WeaponDisable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public WeaponController BindWeaponController(GameObject obj)
    {
        WeaponController temp;
        temp = obj.GetComponent<WeaponController>();
        if (temp==null)
        {
           temp= obj.AddComponent<WeaponController>();
        }
        temp.wm = this;
        return temp;
    }
    public void WeaponEnable()
    {
        colR.enabled = true;
    }
    public void WeaponDisable()
    {
        colR.enabled = false;
    }
}
