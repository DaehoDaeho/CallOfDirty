using UnityEngine;

public interface IDamageable
{
    // 함수 구현은 하지 않는다.
    // TakeDamage라는 이름의 함수를 가지고 있어야 한다라고 강제하는 규칙을 정의.
    void TakeDamage(float damageAmount);
}
