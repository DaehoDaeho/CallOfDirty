using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour, IDamageable
{   
    public float viewDistance = 15.0f;

    public float viewAngle = 60.0f;    // 시야각.

    public float hearingDistance = 5.0f;   // 청각 거리.

    public LayerMask obstacleMask;

    public Transform[] wayPoints;  // 웨이포인트의 배열.
    public int currentWaypointIndex = 0;   // 현재 목표 지점의 인덱스.

    public Transform targetPlayer;
    public FPSMovement playerMovement;

    public NavMeshAgent agent;

    public Animator animator;

    public float attackRange = 1.5f;   // 공격 가능 사거리.

    public float attackRate = 1.0f;    // 공격 속도 (1초에 1번)

    public float attackDamage = 10.0f; // 공격 대미지.

    public float maxHealth = 100.0f;

    public RagdollController ragdoll;

    public float currentHealth = 0.0f;

    public float lastAttackTime = 0.0f;    // 마지막 공격 시간.

    public float idleTimer = 0.0f;
    public float idleDuration = 2.0f;  // 2초 동안 대기 후 이동.

    private ZombieState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            targetPlayer = go.transform;
            playerMovement = go.GetComponent<FPSMovement>();
        }

        currentHealth = maxHealth;

        ChangeState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.Update();
    }

    public void ChangeState(ZombieState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// 애니메이션 이벤트 함수.
    /// 플레이어 캐릭터에 정해진 프레임에 대미지 적용.
    /// </summary>
    public void TakeDamage()
    {
        IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentState is DeadState)
        {
            return;
        }

        currentHealth -= damageAmount;

        if (!(currentState is ChaseState) && !(currentState is AttackState))
        {
            ChangeState(new ChaseState(this));
        }

        if (currentHealth <= 0)
        {
            ChangeState(new DeadState(this));
        }
    }

    /// <summary>
    /// 시각 및 청각 감지 여부를 판단.
    /// </summary>
    /// <param name="distance"></param>
    public bool DetectPlayer(float distance)
    {
        // 청각 감지 (거리 + 플레이어 이동 여부)
        // 등 뒤에 있어도 가깝고, 플레이어가 움직이면 감지.
        if (distance <= hearingDistance)
        {
            if (playerMovement != null && playerMovement.IsMoving() == true)
            {
                return true;
            }
        }

        // 시각 감지 (거리 + 시야각 + 장애물)
        if (distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - transform.position).normalized;

            // 자신의 정면과 타겟 방향 사이의 각도.
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            // 시야각의 절반 이내인지 체크.
            if (angle < viewAngle * 0.5f)
            {
                // 장애물 체크.
                if (Physics.Raycast(transform.position + Vector3.up, dirToTarget, distance, obstacleMask) == false)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
