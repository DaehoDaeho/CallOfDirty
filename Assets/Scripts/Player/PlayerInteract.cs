using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private float interactRange = 3.0f; // 손이 닿는 거리.

    [SerializeField]
    private LayerMask interactableMask; // 아이템 레이어만 검사.

    [SerializeField]
    private Text interactPromptText;    // 화면 중앙 안내 텍스트.

    void Start()
    {
        // 텍스트 UI 빈 문자열로 초기화.
        interactPromptText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        // 레이캐스트.
        if(Physics.Raycast(ray, out hit, interactRange, interactableMask) == true)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if(interactPromptText != null)
                {
                    interactPromptText.text = interactable.GetInteractText();
                }

                if(Input.GetKeyDown(KeyCode.F) == true)
                {
                    interactable.Interact(gameObject);
                }
                return;
            }
        }

        if(interactPromptText != null)
        {
            interactPromptText.text = string.Empty;
        }
    }
}
