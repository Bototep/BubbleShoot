using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	[SerializeField] private GameObject currentProjectilePrefab;
	[SerializeField] private GameObject normalProjectilePrefab;
	[SerializeField] private GameObject penetrateProjectilePrefab;
	[SerializeField] private GameObject explodeProjectilePrefab;
	[SerializeField] private GameObject peneExploProjectilePrefab;
	[SerializeField] private Transform projectileSpawnPosition;
	[SerializeField] private float projectileSpeed = 10f;
	[SerializeField] private float fireRate = 0.5f;

	public float rotationOffset = -90f;
	public TMP_Text ammoTxt;

	private float _nextFireTime = 0f;
	private int penetrateBulletCount = 0;
	private int explodeBulletCount = 0;
	private int peneExploBulletCount = 0;
	private bool isRapidFireActive = false;
	private bool isInfinitePenetrateActive = false;
	private bool isExplodeRapidActive = false;
	private bool isPeneExploActive = false;

	void Update()
	{
		RotateToMouse();

		if (Input.GetMouseButtonDown(0) && (isRapidFireActive || Time.time >= _nextFireTime))
		{
			Shoot();
			if (!isRapidFireActive)
			{
				_nextFireTime = Time.time + fireRate;
			}
		}
	}

	private void Shoot()
	{
		if (currentProjectilePrefab != null && projectileSpawnPosition != null)
		{
			GameObject newProjectile = Instantiate(currentProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
			Vector3 shootDirection = transform.right;

			// If Explode + Rapid is active, don't worry about cooldowns or ammo
			if (isExplodeRapidActive && currentProjectilePrefab == explodeProjectilePrefab)
			{
				Debug.Log("Shooting Rapid Exploding bullets.");
			}
			else
			{
				// Normal behavior or other skill interactions
				if (currentProjectilePrefab == penetrateProjectilePrefab && !isRapidFireActive)
				{
					penetrateBulletCount++;
					if (penetrateBulletCount >= 5)
					{
						ChangeProjectile(normalProjectilePrefab);
						penetrateBulletCount = 0;
					}
				}
				else if (currentProjectilePrefab == explodeProjectilePrefab && !isRapidFireActive)
				{
					explodeBulletCount++;
					if (explodeBulletCount >= 1)
					{
						ChangeProjectile(normalProjectilePrefab);
						explodeBulletCount = 0;
					}
				}
				else if (currentProjectilePrefab == peneExploProjectilePrefab && !isRapidFireActive)
				{
					peneExploBulletCount++;
					if (peneExploBulletCount >= 1)
					{
						ChangeProjectile(normalProjectilePrefab);
						peneExploBulletCount = 0;
					}
				}
			}

			// Standard projectile shooting
			Projectile projectile = newProjectile.GetComponent<Projectile>();
			if (projectile != null)
			{
				projectile.Initialize(shootDirection, projectileSpeed);
			}
		}
	}

	private void RotateToMouse()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePosition - transform.position;
		direction.z = 0;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
	}

	public void ChangeToPenetrate()
	{
		if (penetrateProjectilePrefab != null)
		{
			ChangeProjectile(penetrateProjectilePrefab);
			penetrateBulletCount = 0;
		}
		else
		{
			Debug.LogWarning("PenetrateProjectilePrefab is not assigned.");
		}
	}

	public void ChangeToExplode()
	{
		if (explodeProjectilePrefab != null)
		{
			ChangeProjectile(explodeProjectilePrefab);
		}
		else
		{
			Debug.LogWarning("ExplodeProjectilePrefab is not assigned.");
		}
	}

	public void ChangeToPenetrateExplode()
	{
		if (peneExploProjectilePrefab != null)
		{
			ChangeProjectile(peneExploProjectilePrefab);
		}
		else
		{
			Debug.LogWarning("PenetrateExplodeProjectilePrefab is not assigned.");
		}
	}

	public void RapidFire()
	{
		StartCoroutine(RapidFireCoroutine());
	}

	private System.Collections.IEnumerator RapidFireCoroutine()
	{
		isRapidFireActive = true;

		yield return new WaitForSeconds(3f); // Rapid fire lasts for 3 seconds

		isRapidFireActive = false;

		if (isExplodeRapidActive)
		{
			isExplodeRapidActive = false;
			Debug.Log("Explode + Rapid Disabled");
		}

		if (isInfinitePenetrateActive)
		{
			isInfinitePenetrateActive = false;
			Debug.Log("Infinite Penetrate Disabled");
		}
	}

	public void EnableInfinitePenetrateDuringRapidFire()
	{
		isInfinitePenetrateActive = true;
		Debug.Log("Infinite Penetrate Enabled");
	}

	public void EnableExplodeDuringRapidFire()
	{
		isExplodeRapidActive = true;
		Debug.Log("Explode + Rapid Enabled");
	}

	public void EnableExplodeDuringPenetrate()
	{
		isPeneExploActive = true;
		Debug.Log("Explode + Penetrate Enabled");
	}

	private void ChangeProjectile(GameObject newProjectilePrefab)
	{
		if (newProjectilePrefab != null && newProjectilePrefab != currentProjectilePrefab)
		{
			currentProjectilePrefab = newProjectilePrefab;
			Debug.Log("Projectile changed to: " + newProjectilePrefab.name);
		}
		else
		{
			Debug.LogWarning("Attempted to change to the same projectile or null.");
		}
	}
}
