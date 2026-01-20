using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float range = 100.0f;

    [SerializeField]
    private float fireRate = 0.2f;

    [SerializeField]
    private Camera fpsCamera;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private GameObject hitEffectPrefab;

    private float nextTimetoFire = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + fireRate;

            Shoot();
        }
    }

    void Shoot()
    {
        if(muzzleFlash != null)
        {
            // 파티클 재생.
            muzzleFlash.Play();
        }

        RaycastHit hit;

        bool isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range);

        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.red, 1.0f);

        if(isHit == true)
        {
            Debug.Log("맞은 대상: " + hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hitEffectPrefab != null)
            {
                GameObject go = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(go, 2.0f);
            }
        }
    }
}
