using UnityEngine;
using System.Collections;

public class SimpleSprite : MonoBehaviour
{
	//MeshRenderer renderer = null;
	MeshFilter mesh = null;
	
	void Start()
	{
		//renderer  = gameObject.AddComponent<MeshRenderer>();
		//renderer.material = SetMaterial();
		mesh      = gameObject.AddComponent<MeshFilter>();// = mesh = new MeshFilter();
		mesh.mesh = CreateQuad();
	}

	Mesh CreateQuad()
	{
		Mesh mesh = new Mesh();

		Vector3[] verts = new Vector3[] {
			new Vector3(0.0f, 0.0f, 0.0f),
			new Vector3(0.0f, 0.5f, 0.0f),
			new Vector3(0.5f, 0.5f, 0.0f),
			new Vector3(0.5f, 0.0f, 0.0f)
		};

		Vector2[] uvs = new Vector2[] {
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(0, 0)
		};

		int[] tris = new int[] {
			0, 1, 2,
			2, 3, 0
		};

		mesh.vertices  = verts;
		mesh.uv        = uvs;
		mesh.triangles = tris;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	Material SetMaterial()
	{
		return new Material(Shader.Find("Diffuse"));
	}
}
