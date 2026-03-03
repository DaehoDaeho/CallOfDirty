using UnityEngine;
using System;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100.0f;

    [SerializeField]
    private CinemachineCamera deathCam;

    [SerializeField]
    private GameObject fpsCharacter;

    [SerializeField]
    private RagdollController ragdollCharacter;

    [SerializeField]
    private bool useRagdoll = true;

    private float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if(OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth / maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if(currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;        

        float healthPercent = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            healthPercent = 0.0f;
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(healthPercent);
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;

        if(OnDeath != null)
        {
            OnDeath.Invoke();
        }

        if(deathCam != null)
        {
            deathCam.Priority = 20;
        }

        fpsCharacter.SetActive(false);
        ragdollCharacter.gameObject.SetActive(true);

        if(useRagdoll == true)
        {
            ragdollCharacter.EnableRagdoll();
        }
        else
        {
            Animator animator = ragdollCharacter.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Dead");
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        float healthPercent = currentHealth / maxHealth;

        if(OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(healthPercent);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
