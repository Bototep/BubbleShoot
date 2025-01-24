using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform projectileSpawnPosition;
	[SerializeField] private float projectileSpeed = 10f;
	[SerializeField] private float fireRate = 0.5f;
	private float _nextFireTime = 0f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.time >= _nextFireTime)
		{
			Shoot();
			_nextFireTime = Time.time + fireRate;
		}
	}

	private void Shoot()
	{
		if (projectilePrefab != null && projectileSpawnPosition != null)
		{
			GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
			Vector3 shootDirection = transform.right;

			Projectile projectile = newProjectile.GetComponent<Projectile>();
			if (projectile != null)
			{
				projectile.Initialize(shootDirection, projectileSpeed);
			}
			else
			{
				Debug.LogWarning("Projectile script not found on the projectile prefab!");
			}
		}
		else
		{
			Debug.LogWarning("ProjectilePrefab or ProjectileSpawnPosition is not assigned!");
		}
	}
}
