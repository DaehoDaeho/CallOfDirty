using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private PlayerHealth playerHealth;

    void OnEnable()
    {
        if(playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    void UpdateHealthUI(float percent)
    {
        if(healthImage != null)
        {
            healthImage.fillAmount = percent;
        }
    }
}
