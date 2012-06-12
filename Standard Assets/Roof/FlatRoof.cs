using UnityEngine;

namespace Thesis {

public class FlatRoof : Roof
{
  public FlatRoof (BuildingMesh parent)
    : base(parent)
  {
    width = 0.1f;
    height = 0.05f;

    boundaries = new Vector3[8];

    for (int i = 0; i < 4; ++i)
    {
      boundaries[i] = parentMesh.roofBase.boundaries[i + 4] +
                      width * parentMesh.faces[i].normal +
                      width * parentMesh.faces[(i + 3) % 4].normal;

      boundaries[i + 4] = boundaries[i] + height * Vector3.up;
    }

    decor = new RoofDecoration(this);
    if (parentMesh.parent.roofDecorMaterial == null)
    {
      var list = MaterialManager.Instance.GetCollection("mat_roof_decor");
      decor.material = list[Random.Range(0, list.Count)];
    }
    else
      decor.material = parentMesh.parent.roofDecorMaterial;
    parentMesh.parent.AddCombinable(decor.material.name, decor);
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 3];
    for (int i = 0; i < 3; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[36];
    var i = 0;

    // front
    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 5;

    triangles[i++] = 0;
    triangles[i++] = 5;
    triangles[i++] = 4;

    // back
    triangles[i++] = 2;
    triangles[i++] = 3;
    triangles[i++] = 6;

    triangles[i++] = 3;
    triangles[i++] = 7;
    triangles[i++] = 6;

    // right (+8)
    triangles[i++] = 11;
    triangles[i++] = 8;
    triangles[i++] = 15;

    triangles[i++] = 8;
    triangles[i++] = 12;
    triangles[i++] = 15;

    // left (+8)
    triangles[i++] = 9;
    triangles[i++] = 10;
    triangles[i++] = 14;

    triangles[i++] = 9;
    triangles[i++] = 14;
    triangles[i++] = 13;

    // bottom (+16)
    triangles[i++] = 16;
    triangles[i++] = 18;
    triangles[i++] = 17;

    triangles[i++] = 16;
    triangles[i++] = 19;
    triangles[i++] = 18;

    // top (+16)
    triangles[i++] = 20;
    triangles[i++] = 21;
    triangles[i++] = 22;

    triangles[i++] = 20;
    triangles[i++] = 22;
    triangles[i++] = 23;
  }

  public override void Draw()
  {
    base.Draw();

    float _wdiv = material.mainTexture.width / 128f;
    float _hdiv = material.mainTexture.height / 128f;
    float htimes = (boundaries[0] - boundaries[1]).magnitude / _wdiv;
    float vtimes = (boundaries[2] - boundaries[1]).magnitude / _hdiv;

    var uvs = new Vector2[mesh.vertices.Length];

    uvs[21] = new Vector2(0f, 0f);
    uvs[20] = new Vector2(htimes, 0f);
    uvs[22] = new Vector2(0f, vtimes);
    uvs[23] = new Vector2(htimes, vtimes);

    mesh.uv = uvs;
  }
}

} // namespace Thesis