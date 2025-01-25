using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	[SerializeField] private GameObject currentProjectilePrefab, normalProjectilePrefab, penetrateProjectilePrefab, explodeProjectilePrefab, peneExploProjectilePrefab;
	[SerializeField] private Transform projectileSpawnPosition;
	[SerializeField] private float projectileSpeed = 10f, fireRate = 0.5f;
	public float rotationOffset = -90f;
	public TMP_Text ammoTxt;
	public TMP_Text timeTxt;
	public SkillManager skillManager;
	public PlayerAnimController playerAnimController;

	private float _nextFireTime = 0f;
	private int penetrateBulletCount = 0, explodeBulletCount = 0, peneExploBulletCount = 0;
	private bool isRapidFireActive = false, isInfinitePenetrateActive = false, isExplodeRapidActive = false;

	void Update()
	{
		RotateToMouse();
		if (Input.GetMouseButtonDown(0) && (isRapidFireActive || Time.time >= _nextFireTime))
		{
			Shoot();
			if (!isRapidFireActive) _nextFireTime = Time.time + fireRate;
		}
	}

	private void Shoot()
	{
		if (currentProjectilePrefab != null && projectileSpawnPosition != null)
		{
			if (playerAnimController != null)
			{
				playerAnimController.ATK();
			}

			var newProjectile = Instantiate(currentProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
			Vector3 shootDirection = transform.right;

			if (currentProjectilePrefab == penetrateProjectilePrefab && !isRapidFireActive && ++penetrateBulletCount >= 5)
			{
				skillManager.isPenetrateActive = false;
				ChangeProjectile(normalProjectilePrefab);
				penetrateBulletCount = 0;
				UpdateAmmoText("Normal Bullet");
			}else if (currentProjectilePrefab == explodeProjectilePrefab && !isRapidFireActive && ++explodeBulletCount >= 1)
			{
				skillManager.isExplodeActive = false;
				ChangeProjectile(normalProjectilePrefab);
				explodeBulletCount = 0;
				UpdateAmmoText("Normal Bullet");
			}else if (currentProjectilePrefab == peneExploProjectilePrefab && !isRapidFireActive && ++peneExploBulletCount >= 1)
			{
				skillManager.isPenetrateActive = false;
				skillManager.isExplodeActive = false;
				ChangeProjectile(normalProjectilePrefab);
				peneExploBulletCount = 0;
				UpdateAmmoText("Normal Bullet");
			}else
			{
				UpdateAmmoText("Normal Bullet");
			}

			var projectile = newProjectile.GetComponent<Projectile>();
			if (projectile != null) projectile.Initialize(shootDirection, projectileSpeed);
		}
	}

	private void RotateToMouse()
	{
		Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		direction.z = 0;
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset);
	}

	public void ChangeToPenetrate() { if (penetrateProjectilePrefab != null) ChangeProjectile(penetrateProjectilePrefab); }
	public void ChangeToExplode() { if (explodeProjectilePrefab != null) ChangeProjectile(explodeProjectilePrefab); }
	public void ChangeToPenetrateExplode()
	{
		if (peneExploProjectilePrefab != null)
		{
			currentProjectilePrefab = peneExploProjectilePrefab;
			UpdateAmmoText("Penetrate + Explode");
		}
	}

	public void RapidFire() { StartCoroutine(RapidFireCoroutine()); }
	private System.Collections.IEnumerator RapidFireCoroutine()
	{
		isRapidFireActive = true;
		UpdateAmmoText("Rapid Fire");
		yield return new WaitForSeconds(3f);
		isRapidFireActive = false;
		UpdateAmmoText("Normal Bullet");
		if (isExplodeRapidActive) { isExplodeRapidActive = false; }
		if (isInfinitePenetrateActive) { isInfinitePenetrateActive = false; }
		isRapidFireActive = false;
	}

	public void EnableInfinitePenetrateDuringRapidFire() { isInfinitePenetrateActive = true; UpdateAmmoText("Infinite Penetrate"); }
	public void EnableExplodeDuringRapidFire() { isExplodeRapidActive = true; UpdateAmmoText("Explode + Rapid"); }

	private void ChangeProjectile(GameObject newProjectilePrefab)
	{
		if (newProjectilePrefab != null && newProjectilePrefab != currentProjectilePrefab)
		{
			currentProjectilePrefab = newProjectilePrefab;
			UpdateAmmoText("Normal Bullet");
		}
	}

	private void UpdateAmmoText(string status)
	{
		if (ammoTxt != null) ammoTxt.text = $"Current Mode: {status}";
	}
}
