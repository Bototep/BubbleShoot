using UnityEngine;

public class Bubble : MonoBehaviour
{
	[SerializeField] private int scoreValue = 1;
	[SerializeField] private ParticleSystem destroyParticlePrefab;

	[Header("Movement Settings")]
	[SerializeField] private float horizontalMoveSpeed = 1f;

	private float upwardSpeed;
	private float noiseFrequency;
	private float noiseStrength;

	private Rigidbody2D rb;
	private float noiseTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		upwardSpeed = Random.Range(0.7f, 2f);
		noiseFrequency = Random.Range(0.2f, 0.5f);
		noiseStrength = Random.Range(3f, 1f);

		rb.velocity = new Vector2(Random.Range(-horizontalMoveSpeed, horizontalMoveSpeed), upwardSpeed);
	}

	private void Update()
	{
		noiseTime += Time.deltaTime * noiseFrequency;
		float noiseX = Mathf.PerlinNoise(noiseTime, 0f) * 2f - 1f;
		float horizontalMovement = noiseX * noiseStrength;

		rb.velocity = new Vector2(Mathf.Clamp(horizontalMovement, -horizontalMoveSpeed, horizontalMoveSpeed), rb.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			GameManager.Instance.AddScore(scoreValue);

			if (destroyParticlePrefab != null)
			{
				ParticleSystem particles = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
				Destroy(particles.gameObject, particles.main.duration);
			}

			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.CompareTag("End"))
		{
			Destroy(gameObject);
		}
	}
}
