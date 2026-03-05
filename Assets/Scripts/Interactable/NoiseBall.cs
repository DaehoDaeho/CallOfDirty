using UnityEngine;

public class NoiseBall : MonoBehaviour
{
    [SerializeField]
    private float noiseRange = 10.0f;

    [SerializeField]
    private LayerMask zombieLayer;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRange, zombieLayer);

        for(int i=0; i < hitColliders.Length; ++i)
        {
            INoiseHearable noiseHearable = hitColliders[i].GetComponent<INoiseHearable>();
            if(noiseHearable != null)
            {
                noiseHearable.OnHearNoise(transform.position, 1.0f);
            }
        }

        Destroy(gameObject);
    }
}
