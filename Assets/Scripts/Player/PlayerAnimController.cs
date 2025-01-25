using System.Collections;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
	public PlayerShooting playerShooting;
	private Animator charAnim;

	[SerializeField] private float minAFKInterval = 5f; 
	[SerializeField] private float maxAFKInterval = 15f; 
	[SerializeField] private float afkDuration = 2f;
	[SerializeField] private float atkAnimationDuration = 0.7f;

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

		StartCoroutine(ResetToIdle());
	}

	private IEnumerator ResetToIdle()
	{
		yield return new WaitForSeconds(atkAnimationDuration);
		Idle();
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
