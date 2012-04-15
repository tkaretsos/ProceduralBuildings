using System;
using System.Collections.Generic;
using UnityEngine;

public class Drawable
{
  /*************** FIELDS ***************/
  public GameObject gameObject;

  public Mesh mesh;

  /// <summary>
  /// The origin of the building's mesh
  /// </summary>
  public Vector3 meshOrigin = new Vector3();

  public MeshRenderer meshRenderer;

  public MeshFilter meshFilter;

  public Material material;

  public bool active
  {
    get {
      if (gameObject == null)
        return _isActive;
      else
        return gameObject.active;
    }
    set {
      if (gameObject == null)
        _isActive = value;
      else
        gameObject.active = value;
    }
  }

  public Transform transform
  {
    get { return gameObject.transform; }
  }

  private string _name;

  private bool _isActive;

  /*************** CONSTRUCTORS ***************/

  public Drawable (string name = "drawable_object",
                   string materialName = null,
                   bool isActive = true)
  {
    _name = name;
    _isActive = isActive;

    if (materialName != null)
      material = Resources.Load("Materials/" + materialName, typeof(Material)) as Material;
  }

  /*************** METHODS ***************/

  public virtual void Draw ()
  {
    gameObject = new GameObject(_name);
    gameObject.isStatic = true;
    meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshFilter = gameObject.AddComponent<MeshFilter>();
    gameObject.active = _isActive;
    gameObject.transform.position = meshOrigin;

    if (material != null)
      meshRenderer.sharedMaterial = material;

    mesh = new Mesh();
    mesh.Clear();

    mesh.vertices = FindVertices();
    mesh.triangles = FindTriangles();
    mesh.uv = new Vector2[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; ++i)
      mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

    mesh.RecalculateNormals();
    mesh.Optimize();

    meshFilter.sharedMesh = mesh;
  }

  public virtual Vector3[] FindVertices ()
  {
    throw new NotImplementedException();
  }

  public virtual int[] FindTriangles ()
  {
    throw new NotImplementedException();
  }
}
