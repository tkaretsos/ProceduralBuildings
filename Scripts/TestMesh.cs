using UnityEngine;

public class TestMesh : MonoBehaviour
{
  [SerializeField]
	private Material material;

	private Mesh mesh;
	private GameObject meshobj;
	private MeshFilter mf;
	private MeshRenderer mr;
		
//	void Awake ()
//	{
//		
//	}
	
	// Use this for initialization
	void Start ()
	{
		CreateNeoclassical();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	void CreateNeoclassical()
	{
    mesh = new Mesh();
		
		meshobj = new GameObject();
		mf = meshobj.AddComponent<MeshFilter>();
		mr = meshobj.AddComponent<MeshRenderer>();
		mr.sharedMaterial = material;
		
		Neoclassical neo = new Neoclassical(new Vector3( 4f + Random.Range(0.5f, 1.5f), 0,  1.5f + Random.Range(0.5f, 1.5f)),
																				new Vector3( 4f + Random.Range(0.5f, 1.5f), 0, -1.5f - Random.Range(0.5f, 1.5f)),
																				new Vector3(-4f - Random.Range(0.5f, 1.5f), 0, -1.5f - Random.Range(0.5f, 1.5f)),
																				new Vector3(-4f - Random.Range(0.5f, 1.5f), 0,  1.5f + Random.Range(0.5f, 1.5f)));
		neo.ConstructFaces();
    neo.ConstructFaceComponents();
		
		mesh.Clear();
		mesh.vertices = neo.boundariesArray;
		mesh.triangles = new int[] {
			0, 1, 5,
			0, 5, 4,
			1, 2, 6,
			1, 6, 5,
			2, 3, 7,
			2, 7, 6,
			3, 0, 4,
			3, 4, 7,
			4, 5, 6,
			4, 6, 7
		};
		// Assign UVs to shut the editor up -_-'
		mesh.uv = new Vector2[mesh.vertices.Length];
		for (int i = 0; i < mesh.vertices.Length; ++i)
			mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
		
		mesh.RecalculateNormals();
		mesh.Optimize(); 		
		mf.sharedMesh = mesh;
	}
}
