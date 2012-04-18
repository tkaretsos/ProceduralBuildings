using UnityEngine;
using Thesis.Interface;

namespace Thesis {

public class DrawableObject : ProceduralObject, 
                              IDrawable, 
                              ICombinable
{
  /*************** FIELDS ***************/

  public string       name;

  public string       materialName;

  public GameObject   gameObject;

  public MeshFilter   meshFilter;

  public Vector3      meshOrigin;

  public Vector3[]    vertices;

  public int[]        triangles;

  /*************** CONSTRUCTORS ***************/

  public DrawableObject (string name = "drawableObject", string materialName = null)
  {
    this.name = name;
    if (materialName != null)
      this.materialName = materialName;
  }

  /*************** METHODS ***************/

  /// <summary>
  /// Calculates the center of the quadrangle base of the building.
  /// Used for properly creating the gameObject and serves as the origin
  /// of the created mesh.
  /// </summary>
  public virtual void FindMeshOrigin (Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
  {
    var a_dir = a1 - a2;
    var b_dir = b1 - b2;
    var dir_cross = Vector3.Cross(a_dir, b_dir);
    var tmp_cross = Vector3.Cross(b1 - a1, b_dir);

    float c1;
    if (dir_cross.x != 0f)
     c1 = tmp_cross.x / dir_cross.x;
    else if (dir_cross.y != 0f)
      c1 = tmp_cross.y / dir_cross.y;
    else
      c1 = tmp_cross.z / dir_cross.z;

    meshOrigin = a1 + c1 * a_dir;
  }

  public virtual void FindVertices ()
  {
    throw new System.NotImplementedException();
  }

  public virtual void FindTriangles ()
  {
    throw new System.NotImplementedException();
  }

  public virtual void Draw ()
  {
    gameObject = new GameObject(name);
    gameObject.active = false;
    gameObject.isStatic = true;
    gameObject.transform.position = meshOrigin;
    
    var renderer = gameObject.AddComponent<MeshRenderer>();
    if (materialName != null)
      renderer.sharedMaterial = Resources.Load("Materials/" + materialName, 
                                               typeof(Material)) as Material;

    var mesh = new Mesh();
    mesh.Clear();

    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // this is required to stop a runtime warning
    mesh.uv = new Vector2[mesh.vertices.Length];
    //for (int i = 0; i < mesh.vertices.Length; ++i)
    //  mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

    mesh.RecalculateNormals();
    mesh.Optimize();

    meshFilter = gameObject.AddComponent<MeshFilter>();
    meshFilter.sharedMesh = mesh;
  }

  /*************** INTERFACE EXPLICIT IMPLEMENTATION ***************/

  Vector3[] IDrawable.vertices
  {
    get { return vertices; }
  }

  int[] IDrawable.triangles
  {
    get { return triangles; }
  }

  GameObject ICombinable.gameObject
  {
    get { return gameObject; }
  }

  MeshFilter ICombinable.meshFilter
  {
    get { return meshFilter; }
  }
}

} // namespace Thesis