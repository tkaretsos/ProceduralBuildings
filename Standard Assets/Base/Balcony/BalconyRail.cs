using UnityEngine;

namespace Thesis {
namespace Base {

public class BalconyRail : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly BalconyFloor parentFloor;

  public NeoBuildingMesh parentBuilding
  {
    get { return parentFloor.parentBuilding; }
  }

  public Face parentFace
  {
    get { return parentFloor.parentBalcony.parentFace; }
  }

  public BalconyRail (BalconyFloor parent)
    : base("balcony_rail")
  {
    parentFloor = parent;
    
    var height = 1f;
    boundaries = new Vector3[8];

    boundaries[0] = parentFloor.boundaries[4];
    boundaries[1] = parentFloor.boundaries[5];
    boundaries[2] = boundaries[1] - parentFloor.depth * parentFace.normal;
    boundaries[3] = boundaries[0] - parentFloor.depth * parentFace.normal;

    for (var i = 0; i < 4; ++i)
      boundaries[i + 4] = boundaries[i] + height * Vector3.up;

    FindMeshOrigin(boundaries[0],
                   boundaries[6],
                   boundaries[2],
                   boundaries[4]);

    for (var i = 0; i < boundaries.Length; ++i)
      boundaries[i] -= meshOrigin;
  }

  public override void FindVertices ()
  {
    // make length * 4 for inside sides of rails
    vertices = new Vector3[boundaries.Length * 2];
    for (int i = 0; i < 2; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles ()
  {
    triangles = new int[18];

    // from outside
    
    // front
    triangles[0]  = 0;
    triangles[1]  = 1;
    triangles[2]  = 4;

    triangles[3]  = 1;
    triangles[4]  = 5;
    triangles[5]  = 4;

    //// left
    //triangles[6]  = 1;
    //triangles[7]  = 2;
    //triangles[8]  = 5;

    //triangles[9]  = 2;
    //triangles[10] = 6;
    //triangles[11] = 5;

    //// right
    //triangles[12] = 0;
    //triangles[13] = 4;
    //triangles[14] = 3;

    //triangles[15] = 3;
    //triangles[16] = 4;
    //triangles[17] = 7;

    // left
    triangles[6]  = 9;    // 1 + 8
    triangles[7]  = 10;   // 2 + 8
    triangles[8]  = 13;   // 5 + 8

    triangles[9]  = 10;   // 2 + 8
    triangles[10] = 14;   // 6 + 8
    triangles[11] = 13;   // 5 + 8

    // right
    triangles[12] = 8;    // 0 + 8
    triangles[13] = 12;   // 4 + 8
    triangles[14] = 11;   // 3 + 8

    triangles[15] = 11;   // 3 + 8
    triangles[16] = 12;   // 4 + 8
    triangles[17] = 15;   // 7 + 8

    // from inside are the previous tris in reverse order

    //var index = 18;
    //for (var i = 17; i >= 0; --i)
    //  triangles[index++] = triangles[i] + 16;
  }

  public override void Draw ()
  {
    base.Draw();
    
    var uvs = new Vector2[mesh.vertices.Length];
    uvs[2 + 8] = new Vector2(.5f, 0f);
    uvs[6 + 8] = new Vector2(.5f, 1f);
    uvs[1 + 8] = new Vector2(1f, 0f);
    uvs[5 + 8] = new Vector2(1f, 1f);
    
    uvs[1] = new Vector2(0f, 0f);
    uvs[5] = new Vector2(0f, 1f);
    uvs[0] = new Vector2(1f, 0f);
    uvs[4] = new Vector2(1f, 1f);
    
    uvs[0 + 8] = new Vector2(1f, 0f);
    uvs[4 + 8] = new Vector2(1f, 1f);
    uvs[3 + 8] = new Vector2(.5f, 0f);
    uvs[7 + 8] = new Vector2(.5f, 1f);

    mesh.uv = uvs;

    gameObject.transform.position = meshOrigin + parentBuilding.meshOrigin + parentFloor.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }

  private Texture2D CreateTexture ()
  {
    var tex = new Texture2D(256, 128);
    int y = -1;
    int x = -1;

    var thickness = 5;

    do
    {
      x = 0;
      do
      {
        tex.SetPixel(x, y, Color.black);
        //Debug.Log(y);
      } while (x++ < tex.width);
    } while (y++ < thickness - 1);

    do
    {
      x = 0;
      do
      {
        if ((x & y) == 0)
          tex.SetPixel(x, y, Color.black);
        else
          tex.SetPixel(x, y, Color.clear);
      } while (x++ < tex.width);
    } while (y++ < tex.height - thickness);

    do
    {
      x = 0;
      do
      {
        tex.SetPixel(x, y, Color.black);
        Debug.Log(y);
      } while (x++ < tex.width);
    } while (y++ < tex.height);

    tex.Apply();

    return tex;
  }
}

} // namespace Base
} // namespace Thesis