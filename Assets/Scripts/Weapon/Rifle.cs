using UnityEngine;

public class Rifle : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 10.0f;
        range = 100.0f;
        fireRate = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) == true && CanFire() == true)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        RaycastHit hit;

        bool isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range);

        if (isHit == true)
        {
            Debug.Log("º“√— ∏Ì¡ﬂ: " + hit.transform.name);
            //Target target = hit.transform.GetComponent<Target>();
            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
