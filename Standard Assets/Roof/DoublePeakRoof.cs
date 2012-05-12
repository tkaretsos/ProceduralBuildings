using UnityEngine;

namespace Thesis {
  
public class DoublePeakRoof : Roof
{
  public DoublePeakRoof (BuildingMesh parent)
    : base(parent)
  {
    boundaries = new Vector3[6];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentMesh.roofBase.boundaries[i + 4];

    FindMeshOrigin(boundaries[0], boundaries[2],
                   boundaries[1], boundaries[3]);

    int index = Mathf.Max(parentMesh.sortedFaces[2], parentMesh.sortedFaces[3]);
    Face face = parentMesh.faces[index];
    var dist = 0.4f * face.width;
    height = 0.5f * dist;
    var p = (face.boundaries[0] + face.boundaries[1]) / 2 - dist * face.normal;

    boundaries[4] = new Vector3(p.x,
                                boundaries[0].y + height,
                                p.z);

    index = Mathf.Min(parentMesh.sortedFaces[2], parentMesh.sortedFaces[3]);
    face = parentMesh.faces[index];
    p = (face.boundaries[0] + face.boundaries[1]) / 2 - dist * face.normal;

    boundaries[5] = new Vector3(p.x,
                                boundaries[0].y + height,
                                p.z);
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 6];
    for (int i = 0; i < 6; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[18];
    int i = 0;

    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 5;

    // +6
    triangles[i++] = 6;
    triangles[i++] = 11;
    triangles[i++] = 10;

    // +12
    triangles[i++] = 12;
    triangles[i++] = 16;
    triangles[i++] = 15;

    // +18
    triangles[i++] = 19;
    triangles[i++] = 20;
    triangles[i++] = 23;

    // +24
    triangles[i++] = 26;
    triangles[i++] = 27;
    triangles[i++] = 28;

    // +30
    triangles[i++] = 32;
    triangles[i++] = 34;
    triangles[i++] = 35;
  }
}

} // namespace Thesis