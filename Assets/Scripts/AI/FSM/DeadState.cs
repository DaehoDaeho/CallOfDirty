using UnityEngine;

public class DeadState : ZombieState
{
    public DeadState(ZombieController zombieController) : base(zombieController)
    {

    }

    public override void Enter()
    {
        zombie.agent.isStopped = true;
        zombie.agent.enabled = false;
        zombie.GetComponent<Collider>().enabled = false;

        MissionEventBus.PublishEnemyKilled();

        if(zombie.ragdoll != null)
        {
            zombie.ragdoll.EnableRagdoll();
        }

        Object.Destroy(zombie.gameObject, 5.0f);
    }
}
