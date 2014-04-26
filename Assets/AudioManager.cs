using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager
{
	//Dictionary<Tracks, List<AudioSource> > sources = 
	List<AudioSource> sources       = new List<AudioSource>();
	AudioClip[] tracks              = new AudioClip[(int)Tracks.TRACK_MAX];
	float[] trackPitchOffsetMax     = new float[(int)Tracks.TRACK_MAX];
	float[] trackOffsetMax          = new float[(int)Tracks.TRACK_MAX];
	float[] trackPosition           = new float[(int)Tracks.TRACK_MAX];
	int[] numOffsetDudes            = new int[(int)Tracks.TRACK_MAX];
	List<AudioSource> offsetSources = new List<AudioSource>();
	float t                         = 0.0f;
	float lastOffsetTime            = 0.0f;
	const int maxOffsetDudes          = 2;
	const float maxTimeBetweenOffsets = 20.0f;
	const float minTimeBetweenOffsets = 7.0f;
	const float percentageChance      = 40.0f;
	const float pitchOffset           = 0.01f;

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
			trackPosition[i]       = 0.0f;
			trackOffsetMax[i]      = 1.0f; // 2 seconds
			trackPitchOffsetMax[i] = 0.10f;
		}
	}

	int TotalOffsetDudes()
	{
		int num = 0;
		for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
		{
			num += numOffsetDudes[i];
		}
		return num;
	}

	Tracks GetTrack(AudioSource source)
	{
		for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
		{
			if (source.clip == tracks[i]) return (Tracks)i;
		}
		return Tracks.TRACK_MAX;
	}

	public void CreateAudioSource(AudioSource source, Tracks track)
	{
		source.clip = tracks[(int)track];
		source.time = trackPosition[(int)track];
		source.loop = true;
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
		float dt = Time.deltaTime;
		t += dt;
		for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
		{
			trackPosition[i] += dt;
		}
		int offsetDudes = TotalOffsetDudes();

		if (offsetDudes < maxOffsetDudes && t >= lastOffsetTime + minTimeBetweenOffsets)
		{
			// we can offset a dude!
			if (t >= lastOffsetTime + maxTimeBetweenOffsets || Random.Range(0, 100) < percentageChance)
			{
				// desync a dude!!
				// 1. find which instrument to desync
				List<int> offsetableTracks = new List<int>();
				for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
				{
					if (numOffsetDudes[i] == 0)
					{
						offsetableTracks.Add(i);
					}
				}
				int track = -1;
				if (offsetableTracks.Count == 0)
				{
					track = Random.Range(0, (int)Tracks.TRACK_MAX);
				}
				else
				{
					track = Random.Range(0, offsetableTracks.Count);
				}
				
				// 2. pick a random dude playing that instrument to desync
				List<AudioSource> trackDudes = new List<AudioSource>();
				for (int i = 0; i < sources.Count; ++i)
				{
					if (sources[i].clip == tracks[track] && !offsetSources.Contains(sources[i]))
					{
						trackDudes.Add(sources[i]);
					}
				}

				if (trackDudes.Count > 0)
				{
					int sourceIndex = -1;
					AudioSource source = null;
					while (sourceIndex == -1)
					{
						sourceIndex = Random.Range(0, trackDudes.Count - 1);
						source = trackDudes[sourceIndex];
						if (offsetSources.Contains(source))
						{
							sourceIndex = -1;
						}
					}

					AudioSource screwWithThisOne = trackDudes[sourceIndex];
					// 3. mess with its pitch
					float offset = Random.Range(pitchOffset, trackPitchOffsetMax[(int)track]);
					if ((screwWithThisOne.timeSamples & 1) == 1)
					{
						screwWithThisOne.pitch += offset;
					}
					else
					{
						screwWithThisOne.pitch -= offset;
					}
					Debug.Log("Offset " + source + " @" + offset);
					offsetSources.Add(source);
					++numOffsetDudes[track];
					lastOffsetTime = t;
				}
			}
		}

		// 4. keep track until it's too offset then reset the pitch
		for (int i = 0; i < offsetSources.Count; ++i)
		{
			AudioSource source = offsetSources[i];
			Tracks track = GetTrack(source);
			if (track == Tracks.TRACK_MAX) continue;

			if (source.time >= (trackPosition[(int)track] + trackOffsetMax[(int)track]) || source.time <= (trackPosition[(int)track] - trackOffsetMax[(int)track]))
			{
				if (source.pitch != 1.0f) Debug.Log("done offsetting " + offsetSources[i]);
				source.pitch = 1.0f;
				// do something with the animation
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			foreach (AudioSource source in offsetSources)
			{
				ReSyncSource(source);
			}
		}
	}

	public void ReSyncSource(AudioSource source)
	{
		source.pitch = 1.0f;
		Tracks track = GetTrack(source);
		source.time  = trackPosition[(int)track];
		--numOffsetDudes[(int)track];
		offsetSources.Remove(source);
	}
}
