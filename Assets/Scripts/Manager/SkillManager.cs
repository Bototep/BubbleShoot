using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
	public enum SkillType { None, Penetrate, Rapid, Explode }
	public static SkillManager Instance { get; private set; }
	private SkillType[] skillSlots = new SkillType[3];
	public GameObject[] skillButtons;
	public PlayerShooting playerShooting;

	public Image Penetrate1;
	public Image Rapid1;
	public Image Explode1;
	public Image Penetrate2;
	public Image Rapid2;
	public Image Explode2;
	public Image Penetrate3;
	public Image Rapid3;
	public Image Explode3;

	private bool isPenetrateActive = false;
	private bool isRapidActive = false;
	private bool isExplodeActive = false;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			UseSkill(0); 
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			UseSkill(1); 
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			UseSkill(2); 
		}
	}

	public void OnSkillPickup(SkillType newSkill)
	{
		for (int i = 0; i < skillSlots.Length; i++)
		{
			if (skillSlots[i] == SkillType.None)
			{
				skillSlots[i] = newSkill;
				Debug.Log($"Picked up {newSkill} and added to slot {i + 1}");
				UpdateSkillButton(i);
				return;
			}
		}
		Debug.Log("No available slots for new skill!");
	}

	public void UseSkill(int slotIndex)
	{
		if (skillSlots[slotIndex] != SkillType.None)
		{
			Debug.Log($"Used skill: {skillSlots[slotIndex]} from slot {slotIndex + 1}");
			ExecuteSkill(skillSlots[slotIndex]);

			skillSlots[slotIndex] = SkillType.None;
			UpdateSkillButton(slotIndex);
		}
		else
		{
			Debug.Log($"Slot {slotIndex + 1} is empty.");
		}
	}

	private void ExecuteSkill(SkillType skill)
	{
		switch (skill)
		{
			case SkillType.Penetrate:
				isPenetrateActive = true;
				playerShooting.ChangeToPenetrate();
				Debug.Log("Penetrate skill activated!");
				break;
			case SkillType.Rapid:
				isRapidActive = true;
				playerShooting.RapidFire();
				Debug.Log("Rapid skill activated!");
				break;
			case SkillType.Explode:
				isExplodeActive = true;
				playerShooting.ChangeToExplode();
				Debug.Log("Explode skill activated!");
				break;
			default:
				Debug.Log("No skill to execute.");
				break;
		}

		// Combine Penetrate + Rapid
		if (isPenetrateActive && isRapidActive)
		{
			playerShooting.EnableInfinitePenetrateDuringRapidFire();
		}

		// Combine Explode + Rapid
		if (isExplodeActive && isRapidActive)
		{
			playerShooting.EnableExplodeDuringRapidFire();
		}

		if (isPenetrateActive && isExplodeActive)
		{
			playerShooting.EnableExplodeDuringPenetrate();
		}
	}

	private void UpdateSkillButton(int slotIndex)
	{
		switch (slotIndex)
		{
			case 0:
				UpdateSkillImage(Penetrate1, Rapid1, Explode1, skillSlots[slotIndex]);
				break;
			case 1:
				UpdateSkillImage(Penetrate2, Rapid2, Explode2, skillSlots[slotIndex]);
				break;
			case 2:
				UpdateSkillImage(Penetrate3, Rapid3, Explode3, skillSlots[slotIndex]);
				break;
		}
	}

	private void UpdateSkillImage(Image penetrateImg, Image rapidImg, Image explodeImg, SkillType skill)
	{
		penetrateImg.gameObject.SetActive(false);
		rapidImg.gameObject.SetActive(false);
		explodeImg.gameObject.SetActive(false);

		switch (skill)
		{
			case SkillType.Penetrate:
				penetrateImg.gameObject.SetActive(true);
				break;
			case SkillType.Rapid:
				rapidImg.gameObject.SetActive(true);
				break;
			case SkillType.Explode:
				explodeImg.gameObject.SetActive(true);
				break;
			case SkillType.None:
				break;
		}
	}
}
