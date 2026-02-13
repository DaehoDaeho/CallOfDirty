using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    protected float damage;
    protected float range;
    protected float fireRate;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;

    protected float nextFireTime = 0.0f;

    public event Action OnEnemyHit;

    public AudioClip shootSound;

    public GameObject hitEffectPrefab;

    public virtual void Shoot()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        SoundManager.instance.PlaySFX(shootSound);

        Debug.Log("기본 무기 발사!!!!");
    }

    public bool CanFire()
    {
        if(Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 자식 클래스에서 적을 맞췄을 때 호출할 함수.
    /// </summary>
    protected void TriggerEnemyHit()
    {
        if(OnEnemyHit != null)
        {
            OnEnemyHit.Invoke();
        }
    }
}
