using UnityEngine;

namespace Thesis {

public class RoofBase : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly BuildingMesh parentMesh;

  public float height;

  /*************** CONSTRUCTORS ***************/

  public RoofBase (BuildingMesh parent)
  {
    parentMesh = parent;
    height = 0.3f;

    boundaries = new Vector3[8];

    for (int i = 0; i < 4; ++i)
    {
      boundaries[i] = parentMesh.boundaries[i + 4];
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

    gameObject.transform.parent = parentMesh.parent.gameObject.transform;
    gameObject.transform.position = parentMesh.meshOrigin;
  }
}

} // namespace Thesis