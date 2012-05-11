using UnityEngine;

namespace Thesis {

public class Roof : DrawableObject
{
  public readonly BuildingMesh parentMesh;

  public readonly RoofType type;

  public float width;

  public float height;

  public Roof (BuildingMesh parent, RoofType roofType)
  {
    parentMesh = parent;
    type = roofType;
    width = 0.4f;
    height = 0.4f;

    boundaries = new UnityEngine.Vector3[8];

    for (int i = 0; i < 4; ++i)
    {
      boundaries[i] = parentMesh.boundaries[i + 4] +
                      width * parentMesh.faces[i].normal +
                      width * parentMesh.faces[(i + 3) % 4].normal;

      boundaries[i + 4] = boundaries[i] + height * Vector3.up;
    }    
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

    gameObject.transform.parent = parentMesh.parent.gameObject.transform;
    gameObject.transform.position = parentMesh.meshOrigin;
  }
}

} // namespace Thesis