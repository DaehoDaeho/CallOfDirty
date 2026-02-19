using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private Image damageFlashImage;

    [SerializeField]
    private Image crosshairImage;

    [SerializeField]
    private float flashSpeed = 2.0f;    // 피격 효과가 사라지는 속도.

    [SerializeField]
    private Color hitMarkerColor = Color.red;

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private Weapon currentWeapon;   // 현재 사용중인 무기.

    [SerializeField]
    private DamageVignette damageVignette;
    
    private Color originalCrosshairColor;   // 크로스헤어 UI의 원래 색.

    private void Awake()
    {
        originalCrosshairColor = crosshairImage.color;
    }

    void OnEnable()
    {
        if(playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            playerHealth.OnHealthChanged += damageVignette.UpdateVignette;
        }

        if(currentWeapon != null)
        {
            currentWeapon.OnEnemyHit += ShowHitMarker;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
            playerHealth.OnHealthChanged -= damageVignette.UpdateVignette;
        }

        if (currentWeapon != null)
        {
            currentWeapon.OnEnemyHit -= ShowHitMarker;
        }
    }

    void UpdateHealthUI(float percent)
    {
        if(healthImage != null)
        {
            healthImage.fillAmount = percent;
        }

        if(percent < 1.0f && damageFlashImage != null)
        {
            StartCoroutine(DamageFlashRoutine());
        }
    }

    IEnumerator DamageFlashRoutine()
    {
        Color flashColor = damageFlashImage.color;
        flashColor.a = 0.5f;
        damageFlashImage.color = flashColor;

        // 서서히 투명해지게.
        while(damageFlashImage.color.a > 0.0f)
        {
            flashColor.a -= Time.deltaTime * flashSpeed;
            if(flashColor.a < 0.0f)
            {
                flashColor.a = 0.0f;
            }
            damageFlashImage.color = flashColor;
            yield return null;
        }
    }

    void ShowHitMarker()
    {
        if(crosshairImage != null)
        {
            StopCoroutine(HitMarkerRoutine());
            StartCoroutine(HitMarkerRoutine());
        }
    }

    IEnumerator HitMarkerRoutine()
    {
        crosshairImage.color = hitMarkerColor;
        yield return new WaitForSeconds(0.1f);
        crosshairImage.color = originalCrosshairColor;
    }
}
