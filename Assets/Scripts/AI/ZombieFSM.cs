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

    [SerializeField]
    private float detectionRange = 10.0f;   // 감지 거리. (이 범위 안에 들어오면 추적 상태로 전이)

    [SerializeField]
    private float patrolRadius = 10.0f; // 순찰 반경.

    [SerializeField]
    private Transform targetPlayer;

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
        // pathPending : 경로 계산 중인지 여부.
        // remainingDistance : 남은 거리.
        if (agent.pathPending == false && agent.remainingDistance < 0.5f)
        {
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
                    SetRandomPatrolPoint();
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
    void SetRandomPatrolPoint()
    {
        // 내 위치를 기준으로 순찰 반경 안의 랜덤 좌표를 생성한다.
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        // 생성한 랜덤 좌표가 NavMesh 위의 유효한 좌표인지 체크한다.
        if(NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1) == true)
        {
            agent.SetDestination(hit.position);
        }
    }

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
        if(distanceToPlayer <= detectionRange && currentState != EnemyState.Chase)
        {
            ChangeState(EnemyState.Chase);
        }
        // 플레이어가 감지 거리 바깥으로 멀어졌고 현재 상태가 추적 상태라면 순찰 상태로 전이.
        else if(distanceToPlayer > detectionRange && currentState == EnemyState.Chase)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
