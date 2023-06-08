using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originParent;//最开始的父物体
    public Inventory myBag;
    public int currentItemID;//当前物品的ID
    public void OnBeginDrag(PointerEventData eventData)
    {
        originParent = transform.parent;
        currentItemID = originParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;//关闭该物体的鼠标射线检测，获取该物体下方的物体
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject==null)
        {
            transform.parent = originParent;
            transform.position = originParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts=true;
            return;
        }
        switch (eventData.pointerCurrentRaycast.gameObject.transform.name)
        {
            case "ItemImage":
                //给需要交换的物体更换位置和父物体
                Transform targetParent = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent;
                transform.SetParent(targetParent);
                transform.position = targetParent.position;
                //存储数据更改
                var temp = myBag.itemList[currentItemID];
                int nextID = targetParent.GetComponent<Slot>().slotID;
                myBag.itemList[currentItemID] = myBag.itemList[nextID];
                myBag.itemList[nextID] = temp;
                //给被交换物体更改位置和父物体
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originParent);
                originParent = transform.parent;
                currentItemID = originParent.GetComponent<Slot>().slotID;
                break;
            case "Slot(Clone)":
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //存储数据更改
                int id = eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().slotID;
                myBag.itemList[id] = myBag.itemList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().slotID != currentItemID)
                    myBag.itemList[currentItemID] = null;
                originParent = transform.parent;
                currentItemID = originParent.GetComponent<Slot>().slotID;
                break;
            default:
                transform.position = originParent.position;
                transform.parent = originParent;
                break;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true; ;
    }
}
