using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour
{
	AudioSource[] source = new AudioSource[(int)AudioSources.SOURCE_MAX];
	int activeSource = (int)AudioSources.SOURCE_MAX;
	static int numCreated = 0;
	public int mynum = 0;
	public AudioClip track = null;

	enum AudioSources {
		SOURCE_ONE,
		SOURCE_TWO,
		SOURCE_MAX
	};

	void Awake()
	{
		mynum = numCreated++;
	}

	void Start()
	{
		for (int i = 0; i < (int)AudioSources.SOURCE_MAX; ++i)
		{
			source[i] = gameObject.AddComponent<AudioSource>();
			source[i].clip = track;
		}
		SetActiveSource(AudioSources.SOURCE_ONE);
		GameObject.Find("Conductor").GetComponent<AudioManager>().RegisterPlayer(this);
	}

	void SetClip(AudioSources src, AudioClip clip, float seconds)
	{
		if (src == AudioSources.SOURCE_MAX) return;
		source[activeSource].clip = clip;
		source[activeSource].time = seconds;
	}

	void SetActiveSource(AudioSources src)
	{
		if ((int)AudioSources.SOURCE_MAX != activeSource) source[activeSource].Stop();
		activeSource = (int)src;
		source[activeSource].Play();
	}

	void Update()
	{
		if (mynum == 0) return;
		if (Input.GetKeyDown(KeyCode.Return))
		{
			// increase pitch by 1
			source[activeSource].pitch += 0.02f;
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			source[activeSource].pitch = 1.0f;
		}
	}
}
