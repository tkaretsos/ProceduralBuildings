using UnityEngine;

namespace Thesis {
  
public class DoublePeakRoof : Roof
{
  private enum _Type { four, five }
  private _Type _type;
  private float _widthBig;
  private float _widthSmall;
  private float _heightBig;
  private float _heightSmall;

  public DoublePeakRoof (BuildingMesh parent)
    : base(parent)
  {
    width = 0.2f;
    boundaries = new Vector3[6];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentMesh.roofBase.boundaries[i + 4]+
                      width * parentMesh.faces[i].normal +
                      width * parentMesh.faces[(i + 3) % 4].normal;

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

    if ((boundaries[0] - boundaries[4]).magnitude <
        (boundaries[0] - boundaries[5]).magnitude)
    {
      _type = _Type.four;
      _widthBig = (boundaries[0] - boundaries[1]).magnitude;
      _widthSmall = (boundaries[1] - boundaries[2]).magnitude;
      _heightSmall = ((boundaries[1] + boundaries[2]) / 2 - boundaries[5]).magnitude;
    }
    else
    {
      _type = _Type.five;
      _widthBig = (boundaries[1] - boundaries[2]).magnitude;
      _widthSmall = (boundaries[1] - boundaries[0]).magnitude;
      _heightSmall = ((boundaries[1] + boundaries[0]) / 2 - boundaries[5]).magnitude;
    }

    _heightBig = Mathf.Sqrt(Mathf.Pow(_widthSmall / 2, 2) +
                            Mathf.Pow(height, 2));
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 4];
    for (int i = 0; i < 4; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[24];
    int i = 0;

    if (_type == _Type.five)
    {
      triangles[i++] = 0;
      triangles[i++] = 1;
      triangles[i++] = 5;

      triangles[i++] = 2;
      triangles[i++] = 3;
      triangles[i++] = 4;

      // +6
      triangles[i++] = 7;
      triangles[i++] = 8;
      triangles[i++] = 11;

      triangles[i++] = 8;
      triangles[i++] = 10;
      triangles[i++] = 11;

      // +12
      triangles[i++] = 12;
      triangles[i++] = 17;
      triangles[i++] = 16;

      triangles[i++] = 12;
      triangles[i++] = 16;
      triangles[i++] = 15;
    }
    else
    {
      triangles[i++] = 1;
      triangles[i++] = 2;
      triangles[i++] = 5;

      triangles[i++] = 0;
      triangles[i++] = 4;
      triangles[i++] = 3;

      // +6
      triangles[i++] = 6;
      triangles[i++] = 7;
      triangles[i++] = 11;

      triangles[i++] = 6;
      triangles[i++] = 11;
      triangles[i++] = 10;

      // +12
      triangles[i++] = 14;
      triangles[i++] = 15;
      triangles[i++] = 16;

      triangles[i++] = 14;
      triangles[i++] = 16;
      triangles[i++] = 17;
    }

    // +18
    triangles[i++] = 18;
    triangles[i++] = 21;
    triangles[i++] = 20;

    triangles[i++] = 18;
    triangles[i++] = 20;
    triangles[i++] = 19;
  }

  public override void Draw()
  {
    base.Draw();

    var uvs = new Vector2[mesh.vertices.Length];

    float _wdiv = material.mainTexture.width / 128f;
    float _hdiv = material.mainTexture.height / 128f;
    float htimes = _widthSmall / _wdiv;
    float vtimes = _heightSmall / _hdiv;

    uvs[1] = new Vector2(0f, 0f);
    uvs[0] = new Vector2(htimes, 0f);
    uvs[5] = new Vector2(htimes / 2, vtimes);

    uvs[3] = new Vector2(0f, 0f);
    uvs[2] = new Vector2(htimes, 0f);
    uvs[4] = new Vector2(htimes / 2, vtimes);

    htimes = _widthBig / _wdiv;
    vtimes = _heightBig / _hdiv;
    var _dist45 = (boundaries[4] - boundaries[5]).magnitude;
    var tmp = ((_widthBig - _dist45) / 2) / _wdiv;
    uvs[8] = new Vector2(0f, 0f);
    uvs[7] = new Vector2(htimes, 0f);
    uvs[10] = new Vector2(tmp, vtimes);
    uvs[11] = new Vector2(htimes - tmp, vtimes);

    uvs[12] = new Vector2(0f, 0f);
    uvs[15] = new Vector2(htimes, 0f);
    uvs[17] = new Vector2(tmp, vtimes);
    uvs[16] = new Vector2(htimes - tmp, vtimes);

    mesh.uv = uvs;
  }
}

} // namespace Thesis