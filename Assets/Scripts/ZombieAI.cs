using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject player;
    public LayerMask PlayerLayer;

    [Header("Zombie Agents")]
    public NavMeshAgent agent;
    [Header("Zombie Health")]
    private float zombieHealth = 100f;
    public float presentHealth;
    public float giveDamage = 5f;
    [Header("Zombie Attacking")]
    public float timeBetweenAttack;
    bool previouslyAttack;

    [Header("Zombie Animator")]
    public Animator anim;

    [Header("Zombie Walking")]
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public GameObject[] walkPoints;
    int currentZombiePosition = 0;
    public float zombieSpeed;
    float walkingpointRadius = 2;

    [Header("Zombie States")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInvisionRadius;
    public bool playerInAttackRadius;
    // Start is called before the first frame update
    private void Awake() {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInAttackRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);
        if (!playerInvisionRadius && !playerInAttackRadius) Walk();
        if (playerInvisionRadius && !playerInAttackRadius) PursuePlayer();
        if (playerInvisionRadius && playerInAttackRadius) AttackPlayer();

        // agent.SetDestination(player.transform.position);
    }
    void Walk()
    {
        if(Vector3.Distance(walkPoints[currentZombiePosition].transform.position, transform.position) < walkingpointRadius)
        {
            currentZombiePosition = Random.Range(0, walkPoints.Length);
            if(currentZombiePosition >= walkPoints.Length)
            {
                currentZombiePosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        // change zombie facing
        transform.LookAt(walkPoints[currentZombiePosition].transform.position);
    }
    void PursuePlayer()
    {
        if(agent.SetDestination(player.transform.position))
        {
            // animations
            anim.SetBool("Running", true);
        }
        else{
            anim.SetBool("Running", false);
        }
    }
    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if(!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("attacking" + hitInfo.transform.name);
                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                anim.SetBool("Attacking", true);
            }
            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }
    }
    void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void ZombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if (presentHealth <= 0)
        {
            anim.SetBool("Died", true);
            ZombieDie();
        }
    }
    private void ZombieDie()
    {
        // when it dies
        agent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInAttackRadius = false;
        playerInvisionRadius = false;
        Debug.Log("zombie dead");
    }
}
