using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;

        float targetY = isOpen == true ? 90.0f : 0.0f;
        transform.rotation = Quaternion.Euler(0.0f, targetY, 0.0f);
        Debug.Log(isOpen == true ? "¹®ÀÌ ¿­·È½À´Ï´Ù." : "¹®ÀÌ ´ÝÇû½À´Ï´Ù.");
    }
}
