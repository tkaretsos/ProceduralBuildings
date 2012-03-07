using UnityEngine;
using System.Collections.Generic;

public class WindowFrame
{
  public readonly Base.FaceComponent parentWindow;
  public List<Vector3> boundaries = new List<Vector3>(8);

  public GameObject gameObject;
  public Material material;

  public Base.Face parentFace
  {
    get { return parentWindow.parentFace; }
  }

  public Base.Building parentBuilding
  {
    get { return parentWindow.parentBuilding; }
  }

  public WindowFrame (Base.FaceComponent parent)
  {
    parentWindow = parent;
    material = Resources.Load("Materials/WindowFrameMaterial", typeof(Material)) as Material;

    foreach (Vector3 point in parentWindow.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - 0.2f * parentWindow.normal);

    gameObject = new GameObject("Window Frame");
    gameObject.isStatic = true;
    gameObject.transform.parent = parentBuilding.gameObject.transform;
    Render();
  }

  public void Render ()
  {
    MeshRenderer _mesh_renderer = gameObject.AddComponent<MeshRenderer>();
    _mesh_renderer.sharedMaterial = material;

    Mesh _mesh = new Mesh();
    _mesh.Clear();
    _mesh.vertices = boundaries.ToArray();
    _mesh.triangles = new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4
    };
    _mesh.uv = new Vector2[_mesh.vertices.Length];
    for (int i = 0; i < _mesh.vertices.Length; ++i)
      _mesh.uv[i] = new Vector2(_mesh.vertices[i].x, _mesh.vertices[i].y);

    _mesh.RecalculateNormals();
    _mesh.Optimize();

    MeshFilter _mesh_filter = gameObject.AddComponent<MeshFilter>();
    _mesh_filter.sharedMesh = _mesh;
  }
}
