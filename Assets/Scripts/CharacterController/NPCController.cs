using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCController : MonoBehaviour
{
    public Events.EventMobDeath OnMobDeath;

    public MobType mobType;

    public float patrolTime = 15;
    public float aggroRange = 10;
    public Transform[] waypoints;
    public AttackDifination attack;
    [HideInInspector] public int nextWaypoint;
    private int index;
    private float speed, agentSpeed;
    private Transform player;

    private Animator animator;
    public NavMeshAgent agent;

    [Header("used for dibuging agrro radius")]
    public bool showAggro = true;

    private float timeOfLastAttack;

    public Transform SpellHotSpot;

    public bool playerIsAlive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if(agent != null) { agentSpeed = agent.speed; }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        index = Random.Range(0, waypoints.Length);

        MobManager mobManager = FindObjectOfType<MobManager>();
        if(mobManager != null)
        {
            OnMobDeath.AddListener(mobManager.OnMobDeth);
        }

        agent.updateRotation = false;
        agent.updatePosition = false;

        InvokeRepeating("Tick", 0, 0.5f);

        if(waypoints.Length > 0)
        {
            //InvokeRepeating("Patrol", 0, patrolTime);
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
        }

        timeOfLastAttack = float.MinValue;
        playerIsAlive = true;

       player.gameObject.GetComponent<DestructedEvent>().IDied += playerDied;
    }

    private void playerDied()
    {
        playerIsAlive = false;
    }

    private void Update()
    {
        speed = Mathf.Lerp(speed, agent.velocity.magnitude, Time.deltaTime * 10);
        animator.SetFloat("Forward", speed);

        float timeSinceLastAttack = Time.time - timeOfLastAttack;
        bool attackOnCooldown = timeSinceLastAttack < attack.Cooldown;
        agent.isStopped = attackOnCooldown;

        if (playerIsAlive)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool attackInRange = distanceFromPlayer < attack.Range;

            if (!attackOnCooldown && attackInRange)
            {
                agent.isStopped = true;
                transform.LookAt(player.transform);
                timeOfLastAttack = Time.time;
                
                if(attack is Weapon)
                {
                    animator.SetTrigger(ProjectConstants.vars.SWORD_ATTACK);
                }
                //  else if(attack is Spell)
                // {
                //     animator.SetTrigger("Attack2");
                //  }
                //agent.isStopped = false;
            }
        }

       
    }

    public void OnHit()
    {
        if (!playerIsAlive)
            return;

        if(attack is Weapon)
        {
            ((Weapon)attack).ExecuteAttack(gameObject, player.gameObject);
        }
       // else if(attack is Spell)
       // {
          //  ((Spell)attack).Cast(gameObject, SpellHotSpot.position, player.transform.position, LayerMask.NameToLayer("EnamySpells"));
      //  }
    }

    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
    }

    void Tick()
    {
        agent.destination = waypoints[index].position;
        agent.speed = agentSpeed;
        agent.transform.LookAt(waypoints[index].position);
        if (player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
        {
            agent.transform.LookAt(player.position);
            agent.destination = player.position;
            agent.speed = agentSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (showAggro)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
        
    }

}
[System.Serializable]
public enum MobType
{
    Shoter,
    Slasher
}
