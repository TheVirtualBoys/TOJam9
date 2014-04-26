using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	List<AudioScript> players = new List<AudioScript>();

	public AudioClip[] tracks = new AudioClip[(int)Tracks.TRACK_MAX];

	public enum Tracks
	{
		TRACK_DRUM,
		TRACK_TRUMPET,
		TRACK_TUBA,
		TRACK_FLUTE,
		TRACK_MAX
	}

	void Start()
	{
		
	}

	public AudioSource CreateAudioSource(Tracks track)
	{
		return null;
	}
	
	public void RegisterPlayer(AudioScript player)
	{
		if (players.Contains(player)) return;
		players.Add(player);
	}

	// Update is called once per frame
	void Update()
	{
	
	}
}
