using System.Collections.Generic;
using UnityEngine;

using IDrawable = Thesis.Interface.IDrawable;

namespace Thesis {
namespace Base {

public class ComponentBody : IDrawable
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public Vector3[] boundaries;

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public Building parentBuilding
  {
    get { return parentComponent.parentBuilding; }
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

  private string _name;

  private string _materialName;

  /*************** CONSTRUCTORS ***************/

  public ComponentBody (FaceComponent parent, string name, string materialName)
  {
    parentComponent = parent;
    _name = name;
    _materialName = materialName;

    boundaries = new Vector3[parentComponent.boundaries.Length];
    for (var i = 0; i < parentComponent.boundaries.Length; ++i)
      boundaries[i] = parentComponent.boundaries[i] -
                      parentComponent.depth * parentComponent.normal +
                      parentBuilding.meshOrigin;
  }

  /*************** METHODS ***************/

  public virtual void FindVertices ()
  {
    _vertices = boundaries;
  }

  public virtual void FindTriangles ()
  {
    _triangles = new int[] {
      0, 1, 3,
      1, 2, 3
    };
  }

  public virtual void Draw ()
  {
    _gameObject = new GameObject(_name);
    _gameObject.transform.parent = parentBuilding.gameObject.transform;
    _gameObject.isStatic = true;
    var renderer = _gameObject.AddComponent<MeshRenderer>();
    _meshFilter = _gameObject.AddComponent<MeshFilter>();
    _gameObject.active = true;

    renderer.sharedMaterial = Resources.Load("Materials/" + _materialName, 
                                             typeof(Material)) as Material;

    var mesh = new Mesh();
    mesh.Clear();

    mesh.vertices = vertices;
    mesh.triangles = triangles;
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