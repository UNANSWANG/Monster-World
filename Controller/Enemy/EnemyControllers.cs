using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum enemyState { GUARD,PATROL,CHASE,DEAD}
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControllers : MonoBehaviour,IEndGameObserve
{
    protected CharacterStats data;
    protected GameObject attackTarget;
    private enemyState state;
    private bool playerDead;
    private NavMeshAgent agent;
    private Animator anim;
    private Collider col;
    private Vector3 wayPoint;
    private Vector3 guardPos;
    private Quaternion guardRote;
    private float remainLookTime;
    private float lastAttackTime;
    private float speed;

    bool isDead;
    bool isChase;
    bool isFollow;
    bool isWalk;

    [Header("== 基础设置 ==")]
    public float sightRadius;//可视范围
    public float lookTime;//怪物的观察时间
    public bool isGuard;//是否是站桩敌人
    [Header("== 巡逻范围 ==")]
    public float patrolRange;


    private void Awake()
    {
        data = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        speed = agent.speed;
        guardPos = transform.position;
        guardRote = transform.rotation;
        remainLookTime = lookTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            state = enemyState.GUARD;
        }
        else
        {
            state = enemyState.PATROL;
            GetNewWayPoint();
        }
        GameManager.Instance.AddObserve(this);
    }
    private void Update()
    {
        if (data.CurrentHealth <= 0)
            isDead = true;
        if (!playerDead)
        {
            SwichState();
            SwichAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (!GameManager.isInitialized) return;
        GameManager.Instance.RemoveObserve(this);
    }
    void SwichAnimation()
    {
        anim.SetBool("walk",isWalk);
        anim.SetBool("chase",isChase);
        anim.SetBool("follow",isFollow);
        anim.SetBool("critical",data.isCritical);
        anim.SetBool("die",isDead);
    }
    void SwichState()
    {
        if (isDead)
        {
            state = enemyState.DEAD;
        }
        else if (FoundPlayer())
        {
            state = enemyState.CHASE;//发现敌人就进入追击状态
        }
        switch (state)
        {
            case enemyState.GUARD:
                isChase = false;
                if (transform.position!=guardPos)//如果不在观测点
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos-transform.position)<agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRote,0.01f);
                    }
                }
                break;
            case enemyState.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                if (Vector3.Distance(wayPoint,transform.position)<=agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookTime>0)
                    {
                        remainLookTime -= Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case enemyState.CHASE:
                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainLookTime > 0)
                    {
                        remainLookTime -= Time.deltaTime;
                        agent.destination = transform.position;
                    }
                    else if (isGuard)
                        state = enemyState.GUARD;
                    else
                        state = enemyState.PATROL;
                }
                else
                {
                    agent.isStopped = false;
                    isFollow = true;
                    agent.destination = attackTarget.transform.position;
                }
                if (CanAttack()||CanSkill())//在攻击范围内
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime<0)
                    {
                        lastAttackTime = data.attack_SO.coolDown;
                        //暴击判断
                        data.isCritical = Random.value < data.attack_SO.criticalChance;
                        Attack();
                    }
                }
                break;
            case enemyState.DEAD:
                col.enabled = false;
                agent.radius = 0;
                Destroy(gameObject,2);
                break;
            default:
                break;
        }
    }
    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (data.attack_SO.attackRange>=data.attack_SO.skillRange)
        {
            if (CanSkill())//先播放近的
            {
                anim.SetTrigger("skill");
            }
            else if (CanAttack())
            {
                anim.SetTrigger("attack");
            }
        }
        else
        {
            if (CanAttack())//技能范围比近身远，所以先判断近身的攻击
            {
                anim.SetTrigger("attack");
            }
            else if (CanSkill())
            {
                anim.SetTrigger("skill");
            }
        }
    }
    bool CanAttack()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= data.attack_SO.attackRange;
        else
            return false;
    }
    bool CanSkill()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= data.attack_SO.skillRange;
        else
            return false;
    }
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position,sightRadius);
        foreach (var tar in colliders)
        {
            if (tar.CompareTag("Player"))
            {
                attackTarget = tar.gameObject;
                return true;
            }
        }
        return false;
    }
    void GetNewWayPoint()
    {
        remainLookTime = lookTime;
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x+randomX,guardPos.y,guardPos.z+randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint,out hit,patrolRange,1)?hit.position:transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }
    //动画事件
    void Hit()
    {
        if (attackTarget!=null&&transform&&transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            data.MosterTakeDamage(targetStats);
        }
    }
    public void EndNotify()
    {
        anim.SetBool("win",true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
