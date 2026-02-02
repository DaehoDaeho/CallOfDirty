using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle = 0,   // 대기.
    Patrol = 1, // 순찰.
    Chase = 2,   // 추적.
    Attack = 3, // 공격.
    Dead    // 사망.
}

public class ZombieFSM : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyState currentState;    // 현재 상태.

    //[SerializeField]
    //private float detectionRange = 10.0f;   // 감지 거리. (이 범위 안에 들어오면 추적 상태로 전이)
    [SerializeField]
    private float viewDistance = 15.0f;

    [SerializeField]
    private float viewAngle = 60.0f;    // 시야각.

    [SerializeField]
    private float hearingDistance = 5.0f;   // 청각 거리.

    [SerializeField]
    private LayerMask obstacleMask;

    [SerializeField]
    private Transform[] wayPoints;  // 웨이포인트의 배열.
    private int currentWaypointIndex = 0;   // 현재 목표 지점의 인덱스.

    [SerializeField]
    private Transform targetPlayer;
    private FPSMovement playerMovement;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float attackRange = 1.5f;   // 공격 가능 사거리.

    [SerializeField]
    private float attackRate = 1.0f;    // 공격 속도 (1초에 1번)

    [SerializeField]
    private float attackDamage = 10.0f; // 공격 대미지.

    [SerializeField]
    private float maxHealth = 100.0f;

    [SerializeField]
    private RagdollController ragdoll;

    private float currentHealth = 0.0f;

    private float lastAttackTime = 0.0f;    // 마지막 공격 시간.

    private float idleTimer = 0.0f;
    private float idleDuration = 2.0f;  // 2초 동안 대기 후 이동.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if(go != null)
        {
            targetPlayer = go.transform;
            playerMovement = go.GetComponent<FPSMovement>();
        }

        currentHealth = maxHealth;

        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Idle:
                {
                    UpdateIdle();
                }
                break;

            case EnemyState.Patrol:
                {
                    UpdatePatrol();
                }
                break;

            case EnemyState.Chase:
                {
                    UpdateChase();
                }
                break;

            case EnemyState.Attack:
                {
                    UpdateAttack();
                }
                break;
        }

        // 상태 전이 체크.
        CheckTransitions();
    }

    void UpdateIdle()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    void UpdatePatrol()
    {
        if (wayPoints.Length == 0)
        {
            return;
        }

        // pathPending : 경로 계산 중인지 여부.
        // remainingDistance : 남은 거리.
        if (agent.pathPending == false && agent.remainingDistance < 0.5f)
        {
            // 웨이포인트의 순서를 다음 순서로 갱신.
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;

            // 도착했으면 다시 대기 상태로 전이.
            ChangeState(EnemyState.Idle);
        }
    }

    void UpdateChase()
    {
        if(targetPlayer != null)
        {
            if(agent.enabled == true)
            {
                agent.SetDestination(targetPlayer.position);
                agent.isStopped = false;    // 추적 시 이동 재개.
            }
        }
    }

    void UpdateAttack()
    {
        agent.isStopped = true; // 이동 멈춤.

        if(targetPlayer != null)
        {
            Vector3 targetPosition = new Vector3(targetPlayer.position.x, transform.position.y, targetPlayer.position.z);

            // Transform.LookAt : 파라미터로 전달한 위치를 바라보게 만들어주는 함수.
            transform.LookAt(targetPosition);

            // 공격 주기 체크.
            if (Time.time >= lastAttackTime + attackRate)
            {
                lastAttackTime = Time.time;

                animator.SetTrigger("Attack");

                IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    /// <summary>
    /// 상태 변경.
    /// </summary>
    /// <param name="newState">변경할 상태</param>
    void ChangeState(EnemyState newState)
    {
        if(currentState == newState)
        {
            return;
        }

        currentState = newState;

        switch(currentState)
        {
            case EnemyState.Idle:
                {
                    idleTimer = 0.0f;
                    agent.ResetPath();
                    animator.SetBool("Move", false);
                }
                break;

            case EnemyState.Patrol:
                {
                    //SetRandomPatrolPoint();
                    if(wayPoints.Length > 0)
                    {
                        agent.SetDestination(wayPoints[currentWaypointIndex].position);
                    }
                    animator.SetBool("Move", true);
                }
                break;

            case EnemyState.Chase:
                {
                    animator.SetBool("Move", true);
                }
                break;
        }
    }

    /// <summary>
    /// 랜덤한 순찰 지점을 찾는다.
    /// </summary>
    //void SetRandomPatrolPoint()
    //{
    //    // 내 위치를 기준으로 순찰 반경 안의 랜덤 좌표를 생성한다.
    //    Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
    //    randomDirection += transform.position;

    //    NavMeshHit hit;

    //    // 생성한 랜덤 좌표가 NavMesh 위의 유효한 좌표인지 체크한다.
    //    if(NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1) == true)
    //    {
    //        agent.SetDestination(hit.position);
    //    }
    //}

    /// <summary>
    /// 조건을 체크해서 상태를 전이시킨다.
    /// </summary>
    void CheckTransitions()
    {
        if(targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        if(currentState == EnemyState.Chase)
        {
            if(distanceToPlayer <= attackRange)
            {
                ChangeState(EnemyState.Attack);
            }
            else if(distanceToPlayer > viewDistance)
            {
                ChangeState(EnemyState.Patrol);
            }
        }
        else if(currentState == EnemyState.Attack)
        {
            if(distanceToPlayer > attackRange)
            {
                ChangeState(EnemyState.Chase);
            }
        }
        else
        {
            if (DetectPlayer(distanceToPlayer) == true)
            {
                ChangeState(EnemyState.Chase);
            }
        }
    }

    /// <summary>
    /// 시각 및 청각 감지 여부를 판단.
    /// </summary>
    /// <param name="distance"></param>
    bool DetectPlayer(float distance)
    {
        // 청각 감지 (거리 + 플레이어 이동 여부)
        // 등 뒤에 있어도 가깝고, 플레이어가 움직이면 감지.
        if(distance <= hearingDistance)
        {
            if(playerMovement != null && playerMovement.IsMoving() == true)
            {
                return true;
            }
        }

        // 시각 감지 (거리 + 시야각 + 장애물)
        if(distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - transform.position).normalized;

            // 자신의 정면과 타겟 방향 사이의 각도.
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            // 시야각의 절반 이내인지 체크.
            if(angle < viewAngle * 0.5f)
            {
                // 장애물 체크.
                if(Physics.Raycast(transform.position + Vector3.up, dirToTarget, distance, obstacleMask) == false)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TakeDamage(float damageAmount)
    {
        if(currentState == EnemyState.Dead)
        {
            return;
        }

        currentHealth -= damageAmount;

        if(currentState != EnemyState.Chase && currentState != EnemyState.Attack)
        {
            ChangeState(EnemyState.Chase);
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ChangeState(EnemyState.Dead);

        agent.isStopped = true;
        agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        if(ragdoll != null)
        {
            ragdoll.EnableRagdoll();
        }

        Destroy(gameObject, 5.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
