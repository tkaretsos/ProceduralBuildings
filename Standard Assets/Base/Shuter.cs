using UnityEngine;

namespace Thesis {
namespace Base {

public class Shutter : DrawableObject
{
  public readonly FaceComponent parent;

  public readonly ShutterSide side;

  public BuildingMesh parentBuilding
  {
    get { return parent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public Shutter (FaceComponent parent, ShutterSide side)
  {
    this.parent = parent;
    this.side = side;
    boundaries = new Vector3[8];

    if (side == ShutterSide.Right)
    {
      boundaries[0] = Vector3.zero;
      boundaries[1] = - parent.parentFace.right * (parent.width / 2);
      boundaries[2] = boundaries[1] + Vector3.up * parent.height;
      boundaries[3] = Vector3.up * parent.height;

      meshOrigin = parent.boundaries[0];
    }
    else
    {
      boundaries[0] = parent.parentFace.right * (parent.width / 2);
      boundaries[1] = Vector3.zero;
      boundaries[2] = Vector3.up * parent.height;
      boundaries[3] = boundaries[0] + boundaries[2];

      meshOrigin = parent.boundaries[1];
    }

    for (var i = 0; i < 4; ++i)
        boundaries[i + 4] = boundaries[i] - parent.normal * parent.depth / 2;
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
    int i = 0;

    // front
    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 3;

    triangles[i++] = 1;
    triangles[i++] = 2;
    triangles[i++] = 3;

    // back
    triangles[i++] = 4;
    triangles[i++] = 7;
    triangles[i++] = 5;

    triangles[i++] = 5;
    triangles[i++] = 7;
    triangles[i++] = 6;

    // right  (+8)
    triangles[i++] =  8;
    triangles[i++] = 11;
    triangles[i++] = 15;

    triangles[i++] =  8;
    triangles[i++] = 15;
    triangles[i++] = 12;

    // left   (+8)
    triangles[i++] =  9;
    triangles[i++] = 13;
    triangles[i++] = 10;

    triangles[i++] = 13;
    triangles[i++] = 14;
    triangles[i++] = 10;

    // bottom   (+16)
    triangles[i++] =  16;
    triangles[i++] =  20;
    triangles[i++] =  17;

    triangles[i++] =  17;
    triangles[i++] =  20;
    triangles[i++] =  21;

    // top      (+16)
    triangles[i++] =  18;
    triangles[i++] =  22;
    triangles[i++] =  23;

    triangles[i++] =  18;
    triangles[i++] =  23;
    triangles[i++] =  19;
  }

  public override void Draw()
  {
    base.Draw();

    if (side == ShutterSide.Right)
      gameObject.transform.RotateAround(meshOrigin, Vector3.up, -110);
    else
      gameObject.transform.RotateAround(meshOrigin, Vector3.up,  110);

    gameObject.transform.position = meshOrigin + parent.meshOrigin +
                                    parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }
}

} // Base
} // Thesis