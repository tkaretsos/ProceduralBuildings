using UnityEngine;

namespace Thesis {

public class SinglePeakRoof : Roof
{
  public SinglePeakRoof (BuildingMesh parent)
    : base(parent)
  {
    height = 1f;
    width = 0.2f;

    boundaries = new Vector3[5];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentMesh.roofBase.boundaries[i + 4] +
                      width * parentMesh.faces[i].normal +
                      width * parentMesh.faces[(i + 3) % 4].normal;

    FindMeshOrigin(boundaries[0], boundaries[2],
                   boundaries[1], boundaries[3]);

    boundaries[4] = new Vector3(meshOrigin.x,
                                boundaries[0].y + height,
                                meshOrigin.z);
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 5];
    for (int i = 0; i < 5; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[18];
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

    triangles[i++] = 20;
    triangles[i++] = 23;
    triangles[i++] = 22;

    triangles[i++] = 20;
    triangles[i++] = 22;
    triangles[i++] = 21;
  }

  public override void Draw()
  {
    base.Draw();

    var uvs = new Vector2[mesh.vertices.Length];

    float _wdiv = material.mainTexture.width / 128f;
    float _hdiv = material.mainTexture.height / 128f;
    float _dist01 = (boundaries[0] - boundaries[1]).magnitude;
    float _dist12 = (boundaries[1] - boundaries[2]).magnitude;
    float htimes =  _dist01 / _wdiv;
    float vtimes = Mathf.Sqrt(Mathf.Pow(_dist12 / 2, 2) +
                              Mathf.Pow(height, 2)) / _hdiv;

    uvs[1] = new Vector2(0f, 0f);
    uvs[4] = new Vector2(htimes / 2, vtimes);
    uvs[0] = new Vector2(htimes, 0f);
    uvs[13] = new Vector2(0f, 0f);
    uvs[14] = new Vector2(htimes / 2, vtimes);
    uvs[12] = new Vector2(htimes, 0f);

    htimes = _dist12 / _wdiv;
    vtimes = Mathf.Sqrt(Mathf.Pow(_dist01 / 2, 2) +
                        Mathf.Pow(height, 2)) / _hdiv;

    uvs[7] = new Vector2(0f, 0f);
    uvs[9] = new Vector2(htimes / 2, vtimes);
    uvs[6] = new Vector2(htimes, 0f);
    uvs[15] = new Vector2(0f, 0f);
    uvs[19] = new Vector2(htimes / 2, vtimes);
    uvs[18] = new Vector2(htimes, 0f);
    
    mesh.uv = uvs;
  }
}

}