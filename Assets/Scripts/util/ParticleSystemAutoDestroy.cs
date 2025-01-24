using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
	private void Start()
	{
		ParticleSystem ps = GetComponent<ParticleSystem>();

		if (ps != null)
		{
			Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
		}
	}
}
