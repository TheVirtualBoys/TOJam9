using UnityEngine;
using System.Collections;

public class NoteProgressBar : MonoBehaviour {

	public float percentComplete;

	public Animator aniToTrack;

	public Transform note;
	public Transform filler;

	public float maxBounce;
	public float freq;

	private float noteSpriteWidth;

	// Use this for initialization
	void Start () {
		noteSpriteWidth = note.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

		//aniToTrack.Play("FieldScrollingAnim", -1, 0.9f);
	}
	
	// Update is called once per frame
	void Update () {
		if (percentComplete < 1f)
		{
			float normalizedTime = aniToTrack.GetCurrentAnimatorStateInfo(0).normalizedTime;
			float aniCompletion = normalizedTime - Mathf.Floor(normalizedTime);

			if (aniCompletion < percentComplete)
			{
				Debug.Log("ani: " + aniCompletion + ", %: " + percentComplete);

				//for some reason the following animation is before the current % complete, so it must've looped, so consider % complete done now
				percentComplete = 1;
			}
			else
			{
				percentComplete = aniCompletion;
				if (percentComplete < 0)
					percentComplete = 0;
				if (percentComplete >= 1)
				{
					percentComplete = 1;
				}
			}

			float maxWidth = filler.localScale.x - note.localScale.x * noteSpriteWidth;

			Vector3 notePos = note.position;
			notePos.x = filler.position.x + percentComplete * maxWidth;
			notePos.y = filler.position.y + maxBounce * Mathf.Sin(percentComplete * 2 * Mathf.PI * freq);
			note.position = notePos;

			if (percentComplete >= 1)
			{
				OnWin();
			}
		}
	}

	void OnWin()
	{
		Transform[] characters = null;

		GameObject mainCamera = GameObject.Find("Main Camera");
		if (mainCamera != null)
		{
			CharacterCollection col = mainCamera.GetComponent<CharacterCollection>();
			if (col != null)
			{
				characters = col.characters;
			}
		}

		if (characters != null)
		{
			foreach (Transform character in characters)
			{
				playAnimation(character.gameObject, false);
				showWinAnimation(character.gameObject);
			}
		}

		GameObject field = GameObject.Find("Field");
		if (field != null)
		{
			Animator ani = field.GetComponent<Animator>();
			ani.enabled = false;
		}

		CameraFade.StartAlphaFade(Color.black, false, 5.0f, 0f, () => { RestartScene(); });

	}

	private void playAnimation(GameObject character, bool running)
	{
		if (character != null)
		{
			AniStrip ani = character.GetComponent<AniStrip>();
			ani.running = running;
		}
	}

	private void showWinAnimation(GameObject character)
	{
		WinLoseSpriteSwapper spriteSwapper = character.GetComponent<WinLoseSpriteSwapper>();
		if (spriteSwapper != null)
		{
			spriteSwapper.OnWin();
		}
	}

	private void RestartScene()
	{
		AudioScript.Reset();
		Application.LoadLevel("Title");
	}

}
