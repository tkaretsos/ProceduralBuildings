using UnityEngine;
using System.Collections.Generic;

using ICombinable = Thesis.Interface.ICombinable;
using IDrawable = Thesis.Interface.IDrawable;

namespace Thesis {
namespace Base {

public class ComponentFrame : IDrawable, ICombinable
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
    get { return parentComponent.parentFace.parentBuilding; }
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
    set { _triangles = value; }
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

  public ComponentFrame (FaceComponent parent, string name, string materialName)
  {
    parentComponent = parent;

    _name = name;
    _materialName = materialName;

    //foreach (var point in parentComponent.boundaries)
    //  boundaries.Add(point + parentBuilding.meshOrigin);
    boundaries = new Vector3[8];
    for (var i = 0; i < 4; ++i)
    {
      boundaries[i] = parentComponent.boundaries[i] + parentBuilding.meshOrigin;
      boundaries[i + 4] = boundaries[i] - (parentComponent.depth - 0.001f) * parentComponent.normal;
    }
  }

  /*************** METHODS ***************/

  public virtual void FindVertices ()
  {
    _vertices = boundaries;
  }

  public virtual void FindTriangles ()
  {
    _triangles = new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4
    };
  }

  public virtual void Draw ()
  {
    _gameObject = new GameObject(_name);
    _gameObject.transform.parent = parentBuilding.gameObject.transform;
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