using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadePrefab;   // 투사체 프리팹.

    [SerializeField]
    private Transform firePoint;    // 발사 위치.

    [SerializeField]
    private float throwForce = 15.0f;   // 던지는 힘.

    // Update is called once per frame
    void Update()
    {
        // 마우스 오른쪽 버튼.
        if(Input.GetMouseButtonDown(1) == true)
        {
            LaunchGrenade();
        }
    }

    void LaunchGrenade()
    {
        GameObject go = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);

        if(go != null)
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
