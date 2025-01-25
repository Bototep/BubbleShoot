using System.Collections;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
	public PlayerShooting playerShooting;
	private Animator charAnim;

	[SerializeField] private float minAFKInterval = 5f; // Minimum time before going AFK
	[SerializeField] private float maxAFKInterval = 15f; // Maximum time before going AFK
	[SerializeField] private float afkDuration = 2f; // Fixed time to stay in AFK state

	private void Start()
	{
		charAnim = GetComponent<Animator>();
		StartCoroutine(RandomAFKCoroutine());
	}

	public void Idle()
	{
		charAnim.SetBool("AFK", false);
		charAnim.SetBool("ATK", false);
	}

	public void AFK()
	{
		charAnim.SetBool("AFK", true);
		charAnim.SetBool("ATK", false);
	}

	public void ATK()
	{
		charAnim.SetBool("ATK", true);
		charAnim.SetBool("AFK", false);
	}

	private IEnumerator RandomAFKCoroutine()
	{
		while (true)
		{
			float interval = Random.Range(minAFKInterval, maxAFKInterval);
			yield return new WaitForSeconds(interval);
			AFK();
			yield return new WaitForSeconds(afkDuration);
			Idle();
		}
	}
}
