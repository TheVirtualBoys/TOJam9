using UnityEngine;
using System.Collections;

public class WinLoseSpriteSwapper : MonoBehaviour {

	public Transform winPrefab;
	public Transform losePrefab;

	public void OnLose()
	{
		SetSpriteAni(losePrefab);
	}

	public void OnWin()
	{
		SetSpriteAni(winPrefab);
	}

	private void SetSpriteAni(Transform prefab)
	{
		renderer.enabled = false;
		Instantiate(prefab, transform.position, transform.rotation);
	}
}
