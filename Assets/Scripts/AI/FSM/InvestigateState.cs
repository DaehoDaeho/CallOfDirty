using UnityEngine;

public class InvestigateState : ZombieState
{
    private Vector3 targetPos;
    private float stayTimer = 0.0f;
    private float stayDuration = 3.0f;

    public InvestigateState(ZombieController zombieController, Vector3 noisePos) : base(zombieController)
    {
        targetPos = noisePos;
    }

    public override void Enter()
    {
        zombie.agent.isStopped = false;
        zombie.agent.SetDestination(targetPos);
        zombie.animator.SetBool("Move", true);
    }

    public override void Update()
    {
        float dist = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.position);
        if(zombie.DetectPlayer(dist) == true)
        {
            zombie.ChangeState(new ChaseState(zombie));
            return;
        }

        if (zombie.agent.pathPending == false && zombie.agent.remainingDistance < 0.5)
        {
            zombie.animator.SetBool("Move", false);
            stayTimer += Time.deltaTime;

            if(stayTimer >= stayDuration)
            {
                zombie.ChangeState(new PatrolState(zombie));
            }
        }
    }
}
