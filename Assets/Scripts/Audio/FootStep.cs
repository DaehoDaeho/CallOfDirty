using UnityEngine;

public class FootStep : MonoBehaviour
{
    [SerializeField]
    private AudioSource footstepSource;

    [SerializeField]
    private AudioClip dirtClip;

    [SerializeField]
    private AudioClip metalClip;

    [SerializeField]
    private AudioClip woodClip;

    [SerializeField]
    private AudioClip defaultClip;

    [SerializeField]
    private Transform rayStartPoint;

    [SerializeField]
    private float rayDistance = 1.5f;

    [SerializeField]
    private LayerMask groundMask;

    public void PlayFootStep()
    {
        RaycastHit hit;
        if(Physics.Raycast(rayStartPoint.position, Vector3.down, out hit, rayDistance, groundMask) == true)
        {
            string surfaceTag = hit.collider.gameObject.tag;
            AudioClip clipToPlay = null;
            
            switch(surfaceTag)
            {
                case "Dirt":
                    {
                        clipToPlay = dirtClip;
                    }
                    break;

                case "Metal":
                    {
                        clipToPlay = metalClip;
                    }
                    break;

                case "Wood":
                    {
                        clipToPlay = woodClip;
                    }
                    break;

                default:
                    {
                        clipToPlay = defaultClip;
                    }
                    break;
            }

            if(clipToPlay != null)
            {
                footstepSource.clip = clipToPlay;
                footstepSource.volume = Random.Range(0.8f, 1.0f);
                footstepSource.pitch = Random.Range(0.9f, 1.1f);
                footstepSource.Play();
            }
        }    
    }
}
