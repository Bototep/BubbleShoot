using UnityEngine;

[System.Serializable]
public class BaublePrefabChance
{
	public GameObject baublePrefab;

	[Range(0f, 100f)]
	public float spawnChance;
}

public class BubbleSpawner : MonoBehaviour
{
	[SerializeField] private BaublePrefabChance[] baublePrefabsWithChances;
	[SerializeField] private float minSpawnInterval = 1f;
	[SerializeField] private float maxSpawnInterval = 3f;
	[SerializeField] private float spawnHeightOffset = 2f;

	private Collider2D spawnArea;
	private float nextSpawnTime;

	private void Start()
	{
		spawnArea = GetComponent<Collider2D>();
		ScheduleNextSpawn();
	}

	private void Update()
	{
		if (Time.time >= nextSpawnTime)
		{
			SpawnBauble();
			ScheduleNextSpawn();
		}
	}

	private void SpawnBauble()
	{
		if (spawnArea != null && baublePrefabsWithChances.Length > 0)
		{
			float totalChance = 0f;
			foreach (var prefabChance in baublePrefabsWithChances)
			{
				totalChance += prefabChance.spawnChance;
			}

			if (totalChance > 100f)
			{
				Debug.LogWarning("Total spawn chance exceeds 100%. Resetting to 100.");
				totalChance = 100f;
			}

			float randomValue = Random.Range(0f, totalChance);

			GameObject selectedPrefab = null;
			float cumulativeChance = 0f;
			foreach (var prefabChance in baublePrefabsWithChances)
			{
				cumulativeChance += prefabChance.spawnChance;
				if (randomValue <= cumulativeChance)
				{
					selectedPrefab = prefabChance.baublePrefab;
					break;
				}
			}

			if (selectedPrefab != null)
			{
				Vector2 randomPosition = new Vector2(
					Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
					spawnArea.bounds.max.y + spawnHeightOffset
				);

				Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

				GameObject bauble = Instantiate(selectedPrefab, randomPosition, randomRotation);

				float randomScale = Random.Range(0.04f, 0.1f);
				bauble.transform.localScale = new Vector3(randomScale, randomScale, 1f);
			}
		}
	}

	private void ScheduleNextSpawn()
	{
		nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
	}
}
