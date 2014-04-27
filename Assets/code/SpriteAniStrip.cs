using UnityEngine;
using System.Collections;

public class SpriteAniStrip : MonoBehaviour {

	public Sprite[] sprites;
	public bool running;
	public int iterations;
	public bool destroyWhenFinished;
	public bool repeatForever;

	public float fps;

	private float dt = 0;
	private int curIndex = 0;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (running && (iterations > 0 || repeatForever))
		{
			float frameTime = 1f / fps;
			int framesToAdvance = 0;

			dt += Time.deltaTime;
			while (dt >= frameTime)
			{
				dt -= frameTime;
				framesToAdvance++;
			}

			if (framesToAdvance > 0)
			{
				//count the number of iterations we just finished
				int nextFrame = curIndex + framesToAdvance;
				while (nextFrame >= sprites.Length)
				{
					nextFrame -= sprites.Length;
					iterations--;

				}

				setFrame(nextFrame);
			}

			if (iterations <= 0)
			{
				iterations = 0;
				
				if (destroyWhenFinished && !repeatForever)
				{
					Destroy(this.gameObject);
				}
			}
		}
	}

	public int CurFrame
	{
		get { return curIndex; }
		set { dt = 0; setFrame(value); }
	}

	protected void setFrame(int index)
	{
		index = index % sprites.Length;
		curIndex = index;

		spriteRenderer.sprite = sprites[curIndex];
	}
}
