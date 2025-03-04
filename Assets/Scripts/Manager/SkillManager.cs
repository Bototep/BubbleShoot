using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
	public enum SkillType { None, Penetrate, Rapid, Explode }
	public static SkillManager Instance { get; private set; }
	private SkillType[] skillSlots = new SkillType[3];
	public GameObject[] skillButtons;
	public PlayerShooting playerShooting;
	public Image Penetrate1, Rapid1, Explode1, Penetrate2, Rapid2, Explode2, Penetrate3, Rapid3, Explode3;
	public bool isPenetrateActive, isRapidActive, isExplodeActive;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q)) UseSkill(0);
		else if (Input.GetKeyDown(KeyCode.W)) UseSkill(1);
		else if (Input.GetKeyDown(KeyCode.E)) UseSkill(2);
	}

	public void OnSkillPickup(SkillType newSkill)
	{
		for (int i = 0; i < skillSlots.Length; i++)
		{
			if (skillSlots[i] == SkillType.None)
			{
				skillSlots[i] = newSkill;
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
			ExecuteSkill(skillSlots[slotIndex]);
			skillSlots[slotIndex] = SkillType.None;
			UpdateSkillButton(slotIndex);
		}
		else Debug.Log($"Slot {slotIndex + 1} is empty.");
	}

	private void ExecuteSkill(SkillType skill)
	{
		switch (skill)
		{
			case SkillType.Penetrate: isPenetrateActive = true; playerShooting.ChangeToPenetrate(); break;
			case SkillType.Rapid: isRapidActive = true; playerShooting.RapidFire(); break;
			case SkillType.Explode: isExplodeActive = true; playerShooting.ChangeToExplode(); break;
		}

		if (isPenetrateActive && isRapidActive) playerShooting.EnableInfinitePenetrateDuringRapidFire();
		if (isExplodeActive && isRapidActive) playerShooting.EnableExplodeDuringRapidFire();
		if (isPenetrateActive && isExplodeActive) playerShooting.ChangeToPenetrateExplode();
	}

	private void UpdateSkillButton(int slotIndex)
	{
		switch (slotIndex)
		{
			case 0: UpdateSkillImage(Penetrate1, Rapid1, Explode1, skillSlots[slotIndex]); break;
			case 1: UpdateSkillImage(Penetrate2, Rapid2, Explode2, skillSlots[slotIndex]); break;
			case 2: UpdateSkillImage(Penetrate3, Rapid3, Explode3, skillSlots[slotIndex]); break;
		}
	}

	private void UpdateSkillImage(Image penetrateImg, Image rapidImg, Image explodeImg, SkillType skill)
	{
		penetrateImg.gameObject.SetActive(skill == SkillType.Penetrate);
		rapidImg.gameObject.SetActive(skill == SkillType.Rapid);
		explodeImg.gameObject.SetActive(skill == SkillType.Explode);
	}

	public void ResetSkills()
	{
		for (int i = 0; i < skillSlots.Length; i++)
		{
			skillSlots[i] = SkillType.None;
			UpdateSkillButton(i);
		}

		isPenetrateActive = false;
		isRapidActive = false;
		isExplodeActive = false;

		Penetrate1.gameObject.SetActive(false);
		Rapid1.gameObject.SetActive(false);
		Explode1.gameObject.SetActive(false);
		Penetrate2.gameObject.SetActive(false);
		Rapid2.gameObject.SetActive(false);
		Explode2.gameObject.SetActive(false);
		Penetrate3.gameObject.SetActive(false);
		Rapid3.gameObject.SetActive(false);
		Explode3.gameObject.SetActive(false);
	}
}