using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originParent;//�ʼ�ĸ�����
    public Inventory myBag;
    public int currentItemID;//��ǰ��Ʒ��ID
    public void OnBeginDrag(PointerEventData eventData)
    {
        originParent = transform.parent;
        currentItemID = originParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;//�رո������������߼�⣬��ȡ�������·�������
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
                //����Ҫ�������������λ�ú͸�����
                Transform targetParent = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent;
                transform.SetParent(targetParent);
                transform.position = targetParent.position;
                //�洢���ݸ���
                var temp = myBag.itemList[currentItemID];
                int nextID = targetParent.GetComponent<Slot>().slotID;
                myBag.itemList[currentItemID] = myBag.itemList[nextID];
                myBag.itemList[nextID] = temp;
                //���������������λ�ú͸�����
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originParent);
                originParent = transform.parent;
                currentItemID = originParent.GetComponent<Slot>().slotID;
                break;
            case "Slot(Clone)":
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //�洢���ݸ���
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
