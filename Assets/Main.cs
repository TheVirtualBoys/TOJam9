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

		CameraFade.StartAlphaFade(Color.black, true, 5.0f, 0f, () => { StartLevel(); });
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

	void StartLevel()
	{
		CharacterCollection col = GetComponent<CharacterCollection>();
		if (col != null)
		{
			foreach (Transform character in col.characters)
			{
				playAnimation(character.gameObject, true);
			}
		}

		GameObject field = GameObject.Find("Field");
		if (field != null)
		{
			Animator ani = field.GetComponent<Animator>();
			ani.enabled = true;
		}
	}

	private void playAnimation(GameObject character, bool running)
	{
		if (character != null)
		{
			AniStrip ani = character.GetComponent<AniStrip>();
			ani.running = running;
		}
	}


}
