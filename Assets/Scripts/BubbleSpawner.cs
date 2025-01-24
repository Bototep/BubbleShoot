using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
	[SerializeField] private GameObject[] baublePrefabs; 
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
		if (spawnArea != null && baublePrefabs.Length > 0)
		{
			Vector2 randomPosition = new Vector2(
				Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
				spawnArea.bounds.max.y + spawnHeightOffset
			);

			GameObject baublePrefab = baublePrefabs[Random.Range(0, baublePrefabs.Length)];

			GameObject bauble = Instantiate(baublePrefab, randomPosition, Quaternion.identity);

			float randomScale = Random.Range(1f, 2f);
			bauble.transform.localScale = new Vector3(randomScale, randomScale, 1f);
		}
	}

	private void ScheduleNextSpawn()
	{
		nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
	}
}
