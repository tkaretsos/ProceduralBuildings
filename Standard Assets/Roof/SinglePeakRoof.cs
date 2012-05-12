using UnityEngine;

namespace Thesis {

public class SinglePeakRoof : Roof
{
  public SinglePeakRoof (BuildingMesh parent)
    : base(parent)
  {
    height = 1f;

    boundaries = new Vector3[5];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentMesh.boundaries[i + 4];

    FindMeshOrigin(boundaries[0], boundaries[2],
                   boundaries[1], boundaries[3]);

    boundaries[4] = new Vector3(meshOrigin.x,
                                boundaries[0].y + height,
                                meshOrigin.z);
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length << 2];
    for (int i = 0; i < 4; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[12];
    int i = 0;

    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 4;

    // +5
    triangles[i++] = 6;
    triangles[i++] = 7;
    triangles[i++] = 9;

    // +10
    triangles[i++] = 12;
    triangles[i++] = 13;
    triangles[i++] = 14;

    // +15
    triangles[i++] = 18;
    triangles[i++] = 15;
    triangles[i++] = 19;
  }
}

}