using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public static AudioManager audioManager = null;
	AudioSource[] sources = new AudioSource[(int)AudioManager.Tracks.TRACK_MAX];
	public ScoreManager scoreManager;
	bool startedLevel = false;
	public GameObject readyText = null;
	public GameObject marchText = null;

	GameObject ready = null;
	GameObject march = null;


	void Awake()
	{
		audioManager = new AudioManager(scoreManager);
	}

	// Use this for initialization
	void Start()
	{
		CameraFade.StartAlphaFade(Color.black, true, 5.0f, 0f, () => { StartIntro(); });
		for (int i = 0; i < (int)AudioManager.Tracks.TRACK_MAX; ++i)
		{
			sources[i] = gameObject.AddComponent<AudioSource>();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		audioManager.Update();
		for (int i = 0; i < (int)AudioManager.Tracks.TRACK_MAX; ++i)
		{
			if (sources[i] == null || sources[i].clip == null) continue;
			if (sources[i].time >= sources[i].clip.length)
			{
				//Destroy(sources[i].clip);
				sources[i].Stop();
				sources[i].clip = null;
				if (startedLevel == false)
				{
					StartLevel();
					startedLevel = true;
				}
			}
		}
	}

	void StartIntro()
	{
		CharacterCollection col = GetComponent<CharacterCollection>();
		if (col != null)
		{
			foreach (Transform character in col.characters)
			{
				playAnimation(character.gameObject, true);
			}
		}

		sources[0].clip = Resources.Load<AudioClip>("Drums_In");
		sources[1].clip = Resources.Load<AudioClip>("Tuba_In");
		sources[2].clip = Resources.Load<AudioClip>("Trumpet_In");
		sources[3].clip = Resources.Load<AudioClip>("Flute_In");
		for (int i = 0; i < (int)AudioManager.Tracks.TRACK_MAX; ++i)
		{
			float volume = 0.0f;
			switch ((AudioManager.Tracks)i)
			{
				case AudioManager.Tracks.TRACK_DRUM: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_FLUTE: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_TRUMPET: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_TUBA: volume = 0.02f; break;
			}
			sources[i].volume = volume;
			sources[i].loop = false;
			sources[i].Play();
		}
	}

	void StartLevel()
	{
		CharacterCollection col = GetComponent<CharacterCollection>();
		if (col != null)
		{
			foreach (Transform character in col.characters)
			{
				AudioScript audio = character.gameObject.GetComponent<AudioScript>();
				if (audio != null) audio.Play();
			}
		}

		GameObject field = GameObject.Find("Field");
		if (field != null)
		{
			Animator ani = field.GetComponent<Animator>();
			ani.enabled = true;
		}
		audioManager.EnableOffsetting();
		startedLevel = true;
	}

	private void playAnimation(GameObject character, bool running)
	{
		if (character != null)
		{
			AniStrip ani = character.GetComponent<AniStrip>();
			ani.running = running;
		}
	}

	public void StartOutroMusic()
	{
		sources[0].clip = Resources.Load<AudioClip>("Drums_Out");
		sources[1].clip = Resources.Load<AudioClip>("Tuba_Out");
		sources[2].clip = Resources.Load<AudioClip>("Trumpet_Out");
		sources[3].clip = Resources.Load<AudioClip>("Flute_Out");
		for (int i = 0; i < (int)AudioManager.Tracks.TRACK_MAX; ++i)
		{
			float volume = 0.0f;
			switch ((AudioManager.Tracks)i)
			{
				case AudioManager.Tracks.TRACK_DRUM: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_FLUTE: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_TRUMPET: volume = 0.02f; break;
				case AudioManager.Tracks.TRACK_TUBA: volume = 0.02f; break;
			}
			sources[i].volume = volume;
			sources[i].loop = false;
			sources[i].Play();
		}
	}


}
