using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleAnim : MonoBehaviour
{
	List<Vector3> animFrames = new List<Vector3>();
	float t                  = 0.0f;
	int thisFrame            = 0;

	public void SetAnim(List<Vector3> frames)
	{
		animFrames = frames;
		t          = 0.0f;
		thisFrame  = 0;
		if (animFrames.Count > 0)
		{
			//Utils.SetTexture(new Vector2(x, y), new Vector2(rows, cols), gameObject);
		}
	}

	void Update()
	{
		// Advance animation.
		if (0 == animFrames.Count) return;
		t += Time.deltaTime;
		if (0 < animFrames[thisFrame].z && t >= animFrames[thisFrame].z)
		{
			while (t >= animFrames[thisFrame].z)
			{
				t -= animFrames[thisFrame].z;
				++thisFrame;
				if (thisFrame >= animFrames.Count)
				{
					thisFrame = 0;
				}
			}
			//Utils.SetTexture(new Vector2(x, y), new Vector2(rows, cols), gameObject);
		}
	}
}
