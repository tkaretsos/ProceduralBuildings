using UnityEngine;

namespace Thesis {

public class RoofDecoration : DrawableObject
{
  public readonly Roof parentRoof;

  public float height;

  public RoofDecoration (Roof parent)
  {
    parentRoof = parent;
    height = 0.3f;

    boundaries = new Vector3[8];

    if (parentRoof.boundaries.Length < 8)
      for (int i = 0; i < 4; ++i)
      {
        boundaries[i] = parentRoof.boundaries[i];
        boundaries[i + 4] = boundaries[i] + height * Vector3.up;
      }
    else if (parentRoof.boundaries.Length == 8)
      for (int i = 0; i < 4; ++i)
      {
        boundaries[i] = parentRoof.boundaries[i + 4];
        boundaries[i + 4] = boundaries[i] + height * Vector3.up;
      }
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 2];
    for (int i = 0; i < 2; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[24];
    int i = 0;

    // front
    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 5;

    triangles[i++] = 0;
    triangles[i++] = 5;
    triangles[i++] = 4;

    // back
    triangles[i++] = 3;
    triangles[i++] = 7;
    triangles[i++] = 6;

    triangles[i++] = 3;
    triangles[i++] = 6;
    triangles[i++] = 2;

    // left (+8)
    triangles[i++] = 9;
    triangles[i++] = 10;
    triangles[i++] = 14;

    triangles[i++] = 9;
    triangles[i++] = 14;
    triangles[i++] = 13;

    // right (+8)
    triangles[i++] = 8;
    triangles[i++] = 12;
    triangles[i++] = 15;

    triangles[i++] = 8;
    triangles[i++] = 15;
    triangles[i++] = 11;
  }

  public override void Draw()
  {
    base.Draw();

    var uvs = new Vector2[mesh.vertices.Length];

    float _wdiv = material.mainTexture.width / 128f;
    int times = Mathf.CeilToInt((boundaries[0] - boundaries[1]).magnitude / _wdiv);

    uvs[1] = new Vector2(0f, 0f);
    uvs[0] = new Vector2(times, 0f);
    uvs[5] = new Vector2(0f, 1f);
    uvs[4] = new Vector2(times, 1f);

    uvs[3] = new Vector2(0f, 0f);
    uvs[2] = new Vector2(times, 0f);
    uvs[7] = new Vector2(0f, 1f);
    uvs[6] = new Vector2(times, 1f);

    times = Mathf.CeilToInt((boundaries[2] - boundaries[1]).magnitude / _wdiv);

    uvs[10] = new Vector2(0f, 0f);
    uvs[9]  = new Vector2(times, 0f);
    uvs[14] = new Vector2(0f, 1f);
    uvs[13] = new Vector2(times, 1f);

    uvs[8]  = new Vector2(0f, 0f);
    uvs[11] = new Vector2(times, 0f);
    uvs[12] = new Vector2(0f, 1f);
    uvs[15] = new Vector2(times, 1f);

    mesh.uv = uvs;

    gameObject.transform.parent = parentRoof.parentMesh.parent.gameObject.transform;
    gameObject.transform.position = parentRoof.parentMesh.meshOrigin;
  }
}

} // namespace Thesis