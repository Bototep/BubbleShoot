using UnityEngine;

public class Bubble : MonoBehaviour
{
	[SerializeField] private int scoreValue = 1;
	[SerializeField] private ParticleSystem destroyParticlePrefab;
	[SerializeField] private float horizontalMoveSpeed = 1f;
	[SerializeField] private SkillManager.SkillType skillType = SkillManager.SkillType.None; // Set the skill type in Inspector

	private float upwardSpeed;
	private float noiseFrequency;
	private float noiseStrength;
	private Rigidbody2D rb;
	private float noiseTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		rb.gravityScale = 0f;

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

		rb.velocity = new Vector2(Mathf.Clamp(horizontalMovement, -horizontalMoveSpeed, horizontalMoveSpeed), Mathf.Abs(rb.velocity.y));

		if (rb.velocity.y <= 0) 
		{
			rb.velocity = new Vector2(rb.velocity.x, upwardSpeed); 
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			HandleBubbleHit();
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.CompareTag("Penetrate"))
		{
			HandleBubbleHit();
		}
		else if (collision.gameObject.CompareTag("Explode"))
		{
			HandleExplosion(collision.gameObject.transform.position);
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.CompareTag("PeneExplo"))
		{
			HandleExplosion(collision.gameObject.transform.position);
		}
		else if (collision.gameObject.CompareTag("End"))
		{
			Destroy(gameObject);
		}
	}

	private void HandleBubbleHit()
	{
		GameManager.Instance.AddScore(scoreValue);

		if (skillType != SkillManager.SkillType.None)
		{
			SkillManager.Instance.OnSkillPickup(skillType);
		}

		if (destroyParticlePrefab != null)
		{
			ParticleSystem particles = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
			Destroy(particles.gameObject, particles.main.duration);
		}

		Destroy(gameObject);
	}

	private void HandleExplosion(Vector2 explosionPosition)
	{
		float explosionRadius = 2f; 

		Collider2D[] hitBubbles = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

		foreach (Collider2D bubbleCollider in hitBubbles)
		{
			Bubble bubble = bubbleCollider.GetComponent<Bubble>();
			if (bubble != null)
			{
				bubble.HandleBubbleHit();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 2f); 
	}
}
