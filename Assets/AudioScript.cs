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
		float volume = 1.0f;
		switch (track)
		{
			case AudioManager.Tracks.TRACK_DRUM:    volume = 0.3f; break;
			case AudioManager.Tracks.TRACK_FLUTE:   volume = 0.3f; break;
			case AudioManager.Tracks.TRACK_TRUMPET: volume = 0.7f; break;
			case AudioManager.Tracks.TRACK_TUBA:    volume = 1.0f; break;
		}
		source[activeSource].volume = volume;
	}

	void SetActiveSource(AudioSources src)
	{
		if ((int)AudioSources.SOURCE_MAX != activeSource) source[activeSource].Stop();
		activeSource = (int)src;
		source[activeSource].Play();
	}

	public static void Reset()
	{
		manager = null;
	}
}
