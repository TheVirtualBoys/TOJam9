﻿using UnityEngine;
using System.Collections;

public class ClickDudes : MonoBehaviour
{
	BoxCollider2D collider = null;
	AudioSource audioSource = null;

	// Use this for initialization
	void Start()
	{
		collider    = gameObject.GetComponent<BoxCollider2D>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (!Main.audioManager.IsOffset(audioSource)) return;
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Physics2D.OverlapPoint(mouse) == collider)
			{
				Main.audioManager.ReSyncSource(audioSource);
			}
		}
	}
}