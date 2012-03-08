using UnityEngine;
using System.Collections.Generic;

namespace Base {

public class WindowFrame
{
  public readonly Base.FaceComponent parentWindow;
  public List<Vector3> boundaries = new List<Vector3>();

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
    gameObject.active = false;
    gameObject.isStatic = true;
    gameObject.transform.parent = parentBuilding.gameObject.transform;
  }

  public void Render ()
  {
    var meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshRenderer.sharedMaterial = material;

    var mesh = new Mesh();
    mesh.Clear();
    mesh.vertices = boundaries.ToArray();
    mesh.triangles = new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4
    };
    mesh.uv = new Vector2[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; ++i)
      mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

    mesh.RecalculateNormals();
    mesh.Optimize();

    var meshFilter = gameObject.AddComponent<MeshFilter>();
    meshFilter.sharedMesh = mesh;
  }
}

} // namespace Base
