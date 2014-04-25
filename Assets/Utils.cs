using UnityEngine;
using System.Collections.Generic;

public static class Utils
{
	public static void SetTexture(Vector2 coord, Vector2 rowscols, GameObject rendererGO)
	{
		SetBadTexture(coord, rowscols, rendererGO);
	}

	public static void SetBadTexture(Vector2 coord, Vector2 rowscols, GameObject rendererGO)
	{
		Vector2 scale  = new Vector2(1.0f / rowscols.x, 1.0f / rowscols.y); // Assuming square sprite.
		Vector2 offset = new Vector2(scale.x * coord.x, 1 - ((scale.y * coord.y) + scale.y));

		if (rendererGO)
		{
		/**
		 * This is bad because it creates an instance of the material assigned to the rendererGO's meshfilter
		 */
			rendererGO.renderer.material.mainTextureScale  = scale;
			rendererGO.renderer.material.mainTextureOffset = offset;
		}
	}
}
