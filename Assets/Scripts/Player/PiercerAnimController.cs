using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercerAnimController : MonoBehaviour
{
	public PlayerShooting playerShooting;
	private Animator pierAnim;

	private void Start()
	{
		pierAnim = GetComponent<Animator>();
	}

	public void PierATK()
	{
		pierAnim.SetTrigger("ATKT");
	}
}
