using UnityEngine;
using System.Collections;

public class SimpleSprite : MonoBehaviour
{
	MeshRenderer renderer = null;
	MeshFilter mesh = null;
	
	void Start()
	{
		mesh = new MeshFilter();
		mesh.mesh = CreateQuad();
	}

	Mesh CreateQuad()
	{
		Mesh mesh = new Mesh();

		Vector3[] verts = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(0, 1, 0),
			new Vector3(1, 1, 0)
		};

		Vector2[] uvs = new Vector2[] {
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(0, 0)
		};

		int[] tris = new int[] {
			0, 1, 2,
			2, 1, 3
		};

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;
		return mesh;
	}

	void SetMaterial()
	{
	}
}
