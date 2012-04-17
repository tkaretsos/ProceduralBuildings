using UnityEngine;
using System.Collections.Generic;

using IDrawable = Thesis.Interface.IDrawable;

namespace Thesis {
namespace Base {

public class BalconyFloor : IDrawable
{
  /*************** FIELDS ***************/

  public readonly BalconyDoor parentBalconyDoor;

  public List<Vector3> boundaries = new List<Vector3>();

  public float height;

  public float width;

  public float depth;

  public Neoclassical parentBuilding
  {
    get { return (Neoclassical) parentBalconyDoor.parentBuilding; }
  }

  private Vector3[] _vertices;
  public Vector3[] vertices
  {
    get { return _vertices; }
  }

  private int[] _triangles;
  public int[] triangles
  {
    get { return _triangles; }
  }

  private GameObject _gameObject;
  public GameObject gameObject
  {
    get { return _gameObject; }
  }

  private MeshFilter _meshFilter;
  public MeshFilter meshFilter
  {
    get { return _meshFilter; }
  }

  /*************** CONSTRUCTORS ***************/

  public BalconyFloor (BalconyDoor parent)
  {
    parentBalconyDoor = parent;

    height = parentBuilding.balconyFloorHeight;
    width = parentBuilding.balconyFloorWidth;
    depth = parentBuilding.balconyFloorDepth;

    var tmp_right = parentBalconyDoor.parentFace.right * width;
    var tmp_normal = parentBalconyDoor.parentFace.normal * depth;

    boundaries.Add(parentBalconyDoor.boundaries[1] - tmp_right + tmp_normal);
    boundaries.Add(parentBalconyDoor.boundaries[1] - tmp_right - tmp_normal);
    boundaries.Add(parentBalconyDoor.boundaries[0] + tmp_right - tmp_normal);
    boundaries.Add(parentBalconyDoor.boundaries[0] + tmp_right + tmp_normal);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] + Vector3.up * height);
  }

  public virtual void FindVertices ()
  {
    _vertices = boundaries.ToArray();
  }

  public virtual void FindTriangles ()
  {
    _triangles = new int[] {
      // bottom
      0, 2, 1,
      0, 3, 2,

      // top
      4, 5, 6,
      4, 6, 7,

      // front
      0, 4, 7,
      0, 7, 3,

      // left side
      0, 1, 4,
      4, 1, 5,

      // right side
      3, 7, 6,
      3, 6, 2
    };
  }

  public virtual void Draw ()
  {
    _gameObject = new GameObject("balcony_floor");
    _gameObject.transform.parent = parentBuilding.gameObject.transform;
    _gameObject.isStatic = true;
    var meshRenderer = _gameObject.AddComponent<MeshRenderer>();
    _meshFilter = _gameObject.AddComponent<MeshFilter>();
    _gameObject.active = false;
    _gameObject.transform.position = parentBuilding.meshOrigin;

    meshRenderer.sharedMaterial = Resources.Load("Materials/Building", typeof(Material)) as Material;

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

    _meshFilter.sharedMesh = mesh;
  }
}

} // namespace Base
} // namespace Thesis