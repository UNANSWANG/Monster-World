using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public enum RockState { HitPlayer,HitEnemy,HitNothing}
    [Header("== 基础设置 ==")]
    public float force;
    public RockState rockState;
    public int damage;
    public GameObject target;
    public GameObject breakEffect;
    float staticTime=0;

    private Rigidbody rb;
    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockState = RockState.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        //石头速度小于1就是静止
        if (rb.velocity.sqrMagnitude<1f)
        {
            rockState = RockState.HitNothing;
        }
        if (rockState==RockState.HitNothing)
        {
            staticTime += Time.deltaTime;
            if (staticTime >= 10f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FlyToTarget()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        direction = target.transform.position - transform.position + Vector3.up;
        direction.Normalize();

        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        //print(other.gameObject.name);
        switch (rockState)
        {
            case RockState.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<Rigidbody>().AddForce(direction*force,ForceMode.Impulse);
                    other.gameObject.GetComponent<PlayerController>().SetTrigger("hit");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStats>());

                    rockState = RockState.HitNothing;
                }
                break;
            case RockState.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.PlayerTakeDamageNum(damage,otherStats);
                    Instantiate(breakEffect,transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case RockState.HitNothing:
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
            rockState = RockState.HitEnemy;
            GetComponent<Rigidbody>().velocity = Vector3.one;
            Vector3 direction = transform.position - other.transform.position;
            direction.Normalize();
            GetComponent<Rigidbody>().AddForce(direction*20,ForceMode.Impulse);
        }
    }
}
