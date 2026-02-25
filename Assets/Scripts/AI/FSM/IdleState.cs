using UnityEngine;

public class IdleState : ZombieState
{
    public IdleState(ZombieController zombieController) : base(zombieController)
    {
        
    }

    public override void Enter()
    {
        zombie.idleTimer = 0.0f;
        zombie.agent.ResetPath();
        zombie.animator.SetBool("Move", false);
    }

    public override void Update()
    {
        if(zombie.targetPlayer != null)
        {
            float dist = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.transform.position);
            // 감지 처리.
            if(zombie.DetectPlayer(dist) == true)
            {
                zombie.ChangeState(new ChaseState(zombie));
                return;
            }
        }

        zombie.idleTimer += Time.deltaTime;
        if(zombie.idleTimer >= zombie.idleDuration)
        {
            zombie.ChangeState(new PatrolState(zombie));
        }
    }
}
