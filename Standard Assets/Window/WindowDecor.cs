using UnityEngine;

namespace Thesis {

public class WindowDecor : DrawableObject
{
  public readonly Window parentWindow;

  public float width;

  public float depth;

  public Building allParent
  {
    get { return parentWindow.parentBuilding.parent; }
  }

  public WindowDecor (Window parent)
  {
    parentWindow = parent;

    width = 0.2f;
    depth = 0.02f;

    boundaries = new Vector3[16];

    System.Array.Copy(parentWindow.boundaries, 0, boundaries, 0, 4);

    boundaries[4] = boundaries[0] +
                    width * parentWindow.parentFace.right - width * Vector3.up;
    boundaries[5] = boundaries[1] -
                    width * parentWindow.parentFace.right - width * Vector3.up;
    boundaries[6] = boundaries[2] -
                    width * parentWindow.parentFace.right + width * Vector3.up;
    boundaries[7] = boundaries[3] +
                    width * parentWindow.parentFace.right + width * Vector3.up;

    for (int i = 0; i < 8; ++i)
      boundaries[i + 8] = boundaries[i] + depth * parentWindow.parentFace.normal;
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 4];
    for (int i = 0; i < 4; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[72];
    int i = 0;

    // order is bottom -> left -> up -> right
    // inner -> outter

    // bottom in
    triangles[i++] = 0;
    triangles[i++] = 8;
    triangles[i++] = 9;

    triangles[i++] = 0;
    triangles[i++] = 9;
    triangles[i++] = 1;

    // bottom front (+32)
    triangles[i++] = 40;
    triangles[i++] = 44;
    triangles[i++] = 45;

    triangles[i++] = 40;
    triangles[i++] = 45;
    triangles[i++] = 41;

    // bottom out
    triangles[i++] = 4;
    triangles[i++] = 5;
    triangles[i++] = 13;

    triangles[i++] = 4;
    triangles[i++] = 13;
    triangles[i++] = 12;

    // left in (+16)
    triangles[i++] = 17;
    triangles[i++] = 25;
    triangles[i++] = 26;

    triangles[i++] = 17;
    triangles[i++] = 26;
    triangles[i++] = 18;

    // left front (+48)
    triangles[i++] = 57;
    triangles[i++] = 61;
    triangles[i++] = 62;

    triangles[i++] = 57;
    triangles[i++] = 62;
    triangles[i++] = 58;

    // left out (+16)
    triangles[i++] = 21;
    triangles[i++] = 22;
    triangles[i++] = 30;

    triangles[i++] = 21;
    triangles[i++] = 30;
    triangles[i++] = 29;

    // up in
    triangles[i++] = 2;
    triangles[i++] = 10;
    triangles[i++] = 11;

    triangles[i++] = 2;
    triangles[i++] = 11;
    triangles[i++] = 3;

    // up front (+32)
    triangles[i++] = 42;
    triangles[i++] = 46;
    triangles[i++] = 47;

    triangles[i++] = 42;
    triangles[i++] = 47;
    triangles[i++] = 43;

    // up out
    triangles[i++] = 6;
    triangles[i++] = 7;
    triangles[i++] = 15;

    triangles[i++] = 6;
    triangles[i++] = 15;
    triangles[i++] = 14;

    // right in (+16)
    triangles[i++] = 19;
    triangles[i++] = 27;
    triangles[i++] = 24;

    triangles[i++] = 19;
    triangles[i++] = 24;
    triangles[i++] = 16;

    // right front (+48)
    triangles[i++] = 59;
    triangles[i++] = 63;
    triangles[i++] = 60;

    triangles[i++] = 59;
    triangles[i++] = 60;
    triangles[i++] = 56;

    // right out (+16)
    triangles[i++] = 23;
    triangles[i++] = 20;
    triangles[i++] = 28;

    triangles[i++] = 23;
    triangles[i++] = 28;
    triangles[i++] = 31;
  }

  public override void Draw()
  {
    base.Draw();

    gameObject.transform.parent = allParent.gameObject.transform;
    gameObject.transform.position = allParent.buildingMesh.meshOrigin;
  }
}

} // namespace Thesis