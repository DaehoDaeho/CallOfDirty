using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected float damage;
    protected float range;
    protected float fireRate;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;

    protected float nextFireTime = 0.0f;

    public virtual void Shoot()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

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
}
