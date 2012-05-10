using UnityEngine;

namespace Thesis {
namespace Base {

public class Shutter : DrawableObject
{
  public readonly FaceComponent parent;

  public readonly ShutterSide side;

  static private int angles;

  public BuildingMesh parentBuilding
  {
    get { return parent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public Shutter (FaceComponent parent, ShutterSide side)
  {
    this.parent = parent;
    this.side = side;
    angles = 0;
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
    AssignUVs();

    if (side == ShutterSide.Right)
      gameObject.transform.RotateAround(meshOrigin, Vector3.up, -angles);
    else
      gameObject.transform.RotateAround(meshOrigin, Vector3.up,  angles);

    gameObject.transform.position = meshOrigin + parent.meshOrigin +
                                    parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }

  private void AssignUVs ()
  {
    var uvs = new Vector2[mesh.vertices.Length];

    if (parent.GetType().IsSubclassOf(typeof(Window)) &&
        side == ShutterSide.Left)
    {
      uvs[0] = new Vector2(.5f, .5f);
      uvs[1] = new Vector2(  0, .5f);
      uvs[2] = new Vector2(  0,   1);
      uvs[3] = new Vector2(.5f,   1);
      uvs[4] = new Vector2(.5f, .5f);
      uvs[5] = new Vector2(  0, .5f);
      uvs[6] = new Vector2(  0,   1);
      uvs[7] = new Vector2(.5f,   1);
    }
    else if (parent.GetType().IsSubclassOf(typeof(Window)) &&
             side == ShutterSide.Right)
    {
      uvs[0] = new Vector2(  0, .5f);
      uvs[1] = new Vector2(.5f, .5f);
      uvs[2] = new Vector2(.5f,   1);
      uvs[3] = new Vector2(  0,   1);
      uvs[4] = new Vector2(  0, .5f);
      uvs[5] = new Vector2(.5f, .5f);
      uvs[6] = new Vector2(.5f,   1);
      uvs[7] = new Vector2(  0,   1);
    }
    else if (parent.GetType().IsSubclassOf(typeof(Balcony)) &&
             side == ShutterSide.Left)
    {
      uvs[0] = new Vector2(  1, 0);
      uvs[1] = new Vector2(.5f, 0);
      uvs[2] = new Vector2(.5f, 1);
      uvs[3] = new Vector2(  1, 1);
      uvs[4] = new Vector2(  1, 0);
      uvs[5] = new Vector2(.5f, 0);
      uvs[6] = new Vector2(.5f, 1);
      uvs[7] = new Vector2(  1, 1);
    }
    else
    {
      uvs[0] = new Vector2(.5f, 0);
      uvs[1] = new Vector2(  1, 0);
      uvs[2] = new Vector2(  1, 1);
      uvs[3] = new Vector2(.5f, 1);
      uvs[4] = new Vector2(.5f, 0);
      uvs[5] = new Vector2(  1, 0);
      uvs[6] = new Vector2(  1, 1);
      uvs[7] = new Vector2(.5f, 1);
    }

    mesh.uv = uvs;
  }

  public static void SetAngles ()
  {
    switch (Util.RollDice(new float[] {0.33f, 0.33f, 0.34f}))
    {
      case 1:
        angles = 150;
        break;

      case 2:
        angles = Random.Range(130, 150);
        break;

      case 3:
        angles = 0;
        break;
    }
  }
}

} // Base
} // Thesis