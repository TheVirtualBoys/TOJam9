using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager
{
	//Dictionary<Tracks, List<AudioSource> > sources = 
	List<AudioSource> sources         = new List<AudioSource>();
	AudioClip[] tracks                = new AudioClip[(int)Tracks.TRACK_MAX];
	float[] trackPitchOffsetMax       = new float[(int)Tracks.TRACK_MAX];
	float[] trackOffsetMax            = new float[(int)Tracks.TRACK_MAX];
	int[] numOffsetDudes              = new int[(int)Tracks.TRACK_MAX];
	List<AudioSource> offsetSources   = new List<AudioSource>();
	float t                           = 0.0f;
	float lastOffsetTime              = 0.0f;
	const int maxOffsetDudes          = 2;
	const float maxTimeBetweenOffsets = 20.0f;
	const float minTimeBetweenOffsets = 2.0f;
	const float percentageChance      = 40.0f;
	const float pitchOffset           = 0.01f;
	bool enableOffsetting             = false;

	ScoreManager scoreManager;

	public enum Tracks
	{
		TRACK_DRUM,
		TRACK_TUBA,
		TRACK_TRUMPET,
		TRACK_FLUTE,
		TRACK_MAX,
		TRACK_NONE
	}

	public AudioManager(ScoreManager scoreManager)
	{
		this.scoreManager = scoreManager;

		//tracks[(int)Tracks.TRACK_NONE] = null;
		tracks[(int)Tracks.TRACK_DRUM] = Resources.Load<AudioClip>("Drums_Loop");
		tracks[(int)Tracks.TRACK_TUBA] = Resources.Load<AudioClip>("Tuba_Loop");
		tracks[(int)Tracks.TRACK_TRUMPET] = Resources.Load<AudioClip>("Trumpet_Loop");
		tracks[(int)Tracks.TRACK_FLUTE] = Resources.Load<AudioClip>("Flute_Loop");
		for (int i = 0; i < (int)Tracks.TRACK_MAX; ++i)
		{
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

	public bool IsOffset(AudioSource source)
	{
		return offsetSources.Contains(source);
	}

	AudioSource GetInSyncAudioSource(Tracks track)
	{
		foreach (AudioSource source in sources)
		{
			if (source.clip == tracks[(int)track] && !offsetSources.Contains(source)) return source;
		}
		return null;
	}

	public void CreateAudioSource(AudioSource source, Tracks track)
	{
		if ((int)track < (int)Tracks.TRACK_MAX)
		{
			source.clip = tracks[(int)track];
			source.loop = true;
			sources.Add(source);
		}
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

	public void EnableOffsetting()
	{
		enableOffsetting = true;
	}

	public void Update()
	{
		float dt = Time.deltaTime;
		t += dt;
		int offsetDudes = TotalOffsetDudes();
			/*if (Mathf.FloorToInt(t) > 1)
			{
				string str = "";
				for (int i = 0; i < numOffsetDudes.Length; ++i)
					str += numOffsetDudes[i] + " ";
				Debug.Log(str);
			}*/
		if (enableOffsetting && offsetDudes < maxOffsetDudes && t >= lastOffsetTime + minTimeBetweenOffsets)
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
					track = offsetableTracks[Random.Range(0, offsetableTracks.Count)];
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
						sourceIndex = Random.Range(0, trackDudes.Count);
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
						source.gameObject.GetComponent<AniStrip>().speedMultiplier += offset;
					}
					else
					{
						screwWithThisOne.pitch -= offset;
						source.gameObject.GetComponent<AniStrip>().speedMultiplier -= offset;
					}
					Debug.Log("Offset " + source + " @" + offset);
					offsetSources.Add(source);
					++numOffsetDudes[track];
					lastOffsetTime = t;
					scoreManager.onGotDesync(source.gameObject.GetInstanceID());
				}
			}
		}

		// 4. keep track until it's too offset then reset the pitch
		for (int i = 0; i < offsetSources.Count; ++i)
		{
			AudioSource source = offsetSources[i];
			Tracks track = GetTrack(source);
			if (track == Tracks.TRACK_MAX) continue;

			if (source.pitch == 1.0f)
			{
				Vector3 pos = source.gameObject.transform.position;
				Debug.DrawLine(pos, pos + new Vector3(10, 0, 0), Color.red);
			}

			float trackLength = (source.clip != null)? source.clip.length : 0;
			AudioSource inSyncAudioSource = GetInSyncAudioSource(track);
			float diff = 0;
			if ((diff = inSyncAudioSource.time + trackOffsetMax[(int)track] - trackLength) > 0.0f)
			{
				if (source.time <= inSyncAudioSource.time - trackOffsetMax[(int)track] &&
					source.time >= diff)
				{
					// reset it
					if (source.pitch != 1.0f) Debug.Log("done offsetting " + offsetSources[i] + " (wrap high)");
					source.pitch = 1.0f;
					source.gameObject.GetComponent<AniStrip>().speedMultiplier = 1.0f;
				}
			}
			else if ((diff = inSyncAudioSource.time - trackOffsetMax[(int)track]) < 0.0f)
			{
				diff += trackLength;
				if (source.time <= diff &&
					source.time >= inSyncAudioSource.time + trackOffsetMax[(int)track])
				{
					// reset it
					if (source.pitch != 1.0f) Debug.Log("done offsetting " + offsetSources[i] + " (wrap low)");
					source.pitch = 1.0f;
					source.gameObject.GetComponent<AniStrip>().speedMultiplier = 1.0f;
				}
			}
			else
			{
				if (source.time >= (inSyncAudioSource.time + trackOffsetMax[(int)track]) ||
					source.time <= (inSyncAudioSource.time - trackOffsetMax[(int)track]))
				{
					if (source.pitch != 1.0f) Debug.Log("done offsetting " + offsetSources[i]);
					source.pitch = 1.0f;
					source.gameObject.GetComponent<AniStrip>().speedMultiplier = 1.0f;
				}
			}
		}
	}

	public void ReSyncSource(AudioSource source)
	{
		Debug.Log("Resync " + source);
		Tracks track = GetTrack(source);
		GameObject sourceGO = source.gameObject;
		source.pitch = 1.0f;

		AudioSource inSyncAudioSource = GetInSyncAudioSource(track);
		Debug.Log("With " + inSyncAudioSource);

		AniStrip inSyncAni = inSyncAudioSource.gameObject.GetComponent<AniStrip>();
		AniStrip outSyncAni = sourceGO.GetComponent<AniStrip>();
		outSyncAni.speedMultiplier = 1.0f;
		outSyncAni.CurFrame = inSyncAni.CurFrame;
		outSyncAni.FrameOffsetTime = inSyncAni.FrameOffsetTime;

		source.time = inSyncAudioSource.time;
		--numOffsetDudes[(int)track];
		offsetSources.Remove(source);
		lastOffsetTime = t;

		scoreManager.onGotResync(sourceGO.GetInstanceID());
	}
}
