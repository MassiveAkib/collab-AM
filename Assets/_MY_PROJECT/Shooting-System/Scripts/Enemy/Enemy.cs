using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    private Color color;
    public Renderer enemyRenderer;
    public float hp = 100f;

    public Vector3 maxPosition;
    public Vector3 minPosition;

    private NavMeshAgent agent;
    private Transform playerTransform;

    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;

    public Vector3 walkPoint;
    private bool isWalkPointSet = false;
    public float walkPointRange;

    public float fireDelay;
    private bool isAlreadyFire;

    public float sightRange, attackRange;
    public bool isPlayerInSightRange, isPlayerInAttackRange;//isWallCheck

    public Transform firePosition;
    public float fireDistance;

    private PlayerMove playerMove;
    public LineRenderer bulletLineRenderer;
    public Animator animator;

    public int attack;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashShoot = Animator.StringToHash("isShoot");

    private AudioSource audioSource = null;

    private bool isAllKillEnemyMode = false;

    private void Awake()
    {
        transform.localPosition = RandomPosition();
        agent = GetComponent<NavMeshAgent>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerTransform = playerMove.gameObject.transform;
        audioSource = GetComponent<AudioSource>();

        hp = GameStateManager.Instance.Stage * 100 - 50;
        attack = GameStateManager.Instance.Stage * 7;

        if (GameStateManager.Instance.mazeMode == eMazeMode.ALLKILLENEMY)
        {
            isAllKillEnemyMode = true;
        }
    }

    private void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //isWallCheck = Physics.CheckSphere(firePosition.position, 1f, whatIsWall);

        if (!isPlayerInSightRange && !isPlayerInAttackRange) Patrolling();

        if (isPlayerInSightRange && !isPlayerInAttackRange) ChasePlayer();

        if (isPlayerInSightRange && isPlayerInAttackRange )//&& !isWallCheck
        {
            AttackPlayer();
        }
        else if ((isPlayerInSightRange && isPlayerInAttackRange))
        {
            ChasePlayer();
        }
    }

    private void Patrolling()
    {
        if (!isWalkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);
        {
            
            animator.SetBool(hashMove, true);
        }

        Vector3 distToWalkPoint = transform.position - walkPoint;
        if (distToWalkPoint.sqrMagnitude <= 3f) isWalkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        Vector3 pos = transform.position;
        walkPoint = new Vector3(pos.x + randomX, pos.y, pos.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && !Physics.CheckSphere(walkPoint, 0.01f, whatIsWall))
        {
            isWalkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        animator.SetBool(hashMove, true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        animator.SetBool(hashMove, false);

        transform.LookAt(playerTransform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if(!isAlreadyFire)
        {
            isAlreadyFire = true;

            Fire();

            Invoke("EndFire", 0.1f);
            Invoke("ResetAttack", fireDelay);
        }
    }

    private void Fire()
    {
        animator.SetBool(hashShoot, true);

        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {
            hitPosition = hit.point;
            if (hit.transform == playerTransform)
            {
                playerMove.Damage(attack);
            }
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * fireDistance;
        }

        bulletLineRenderer.SetPosition(1, bulletLineRenderer.transform.InverseTransformPoint(hitPosition));
        bulletLineRenderer.gameObject.SetActive(true);
        audioSource.Play();

        Invoke("EndFire", 0.2f);
    }

    private void EndFire()
    {
        bulletLineRenderer.gameObject.SetActive(false);
        animator.SetBool(hashShoot, false);
    }

    private void ResetAttack()
    {
        isAlreadyFire = false;
    }

    public void OnDamage(float damage)
    {
        hp -= damage;

        color = Color.red;
        ChangeColor();
        color = Color.white;
        Invoke("ChangeColor", 0.1f);

        if (hp <= 0)
        {
            Die();

            if (!isAllKillEnemyMode)
            {
                Invoke("Spawn", 5f);
            }
            else
            {
                GameManager.Instance.EnemyKillCount++;
            }
        }
    }

    private void ChangeColor()
    {
        enemyRenderer.material.SetColor("_Color", color);
    }

    private void Spawn()
    {
        hp = GameStateManager.Instance.Stage * 100 - 50;
        transform.localPosition = RandomPosition();
        gameObject.SetActive(true);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(minPosition.x, maxPosition.x), transform.localPosition.y, Random.Range(minPosition.z, maxPosition.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(firePosition.position, 1f);
    }
}
