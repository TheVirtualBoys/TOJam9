using UnityEngine;
using System.Collections;

public class NoteProgressBar : MonoBehaviour {

	public float percentComplete;

	public Animator aniToTrack;

	public Transform note;
	public Transform filler;

	private float noteSpriteWidth;

	// Use this for initialization
	void Start () {
		noteSpriteWidth = note.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
	}
	
	// Update is called once per frame
	void Update () {
		float normalizedTime = aniToTrack.GetCurrentAnimatorStateInfo(0).normalizedTime;
		float aniCompletion = normalizedTime - Mathf.Floor(normalizedTime);

		percentComplete = aniCompletion;
		if (percentComplete < 0)
			percentComplete = 0;
		if (percentComplete > 1)
			percentComplete = 1;

		float maxWidth = filler.localScale.x - note.localScale.x * noteSpriteWidth;
		
		Vector3 notePos = note.position;
		notePos.x = filler.position.x + percentComplete * maxWidth;
		note.position = notePos;
	}
}
