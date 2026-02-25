using UnityEngine;

public class ZombieState
{
    protected ZombieController zombie;

    public ZombieState(ZombieController zombieController)
    {
        zombie = zombieController;
    }

    // virtual : 가상 함수
    // 해당 클래스를 상속하는 자식 클래스에서 이 함수를 재정의 할 수 있게 만들어 준다.
    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
