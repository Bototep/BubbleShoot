using UnityEngine;

public class SetSortingLayer : MonoBehaviour
{
	public string sortingLayerName = "Ground"; 
	public int sortingOrder = 0;               

	private void Start()
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		if (meshRenderer != null)
		{	
			meshRenderer.sortingLayerName = sortingLayerName;

			meshRenderer.sortingOrder = sortingOrder;
		}
		else
		{
			Debug.LogWarning("MeshRenderer not found on this GameObject.");
		}
	}
}
