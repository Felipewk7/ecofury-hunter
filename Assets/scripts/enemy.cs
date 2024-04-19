using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public NavMeshAgent hunter;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float heath;

    //parulhando
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPoitRange;


    //ataque
    public float timeBetewwnAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //states

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private object tramsform;

    private void Awake()
    {
        player = GameObject.Find("mano").transform;
        hunter = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if (!playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (!playerInAttackRange && !playerInSightRange) AttackPlayer();

    }



    private void Patroling ()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            hunter.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude<1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPoitRange, walkPoitRange);
        float randomX= Random.Range(-walkPoitRange, walkPoitRange);

      walkPoint = new Vector3(transform.position.x, transform.position.y, randomZ);

        if(Physics.Raycast(walkPoint,-transform.up, 2f, whatIsGround))
            walkPointSet = true;



    }



    private void ChasePlayer ()
    {
        hunter.SetDestination(player.position);
    }
    private void AttackPlayer () 
    {
        hunter.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked) 
        
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetewwnAttacks);
        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamege(int damage)
    {
        heath -= damage;

        if (heath <= 0) Invoke(nameof(Destroyenemy), 0.5f);

       
    }
    private void Destroyenemy()
    {
        Destroy(gameObject);
    }
}
