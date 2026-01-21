using UnityEngine;

public class Shotgun : Weapon
{
    public int pelletsCount = 8;
    public float spreadAngle = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 5.0f;
        range = 30.0f;
        fireRate = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) == true && CanFire() == true)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        for(int i=0; i<pelletsCount; ++i)
        {
            // 반지름이 1인 원 안에서 랜덤한 점(x, y)가 반환.
            Vector2 randomCircle = Random.insideUnitCircle * spreadAngle * 0.1f;

            Vector3 shootDirection = fpsCamera.transform.forward;
            shootDirection.x += randomCircle.x;
            shootDirection.y += randomCircle.y;

            RaycastHit hit;

            bool isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range);

            if (isHit == true)
            {
                Debug.Log("샷건 파편 명중: " + hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();

                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }
}
