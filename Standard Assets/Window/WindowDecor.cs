using UnityEngine;

namespace Thesis {

public class ComponentDecor : DrawableObject
{
  public readonly FaceComponent parentComponent;

  public float width;

  public Building allParent
  {
    get { return parentComponent.parentBuilding.parent; }
  }

  public ComponentDecor (FaceComponent parent)
  {
    parentComponent = parent;

    width = 0.3f;

    boundaries = new Vector3[12];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentComponent.boundaries[i] +
                      0.01f * parentComponent.parentFace.normal;

    var dist = width * parentComponent.parentFace.right;

    boundaries[4] = boundaries[0] + dist;
    boundaries[5] = boundaries[1] - dist;
    boundaries[6] = boundaries[2] - dist;
    boundaries[7] = boundaries[3] + dist;
    
    boundaries[8]  = boundaries[2] - 2 * dist;
    boundaries[9]  = boundaries[8] + 2 * width * Vector3.up;
    boundaries[11] = boundaries[3] + 2 * dist;
    boundaries[10] = boundaries[11] + 2 * width * Vector3.up;
  }

  public override void FindVertices()
  {
    vertices = new Vector3[boundaries.Length * 2];
    for (int i = 0; i < 2; ++i)
      System.Array.Copy(boundaries, 0, vertices, i * boundaries.Length, boundaries.Length);
  }

  public override void FindTriangles()
  {
    triangles = new int[18];
    int i = 0;

    // left side
    triangles[i++] = 1;
    triangles[i++] = 5;
    triangles[i++] = 6;

    triangles[i++] = 1;
    triangles[i++] = 6;
    triangles[i++] = 2;

    // right side
    triangles[i++] = 0;
    triangles[i++] = 3;
    triangles[i++] = 7;

    triangles[i++] = 0;
    triangles[i++] = 7;
    triangles[i++] = 4;

    // top
    triangles[i++] = 8;
    triangles[i++] = 9;
    triangles[i++] = 10;

    triangles[i++] = 8;
    triangles[i++] = 10;
    triangles[i++] = 11;
  }

  public override void Draw()
  {
    base.Draw();

    var uvs = new Vector2[mesh.vertices.Length];

    uvs[5] = new Vector2(  0f, 0f);
    uvs[1] = new Vector2(.25f, 0f);
    uvs[6] = new Vector2(  0f, 1f);
    uvs[2] = new Vector2(.25f, 1f);

    uvs[0] = new Vector2(.25f, 0f);
    uvs[4] = new Vector2( .5f, 0f);
    uvs[3] = new Vector2(.25f, 1f);
    uvs[7] = new Vector2( .5f, 1f);

    uvs[8] = new Vector2(.5f, 1f);
    uvs[11] = new Vector2(.5f, 0f);
    uvs[9] = new Vector2( 1f, 1f);
    uvs[10] = new Vector2( 1f, 0f);

    mesh.uv = uvs;

    gameObject.transform.parent = allParent.gameObject.transform;
    gameObject.transform.position = allParent.buildingMesh.meshOrigin;
  }
}

} // namespace Thesis