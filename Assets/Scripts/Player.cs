using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
	public float rotationOffset = -90f;

	void Update() 
	{
		RotateToMouse();
	}

	private void RotateToMouse() 
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector3 direction = mousePosition - transform.position;
		direction.z = 0;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
	}
}
