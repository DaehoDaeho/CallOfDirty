using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle = 0,   // 대기.
    Patrol = 1, // 순찰.
    Chase = 2   // 추적.
}

public class ZombieFSM : MonoBehaviour
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

    //[SerializeField]
    //private float patrolRadius = 10.0f; // 순찰 반경.

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
            agent.SetDestination(targetPlayer.position);
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

        // 플레이어가 감지 거리 내에 있고, 현재 상태가 추적 상태가 아니면 추적 상태로 전이.
        //if(distanceToPlayer <= detectionRange && currentState != EnemyState.Chase)
        //{
        //    ChangeState(EnemyState.Chase);
        //}
        //// 플레이어가 감지 거리 바깥으로 멀어졌고 현재 상태가 추적 상태라면 순찰 상태로 전이.
        //else if(distanceToPlayer > detectionRange && currentState == EnemyState.Chase)
        //{
        //    ChangeState(EnemyState.Patrol);
        //}

        if(currentState != EnemyState.Chase)
        {
            if(DetectPlayer(distanceToPlayer) == true)
            {
                ChangeState(EnemyState.Chase);
            }
        }
        else
        {
            if(distanceToPlayer > viewDistance)
            {
                ChangeState(EnemyState.Patrol);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
