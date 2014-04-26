using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager
{
	//Dictionary<Tracks, List<AudioSource> > sources = 
	List<AudioSource> sources   = new List<AudioSource>();
	AudioClip[] tracks          = new AudioClip[(int)Tracks.TRACK_MAX];
	int[] trackPosition         = new int[(int)Tracks.TRACK_MAX];
	int[] trackOffsetMax        = new int[(int)Tracks.TRACK_MAX];
	float[] trackPitchOffsetMax = new float[(int)Tracks.TRACK_MAX];
	float t                     = 0.0f;
	int numOffsetDudes          = 0;

	public enum Tracks
	{
		TRACK_DRUM,
		TRACK_TUBA,
		TRACK_TRUMPET,
		TRACK_FLUTE,
		TRACK_MAX
	}

	public AudioManager()
	{
		tracks[0] = Resources.Load<AudioClip>("Drums_Loop");
		tracks[1] = Resources.Load<AudioClip>("Tuba_Loop");
		tracks[2] = Resources.Load<AudioClip>("Trumpet_Loop");
		tracks[3] = Resources.Load<AudioClip>("Flute_Loop");
		for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
		{
			trackPosition[i]       = 0;
			trackOffsetMax[i]      = 44100; // 44.1kHz == 1s
			trackPitchOffsetMax[i] = 0.02f;
		}
	}

	public void CreateAudioSource(AudioSource source, Tracks track)
	{
		source.clip        = tracks[(int)track];
		source.timeSamples = trackPosition[(int)track];
		sources.Add(source);
	}

	public int NumActiveSources(Tracks track)
	{
		int num = 0;
		for (int i = 0; i < sources.Count; ++i)
		{
			if (sources[i].clip == tracks[(int)track]) ++num;
		}
		return num;
	}

	public void Update()
	{
		// put logic for getting people out of sync.

	}
}
