using UnityEngine;
using System.Collections;

public class CreateMesh : MonoBehaviour {

	private GameObject mMesh;
	private Material mMaterial;

	/*   Mesh
	 *     with  length
	 *     segment
	 *     height
	 *
	 */

	private Vector2 size;
//	private float height = 0;
	private Vector2 segment;


	/*  vertex
	 *  uv
	 *  triangles
	 */

	private Vector3[] vertexes;
	private int[] triangles;


	/*
	 * compute vertexes
	*/

	private void computeVertexes()
	{
		int sum = Mathf.FloorToInt ((segment.x + 1)*(segment.y + 1));
		float w = size.x / segment.x;
		float h = size.y / segment.y;

		GetTriangles ();

		int index = 0;
		vertexes = new Vector3[sum];
		for (int i = 0; i < segment.y+1; i++) 
		{
			for (int j = 0; j< segment.x + 1; j++)
			{
//				float tempHeight = 0;
				vertexes[index] = new Vector3(j*w,0,i*h);
				index++;
			}
		}

	}

	/*
	 * getTriangles
	 */

	private int[] GetTriangles()
	{
		int sum = Mathf.FloorToInt (segment.x * segment.y * 6);//三角形顶点总数：假设是1*1的网格，会有2个顶点复用，因此是6个顶点。假设是2*2的网格，则是4个1*1的网格，即4*6即2*2*6！
		triangles = new int[sum];
		uint index = 0;

		for (int i = 0; i < segment.y; i++) 
		{
			for(int j = 0; j< segment.x; j++)
			{
				int role = Mathf.FloorToInt(segment.x)+1;
				int self = j + (i*role);
				int next = j + ((i+1)*role);


				//first triangle
				triangles[index] = self;
				triangles[index + 1] = next + 1;
				triangles[index + 2] = self + 1;

				//second triangle
				triangles[index + 3] =  self;
				triangles[index + 4] = next;
				triangles[index + 5] = next + 1;
				index += 6;

			}
		}

		return triangles;
	}

	private void DrawMesh()
	{
		Mesh mesh = mMesh.AddComponent<MeshFilter> ().mesh;
		mMesh.AddComponent<MeshRenderer> ();

		mMaterial = new Material(Shader.Find("custom/BumpSpec_Twoside"));

		mMesh.GetComponent<Renderer> ().material = mMaterial;

		mesh.Clear ();
		mesh.vertices = vertexes;

		mesh.triangles = triangles;

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
	}

	private void createMesh(float width, float height,uint segmentX, uint segmentY,int min,int max)
	{
		size = new Vector2 (width, height);
		segment = new Vector2 (segmentX, segmentY);

		if (mMesh != null)
		{
			Destroy(mMesh);
		}

		mMesh = new GameObject ();
		mMesh.name = "CreateMesh";

		computeVertexes ();
		DrawMesh ();
	}

	// Use this for initialization
	void Start () {
		createMesh (10, 10, 3, 3, -10, 10);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
