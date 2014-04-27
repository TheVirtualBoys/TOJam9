using UnityEngine;
using System.Collections;

public class FeelingsBar : MonoBehaviour {

	public Transform filler;
	public Transform rightEdge;

	public Sprite[] feelingSprites;

	public int level;
	public int maxLevel;

	// Update is called once per frame
	void Update () {
		if (filler.localScale.x != level && level >= 0 && level < maxLevel)
		{
			int localLevel = Mathf.Max(Mathf.Min(level, maxLevel), 0);

			Vector3 scale = filler.localScale;
			scale.x = localLevel;
			filler.localScale = scale;

			Vector3 rhPos = rightEdge.position;
			rhPos.x = filler.position.x + localLevel;
			rightEdge.position = rhPos;

			SpriteRenderer renderer = rightEdge.GetComponent<SpriteRenderer>();

			float feelingsPercent = (float)localLevel / (float)maxLevel;
			int spriteIndex = feelingSprites.Length - 1 - (int)(feelingsPercent * (feelingSprites.Length));
			renderer.sprite = feelingSprites[spriteIndex];
		}
	}
}
