using UnityEngine;

public class Bubble : MonoBehaviour
{
	[SerializeField] private int scoreValue = 1;
	[SerializeField] private ParticleSystem destroyParticlePrefab;
	[SerializeField] private ParticleSystem blastParticlePrefab;
	[SerializeField] private ParticleSystem KikiParticlePrefab;
	[SerializeField] private float horizontalMoveSpeed = 1f;
	[SerializeField] private SkillManager.SkillType skillType = SkillManager.SkillType.None;
	[SerializeField] private Transform childGameObject;
	[SerializeField] private Vector3 childWorldScale = new Vector3(0.3f, 0.3f, 1f);

	private GameManager gameManager;
	private SkillManager skillManager; 
	private float upwardSpeed;
	private float noiseFrequency;
	private float noiseStrength;
	private Rigidbody2D rb;
	private float noiseTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		skillManager = FindFirstObjectByType<SkillManager>();
		gameManager = FindFirstObjectByType<GameManager>();

		rb.gravityScale = 0f;
		upwardSpeed = Random.Range(1.5f, 2.5f);
		noiseFrequency = Random.Range(0.2f, 0.5f);
		noiseStrength = Random.Range(3f, 1f);

		rb.velocity = new Vector2(Random.Range(-horizontalMoveSpeed, horizontalMoveSpeed), upwardSpeed);
	}

	private void Update()
	{
		if (childGameObject != null)
		{
			Vector3 parentScale = transform.lossyScale;
			childGameObject.localScale = new Vector3(
				childWorldScale.x / parentScale.x,
				childWorldScale.y / parentScale.y,
				childWorldScale.z / parentScale.z
			);
		}

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
		gameManager.AddScore(scoreValue);

		if (skillManager != null && skillType != SkillManager.SkillType.None)
		{
			skillManager.OnSkillPickup(skillType);
		}

		if (destroyParticlePrefab != null)
		{
			ParticleSystem particles = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
			Destroy(particles.gameObject, particles.main.duration);
		}

		if (blastParticlePrefab != null)
		{
			ParticleSystem blastParticles = Instantiate(blastParticlePrefab, transform.position, Quaternion.identity);
			blastParticles.transform.localScale = transform.localScale;
			blastParticles.Play();
			Destroy(blastParticles.gameObject, blastParticles.main.duration);
		}

		if (KikiParticlePrefab != null)
		{
			ParticleSystem kikiParticles = Instantiate(KikiParticlePrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));
			kikiParticles.transform.localScale = Vector3.one;
			Destroy(kikiParticles.gameObject, kikiParticles.main.duration);
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

	public void SetSkillManager(SkillManager manager)
	{
		skillManager = manager; 
	}
}
