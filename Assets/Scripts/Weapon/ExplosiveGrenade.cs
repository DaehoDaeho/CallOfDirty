using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 5.0f;   // Æø¹ß ¹Ý°æ.

    [SerializeField]
    private float explosionForce = 700.0f;  // ¹Ð¾î³»´Â Èû.

    [SerializeField]
    private float damage = 50.0f;   // ´ë¹ÌÁö.

    [SerializeField]
    private GameObject explosionEffect; // Æø¹ß ÀÌÆåÆ® ÇÁ¸®ÆÕ.

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    /// <summary>
    /// Æø¹ß Ã³¸®.
    /// </summary>
    void Explode()
    {
        if(explosionEffect != null)
        {
            GameObject go = Instantiate(explosionEffect, transform.position, transform.rotation);
            if(go != null)
            {
                Destroy(go, 3.0f);
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for(int i=0; i<colliders.Length; ++i)
        {
            IDamageable damageable = colliders[i].GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
