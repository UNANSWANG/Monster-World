using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingleTon<InventoryManager>
{
    public Inventory myBag;
    public GameObject slotGrid;
    public GameObject emptySlot;
    public Text itemInfo;

    public List<GameObject> slots = new List<GameObject>();

    private void OnEnable()
    {
        RefreshItem();
        Instance.itemInfo.text = "";
    }

    public static void UpdateInfo(string itemDescrition)
    {
        Instance.itemInfo.text = itemDescrition;
    }

    public static void RefreshItem()
    {
        //清空背包内物品
        for (int i = 0; i < Instance.slotGrid.transform.childCount; i++)
        {
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
        }
        Instance.slots.Clear();
        //重新生成背包内物品
        for (int i = 0; i < Instance.myBag.itemList.Count; i++)
        {
            Instance.slots.Add(Instantiate(Instance.emptySlot,Instance.slotGrid.transform));
            Instance.slots[i].GetComponent<Slot>().slotID = i;
            Instance.slots[i].GetComponent<Slot>().SetUpSlot(Instance.myBag.itemList[i]);
        }
    }
}
