using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplashLogic : MonoBehaviour
{
	List<GameObject> splashes = new List<GameObject>();
	int currentSplash = 0;
	float t = 0.0f;
	const float timeBetweenSplash = 3.0f;
	bool isFading = false;
	bool doneFade = false;

	// Use this for initialization
	void Start()
	{
		t = 0.0f;
		currentSplash = 0;
		splashes.Add(GameObject.Find("VirtualBoys"));
		splashes.Add(GameObject.Find("TOJam9"));
		splashes.Add(GameObject.Find("TitleScreen"));
	}
	
	// Update is called once per frame
	void Update()
	{
		t += Time.deltaTime;
		if (isFading == false && (Input.anyKeyDown || (currentSplash < 2 && t >= timeBetweenSplash)))
		{
			//Debug.Log("--here");
			isFading = true;
			// tween it out
			if (currentSplash < 2)
			{
				//Debug.Log("--here 2");
				CameraFade.StartAlphaFade(Color.black, false, 3.0f, 0f, () => { TweenCallback(); });
			}
			else
			{
				//Debug.Log("--here 3");
				CameraFade.StartAlphaFade(Color.black, false, 3.0f, 0f, () => { StartGame(); });
			}
		}
		else if (doneFade)
		{
			//Debug.Log("--here 4");
			isFading = true;
			doneFade = false;
			CameraFade.StartAlphaFade(Color.black, true, 3.0f, 0f, () => { TweenCallback2(); });
			t = 0.0f;
		}
	}

	void TweenCallback()
	{
		//Debug.Log("--here 5");
		splashes[currentSplash++].SetActive(false);
		doneFade = true;
	}

	void TweenCallback2()
	{
		//Debug.Log("--here 6");
		t = 0.0f;
		isFading = false;
		doneFade = false;
	}

	void StartGame()
	{
		Application.LoadLevel("Title");
	}
}
