using UnityEngine;
using System.Collections;

public class ClickDudes : MonoBehaviour
{
	BoxCollider2D boxCollider = null;
	AudioSource audioSource = null;

	public Transform goodClickPrefab;
	public Transform badClickPrefab;

	// Use this for initialization
	void Start()
	{
		boxCollider    = gameObject.GetComponent<BoxCollider2D>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Physics2D.OverlapPoint(mouse) == boxCollider)
			{
				if (Main.audioManager.IsOffset(audioSource))
				{
					mouse.z = -5;
					Instantiate(goodClickPrefab, mouse, Quaternion.identity);

					Main.audioManager.ReSyncSource(audioSource);
				}
				else
				{
					mouse.z = -5;
					Instantiate(badClickPrefab, mouse, Quaternion.identity);
				}
			}
		}
	}
}
