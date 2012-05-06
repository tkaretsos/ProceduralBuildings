using UnityEngine;

namespace Thesis {
namespace Base {

public class Shutter : DrawableObject
{
  public readonly Window parentWindow;

  public readonly ShutterSide side;

  public BuildingMesh parentBuilding
  {
    get { return parentWindow.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public Shutter (Window parent, ShutterSide side)
  {
    parentWindow = parent;
    this.side = side;
    boundaries = new Vector3[8];

    if (side == ShutterSide.Right)
    {
      boundaries[0] = Vector3.zero;
      boundaries[1] = - parentWindow.parentFace.right * (parentWindow.width / 2);
      boundaries[2] = boundaries[1] + Vector3.up * parentWindow.height;
      boundaries[3] = Vector3.up * parentWindow.height;

      meshOrigin = parentWindow.boundaries[0];
    }
    else
    {
      boundaries[0] = parentWindow.parentFace.right * (parentWindow.width / 2);
      boundaries[1] = Vector3.zero;
      boundaries[2] = Vector3.up * parentWindow.height;
      boundaries[3] = boundaries[0] + boundaries[2];

      meshOrigin = parentWindow.boundaries[1];
    }

    for (var i = 0; i < 4; ++i)
        boundaries[i + 4] = boundaries[i] - parentWindow.normal * parentWindow.depth / 2;
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

    // bottom
    triangles[i++] =  0;
    triangles[i++] =  4;
    triangles[i++] =  1;

    triangles[i++] =  1;
    triangles[i++] =  4;
    triangles[i++] =  5;

    // top
    triangles[i++] =  2;
    triangles[i++] =  6;
    triangles[i++] =  7;

    triangles[i++] =  2;
    triangles[i++] =  7;
    triangles[i++] =  3;

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

    // front  (+16)
    triangles[i++] = 16;
    triangles[i++] = 17;
    triangles[i++] = 19;

    triangles[i++] = 17;
    triangles[i++] = 18;
    triangles[i++] = 19;

    // back   (+16)
    triangles[i++] = 20;
    triangles[i++] = 23;
    triangles[i++] = 21;

    triangles[i++] = 21;
    triangles[i++] = 23;
    triangles[i++] = 22;
  }

  public override void Draw()
  {
    base.Draw();

    gameObject.transform.position = meshOrigin + parentWindow.meshOrigin +
                                    parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }
}

} // Base
} // Thesis