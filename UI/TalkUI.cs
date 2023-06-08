using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkUI : MonoBehaviour
{
    public GameObject DialogUI;
    Transform model;
    Vector3 direction;

    private void Awake()
    {
        model = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        direction = other.gameObject.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            model.forward = Vector3.Lerp(model.forward, direction, 0.5f);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogUI.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                DialogUI.SetActive(false);
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogUI.SetActive(false);
        }
    }
}
