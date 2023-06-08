using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;//物体持有量
    [TextArea]
    public string itemInfo;//物体描述
    public bool equip;//是否可装备（可能是药水）
}
