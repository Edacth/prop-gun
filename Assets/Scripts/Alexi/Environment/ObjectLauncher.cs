using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// launches projectile objects with a given force
/// </summary>
public class ObjectLauncher : MonoBehaviour
{
    [Tooltip("THe projectile to launch")]
    [SerializeField] Projectile projectile;
    [Tooltip("Projectile launch force (world space)")]
    [SerializeField] Vector3 launchForce;
    [Tooltip("Maximum number of projectiles (pool)")]
    [SerializeField] int maxProjectiles = 10;
    [Tooltip("Transform to initialize and launch from")]
    [SerializeField] Transform launchPoint;
    [Tooltip("Lifetime of projectiles")]
    [SerializeField] float projectileLifetime = 5;

    Projectile[] projectiles; // projectile pool
    int activeCount; // number of active projectiles

    Projectile currentProjectile; // holder for current projectile

    // Start is called before the first frame update
    void Start()
    {
        projectiles = new Projectile[maxProjectiles];
        for(int i = 0; i < maxProjectiles; i++)
        {
            currentProjectile = Instantiate(projectile);
            currentProjectile.gameObject.SetActive(false);
            currentProjectile.GetComponent<MeshRenderer>().enabled = true;
            projectiles[i] = currentProjectile;
        }
        currentProjectile = null;
        activeCount = 0;
    }

    /// <summary>
    /// instantiate and launch the projectile
    /// </summary>
    public void Launch()
    {
        currentProjectile = GetProjectile();

        if(null == currentProjectile) { return; }

        currentProjectile.transform.position = launchPoint.transform.position;
        currentProjectile.transform.rotation = Quaternion.identity; // not good
        currentProjectile.myRigidbody.AddForce(launchForce);
        currentProjectile.StartLifetimeCountdown(projectileLifetime, this);
    }

    /// <summary>
    /// pulls a prijectile from the pool
    /// </summary>
    /// 
    /// <returns>
    /// the fresh projectile
    /// </returns>
    Projectile GetProjectile()
    {
        // if no active projectiles
        if(activeCount >= maxProjectiles)
        {
            Debug.LogWarning("No available projectiles");
            return null;
        }

        for(int i = 0; i < maxProjectiles; i++)
        {
            // if not already active
            if (! projectiles[i].gameObject.activeSelf)
            {
                activeCount++;
                currentProjectile = projectiles[i];
                currentProjectile.gameObject.SetActive(true);
                return currentProjectile;
            }
        }

        Debug.LogWarning("No projectiles found");
        return null;
    }

    /// <summary>
    /// recycle old projectile back into pool
    /// </summary>
    /// 
    /// <param name="oldProjectile">
    /// the projectile to recycle
    /// </param>
    public void RecycleProjectile(Projectile oldProjectile)
    {
        oldProjectile.gameObject.SetActive(false);
        oldProjectile.myRigidbody.velocity = Vector3.zero;

        for(int i = 0; i < maxProjectiles; i++)
        {
            if(projectiles[i] == oldProjectile)
            {
                activeCount--;
                return;
            }
        }

        Debug.LogWarning("Cannot recycle projectile into " + name);
    }
}
