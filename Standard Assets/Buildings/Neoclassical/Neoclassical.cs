using UnityEngine;

using Exception = System.Exception;

public sealed class Neoclassical : Building
{
  // fields
  private const float _component_width_min = 1.5f;
  private const float _component_width_max = 1.75f;
  private const float _component_space_min = 2f;
  private const float _component_space_max = 2.25f;

  private GameObject meshobj;
  private Mesh mesh;
  private MeshFilter mf;
  private MeshRenderer mr;
  private Material _material;

  public GameObject gameObject
  {
    get { return meshobj; }
  }

  // constructors
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Neoclassical"/> class.
  /// The boundaries of the base of this building must be given in 
  /// clockwise order.
  /// </summary>
  /// <param name='p1'>
  /// A point in space.
  /// </param>
  /// <param name='p2'>
  /// A point in space.
  /// </param>
  /// <param name='p3'>
  /// A point in space.
  /// </param>
  /// <param name='p4'>
  /// A point in space.
  /// </param>
  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Material material)
  : base(p1, p2, p3, p4)
  {
    floorHeight = Random.Range(4.5f, 5f);
    floorNumber = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});

    _material = material;

    ConstructFaces();
    ConstructFaceComponents();
    ConstructGameObject();
  }
  
  
  // methods

  public void ConstructFaceComponents ()
  {
    if (_faces.Count == 0) throw new Exception("There are no faces to construct the components.");
  
    float component_width = Random.Range(_component_width_min, _component_width_max);
    float inbetween_space = Random.Range(_component_space_min, _component_space_max);
  
    foreach (Face face in _faces)
      face.ConstructFaceComponents(component_width, inbetween_space);
  }

  public override void ConstructFaces ()
  {
    _faces.Add(new NeoclassicalFace(this, _boundaries[0], _boundaries[1]));
    _faces.Add(new NeoclassicalFace(this, _boundaries[1], _boundaries[2]));
    _faces.Add(new NeoclassicalFace(this, _boundaries[2], _boundaries[3]));
    _faces.Add(new NeoclassicalFace(this, _boundaries[3], _boundaries[0]));
  }

  public void ConstructGameObject ()
  {
    mesh = new Mesh();
    meshobj = new GameObject();
    mf = meshobj.AddComponent<MeshFilter>();
    mr = meshobj.AddComponent<MeshRenderer>();
    mr.sharedMaterial = _material;

    mesh.Clear();
    mesh.vertices = FindVertices();
    mesh.triangles = FindTriangles();
    // Assign UVs to shut the editor up -_-'
    mesh.uv = new Vector2[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; ++i)
      mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

    mesh.RecalculateNormals();
    mesh.Optimize();
    mf.sharedMesh = mesh;
  }
}
