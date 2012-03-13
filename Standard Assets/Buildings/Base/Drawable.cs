using System;
using System.Collections.Generic;
using UnityEngine;

public class Drawable
{
  /*************** FIELDS ***************/
  public readonly GameObject gameObject;

  public Mesh mesh;

  public MeshRenderer meshRenderer;

  public MeshFilter meshFilter;

  public Material material;

  public bool active
  {
    get { return gameObject.active; }
    set { gameObject.active = value; }
  }

  public Transform transform
  {
    get { return gameObject.transform; }
  }

  /*************** CONSTRUCTORS ***************/

  public Drawable (string name = "drawable_object",
                   string materialName = null,
                   bool isActive = true)
  {
    gameObject = new GameObject(name);
    gameObject.isStatic = true;
    meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshFilter = gameObject.AddComponent<MeshFilter>();
    gameObject.active = isActive;

    if (materialName != null)
      material = Resources.Load("Materials/" + materialName, typeof(Material)) as Material;
  }

  /*************** METHODS ***************/

  public virtual void Draw ()
  {
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

  public void SetActiveRecursively (bool status)
  {
    gameObject.SetActiveRecursively(status);
  }
}
