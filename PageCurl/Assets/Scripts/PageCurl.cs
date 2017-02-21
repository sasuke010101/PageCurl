using UnityEngine;
using System.Collections;

public class PageCurl : MonoBehaviour {

	[Range(0, 1.6f)]
	public float theta;

	public Vector2 A;

	public float B = 0;

	public float rho;

	private Mesh mesh;
	private Vector3[] vertices;
	void Start()
	{
		mesh = this.GetComponent<MeshFilter> ().mesh;
		vertices = new Vector3[mesh.vertices.Length];
		for (int i = 0; i < mesh.vertices.Length; i++) 
		{
			vertices[i] = mesh.vertices[i];
		}
//		Deform ();
	}

	private void Deform()
	{
		Vector3 vi = new Vector3();
		Vector3 v1 = new Vector3();
		Vector3 vo = new Vector3();

		Vector3[] newVertices = new Vector3[mesh.vertices.Length];
;
		float R, r, beta;
		for (int i = 0; i < mesh.vertices.Length; i++) 
		{
			vi = vertices[i];

			/*
			 * 
			 * 
			 * Version   A
			 * 
			 * 
			 */
//			R = Mathf.Sqrt(vi.x * vi.x + Mathf.Pow(vi.y - A.y ,2));
//
//			r = R * Mathf.Sin(theta);
//
//			beta = Mathf.Asin(vi.x / R) / Mathf.Sin(theta);
//
//
//			v1.x = r * Mathf.Sin(beta);
//			v1.y = R + A.y - r* (1 - Mathf.Cos(beta))*Mathf.Sin(theta);
//			v1.z = r * (1-Mathf.Cos(beta)) * Mathf.Cos(theta);


			/*
			 * 
			 * 
			 * VersionB
			 * 
			 */

			R = Mathf.Sqrt(Mathf.Pow(vi.x - B,2) + Mathf.Pow(vi.y - A.y ,2));
			r = R * Mathf.Sin(theta);
//			beta = Mathf.Asin(vi.x / R) / Mathf.Sin(theta);

			if(vi.x < B)
			{
				beta = 0;
			}else
			{
				beta = Mathf.Asin((vi.x-B) / R) / Mathf.Sin(theta);
			}

			if(vi.x < B)
			{
				v1.x = vi.x;
			}else{
				v1.x = B + r * Mathf.Sin(beta);
			}

			if(vi.x < B)
			{
				v1.y = vi.y;
			}else{
				v1.y = R + A.y - r* (1 - Mathf.Cos(beta))*Mathf.Sin(theta);
			}

			v1.z = r * (1-Mathf.Cos(beta)) * Mathf.Cos(theta);

			newVertices[i].x = v1.x;
			newVertices[i].y = v1.y;
			newVertices[i].z = v1.z;

			vo.x = (v1.x * Mathf.Cos(rho) - v1.z * Mathf.Sin(rho));
			vo.y = v1.y;
			vo.z = (v1.x * Mathf.Sin(rho) + v1.z * Mathf.Cos(rho));
		}

		mesh.vertices = newVertices;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.black;
		for (int i = 0; i < mesh.vertices.Length; i++) {

			if (mesh.vertices[i] == null) {
				return;
			}

			Gizmos.DrawSphere(mesh.vertices[i], 0.1f);
		}
	}

	// Update is called once per frame
	void Update () {
		Deform ();
	}

}



//http://blog.flirble.org/2010/10/08/the-anatomy-of-a-page-curl/
//http://wdnuon.blogspot.jp/2010/05/implementing-ibooks-page-curling-using.html
//http://www2.parc.com/istl/groups/uir/publications/items/UIR-2004-10-Hong-DeformingPages.pdf
