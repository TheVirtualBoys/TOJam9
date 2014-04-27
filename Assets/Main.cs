using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public static AudioManager audioManager = null;
	
	public ScoreManager scoreManager;

	void Awake()
	{
		audioManager = new AudioManager(scoreManager);
	}

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		audioManager.Update();
	}
}
