using UnityEngine;
using System.Collections;

public class ClickAnimator : MonoBehaviour {

	public Transform animationPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D collider = Physics2D.OverlapPoint(mouse);
			if (collider == null)
			{
				//show the bad click animation since we didn't hit anything
				mouse.z = -5;
				Instantiate(animationPrefab, mouse, Quaternion.identity);
			}
			//else the individual ClickDudes script will handle things
		}
	}
}
