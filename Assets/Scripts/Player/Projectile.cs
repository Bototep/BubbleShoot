using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float lifeTime = 3f;
	private Vector2 velocity;

	void Start() => Destroy(gameObject, lifeTime);

	void Update()
	{
		transform.Translate(velocity * Time.deltaTime, Space.World);

		if (velocity != Vector2.zero)
		{
			float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; 
			transform.rotation = Quaternion.Euler(0, 0, angle); 
		}
	}

	public void Initialize(Vector2 direction, float speed)
	{
		velocity = direction.normalized * speed; 
	}
}
