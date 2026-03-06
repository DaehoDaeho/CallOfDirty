using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float health = 20.0f;

    [SerializeField]
    private float explosionRadius = 7.0f;

    [SerializeField]
    private float explosionDamage = 100.0f;

    [SerializeField]
    private float explosionForce = 700.0f;

    [SerializeField]
    private GameObject explosionEffect;

    private bool isExploded = false;

    public void TakeDamage(float damageAmount)
    {
        if(isExploded == true)
        {
            return;
        }

        health -= damageAmount;
        if(health <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        isExploded = true;

        if(explosionEffect != null)
        {
            GameObject go = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(go, 3.0f);
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);

        for(int i=0; i<targets.Length; ++i)
        {
            IDamageable damageable = targets[i].GetComponent<IDamageable>();
            if(damageable != null && targets[i].gameObject != gameObject)
            {
                float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                float damagePercentage = 1.0f - (dist / explosionRadius);
                damageable.TakeDamage(explosionDamage * damagePercentage);
            }

            // 물리 효과 적용.
            Rigidbody rb = targets[i].GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
