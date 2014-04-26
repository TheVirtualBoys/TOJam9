using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour
{
	static AudioManager manager = null;
	AudioSource[] source        = new AudioSource[(int)AudioSources.SOURCE_MAX];
	int activeSource            = (int)AudioSources.SOURCE_MAX;
	public AudioManager.Tracks track = AudioManager.Tracks.TRACK_MAX;

	enum AudioSources {
		SOURCE_ONE,
		SOURCE_TWO,
		SOURCE_MAX
	};

	void Awake()
	{
		if (manager == null) manager = Main.audioManager;
	}

	void Start()
	{
		for (int i = 0; i < (int)AudioSources.SOURCE_MAX; ++i)
		{
			source[i] = gameObject.AddComponent<AudioSource>();
		}
		SetActiveSource(AudioSources.SOURCE_ONE);
		SetInstrument(track);
		source[activeSource].Play();
	}

	public void SetInstrument(AudioManager.Tracks track)
	{
		manager.CreateAudioSource(source[activeSource], track);
	}

	void SetActiveSource(AudioSources src)
	{
		if ((int)AudioSources.SOURCE_MAX != activeSource) source[activeSource].Stop();
		activeSource = (int)src;
		source[activeSource].Play();
	}
}
