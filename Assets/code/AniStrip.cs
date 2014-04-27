using UnityEngine;
using System.Collections;

/**
 * @author cregnier
 * 
 * Sets up an animation strip that assumes each cell of the animation is the same size,
 * and the texture size is split evenly among all tiles horizontally and vertically.
 * 
 * Cell numbering is from top left to bottom right.
 * 
 */
public class AniStrip : MonoBehaviour {

	public Material mat;
	public string texPropertyName;

	public int numFrames;

	public float frameRate;

	public int cellWidth;
	public int cellHeight;

	public float speedMultiplier;

	private int numTilesWide;
	private int numTilesHigh;

	private float scaleW;
	private float scaleH;

	private int curFrame = 0;

	private float dt;
	private float timePerFrame;

	void Awake()
	{
		if (mat != null && mat.HasProperty(texPropertyName))
		{
			//need to clone the material so we have our own version
			mat = new Material(mat);

			Texture tex = mat.GetTexture(texPropertyName);
			if (cellWidth <= 0)
				cellWidth = tex.width;
			if (cellHeight <= 0)
				cellHeight = tex.height;

			numTilesWide = tex.width / cellWidth;
			numTilesHigh = tex.height / cellHeight;
			scaleW = (float)cellWidth / tex.width;
			scaleH = (float)cellHeight / tex.height;

			mat.SetTextureScale(texPropertyName, new Vector2(scaleW, scaleH));
			CurFrame = 0;
		}
		else
		{
			if (mat != null)
				Debug.LogError("Material (" + mat.name + ") does not have property: " + texPropertyName);
			else
				Debug.LogError("AniStrip (" + name + ") does not have a proper Material");
		}

		renderer.material = mat;

		timePerFrame = 1f / frameRate;

		speedMultiplier = 1f;
	}

	public int CurFrame
	{
		get { return curFrame; }
		set { setFrame(value); }
	}

	public int NumFrames
	{
		get { return numFrames; }
	}

	public float FrameOffsetTime
	{
		get { return dt; }
		set { dt = value; }
	}

	public Material AniMaterial
	{
		get { return mat; }
	}

	protected void setFrame(int frame)
	{
		if (frame >= NumFrames)
			frame = frame % NumFrames;

		int col = frame % numTilesWide;
		int row = frame / numTilesWide;
		mat.SetTextureOffset(texPropertyName, new Vector2(col * scaleW, (numTilesHigh - row - 1) * scaleH));

		curFrame = frame;
	}

	public void Update()
	{
		dt += Time.deltaTime * speedMultiplier;
		int framesToAdvance = 0;
		while (dt > timePerFrame)
		{
			dt -= timePerFrame;
			framesToAdvance++;
		}

		if (framesToAdvance > 0)
		{
			//Debug.Log("advancing " + framesToAdvance);
			setFrame(curFrame + framesToAdvance);
		}

	}
}
