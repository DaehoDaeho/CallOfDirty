using UnityEngine;

public enum LimbType
{
    Head,
    Body,
    Leg
}

public class ZombieLimb : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ZombieController zombieController;

    [SerializeField]
    private LimbType type;

    [SerializeField]
    private float damageMultiplier = 1.0f;

    public void TakeDamage(float damageAmount)
    {
        float finalDamage = damageAmount * damageMultiplier;

        bool isLegHit = (type == LimbType.Leg);

        // 최종 데미지 적용.
        zombieController.TakeLimpDamage(finalDamage, isLegHit);
    }
}
