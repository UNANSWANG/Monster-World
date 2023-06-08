using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OprationAbleController : MonoBehaviour
{
    public AreaFinishController area;
    public List<GameObject> objList;
    public float force;
    Transform point;
    

    public void Awake()
    {
        area = GetComponentInParent<AreaFinishController>();
        point=transform.parent.GetChild(2);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&area.canOpen)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                transform.parent.GetChild(0).GetComponent<Animator>().SetTrigger("Open");
                if (objList.Count>0)
                {
                    Vector3 direction = other.gameObject.transform.position - point.position;
                    direction.Normalize();
                    for (int i = 0; i < objList.Count; i++)
                    {
                        Rigidbody tempRig = Instantiate(objList[i], point.position, point.rotation).GetComponent<Rigidbody>();
                        tempRig.AddForce(direction*force,ForceMode.Impulse);
                    }
                }
                Destroy(area.gameObject,2f);
            }
        }
    }
}
