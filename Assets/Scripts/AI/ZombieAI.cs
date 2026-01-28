using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private Transform targetPlayer;

    [SerializeField]
    private float pathUpdateDelay = 0.2f;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    private float nextUpdateTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            targetPlayer = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPlayer == null)
        {
            return;
        }

        if(Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + pathUpdateDelay;

            agent.SetDestination(targetPlayer.position);
        }

        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        if(distance >= 10.0f)
        {
            agent.ResetPath();
        }

        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;

        if(animator != null)
        {
            animator.SetBool("Move", isMoving);
        }
    }
}
