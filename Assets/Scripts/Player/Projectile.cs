using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float lifeTime = 3f; 
	private Vector2 velocity;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void Update()
	{
		transform.Translate(velocity * Time.deltaTime, Space.World);
	}

	public void Initialize(Vector2 direction, float speed)
	{
		velocity = direction.normalized * speed;
	}
}