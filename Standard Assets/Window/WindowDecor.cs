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

  private readonly bool _parentIsWindow;
  private readonly bool _isSimple;

  public ComponentDecor (FaceComponent parent, bool isSimple = false)
  {
    parentComponent = parent;
    _isSimple = isSimple;
    _parentIsWindow = parent.GetType().Equals(typeof(Window)) ? true : false;

    if (_isSimple)
      initSimpleDecor();
    else
      initDecor();
  }

  private void initDecor ()
  {
    boundaries = new Vector3[16];

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentComponent.boundaries[i] +
                      0.01f * parentComponent.parentFace.normal;

    width = 0.3f;
    var dist = width * parentComponent.parentFace.right;

    boundaries[4] = boundaries[0] + dist;
    boundaries[5] = boundaries[1] - dist;
    boundaries[6] = boundaries[2] - dist;
    boundaries[7] = boundaries[3] + dist;

    boundaries[8] = boundaries[2] - 2 * dist;
    boundaries[9] = boundaries[8] + 2 * width * Vector3.up;
    boundaries[11] = boundaries[3] + 2 * dist;
    boundaries[10] = boundaries[11] + 2 * width * Vector3.up;

    boundaries[12] = boundaries[5] - 4 * width * Vector3.up;
    boundaries[13] = boundaries[5];
    boundaries[14] = boundaries[4];
    boundaries[15] = boundaries[4] - 4 * width * Vector3.up;
  }

  private void initSimpleDecor ()
  {
    boundaries = new Vector3[8];
    width = 0.15f;

    for (int i = 0; i < 4; ++i)
      boundaries[i] = parentComponent.boundaries[i] +
                      0.01f * parentComponent.parentFace.normal;

    var right = parentComponent.parentFace.right;

    boundaries[4] = boundaries[0] + width * right - width * Vector3.up;
    boundaries[5] = boundaries[1] - width * right - width * Vector3.up;
    boundaries[6] = boundaries[2] - width * right + width * Vector3.up;
    boundaries[7] = boundaries[3] + width * right + width * Vector3.up;
  }

  public override void FindVertices()
  {
    vertices = boundaries;
  }

  public override void FindTriangles()
  {
    triangles = new int[_parentIsWindow ? 24 : 18];
    if (_isSimple)
      findTrisSimple();
    else
      findTris();
  }

  private void findTris()
  {
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

    // bottom
    if (_parentIsWindow)
    {
      triangles[i++] = 12;
      triangles[i++] = 13;
      triangles[i++] = 14;

      triangles[i++] = 12;
      triangles[i++] = 14;
      triangles[i++] = 15;
    }
  }

  private void findTrisSimple()
  {
    int i = 0;

    // left
    triangles[i++] = 1;
    triangles[i++] = 5;
    triangles[i++] = 6;

    triangles[i++] = 1;
    triangles[i++] = 6;
    triangles[i++] = 2;

    // top
    triangles[i++] = 2;
    triangles[i++] = 6;
    triangles[i++] = 7;

    triangles[i++] = 2;
    triangles[i++] = 7;
    triangles[i++] = 3;

    // right
    triangles[i++] = 4;
    triangles[i++] = 0;
    triangles[i++] = 3;

    triangles[i++] = 4;
    triangles[i++] = 3;
    triangles[i++] = 7;

    if (_parentIsWindow)
    {
      // bottom
      triangles[i++] = 4;
      triangles[i++] = 5;
      triangles[i++] = 1;

      triangles[i++] = 4;
      triangles[i++] = 1;
      triangles[i++] = 0;
    }
  }

  public override void Draw()
  {
    base.Draw();

    if (_isSimple)
      findUvsSimple();
    else
      findUvs();

    gameObject.transform.parent = allParent.gameObject.transform;
    gameObject.transform.position = allParent.buildingMesh.meshOrigin;
  }

  private void findUvs()
  {
    var uvs = new Vector2[mesh.vertices.Length];

    // left
    uvs[5] = new Vector2(0f, 0f);
    uvs[1] = new Vector2(.125f, 0f);
    uvs[6] = new Vector2(0f, 1f);
    uvs[2] = new Vector2(.125f, 1f);

    // right
    uvs[0] = new Vector2(.125f, 0f);
    uvs[4] = new Vector2(.25f, 0f);
    uvs[3] = new Vector2(.125f, 1f);
    uvs[7] = new Vector2(.25f, 1f);

    // top
    uvs[8] = new Vector2(.25f, 1f);
    uvs[11] = new Vector2(.25f, 0f);
    uvs[9] = new Vector2(.5f, 1f);
    uvs[10] = new Vector2(.5f, 0f);

    // bottom
    if (_parentIsWindow)
    {
      uvs[15] = new Vector2(.5f, 0f);
      uvs[14] = new Vector2(1f, 0f);
      uvs[12] = new Vector2(.5f, 1f);
      uvs[13] = new Vector2(1f, 1f);
    }

    mesh.uv = uvs;
  }

  private void findUvsSimple()
  {
    var uvs = new Vector2[mesh.vertices.Length];

    // bottom
    uvs[4] = new Vector2(0f, 0f);
    uvs[0] = new Vector2(1f, 0f);
    uvs[5] = new Vector2(0f, 1f);
    uvs[1] = new Vector2(1f, 1f);

    // left
    uvs[5] = new Vector2(0f, 0f);
    uvs[1] = new Vector2(1f, 0f);
    uvs[6] = new Vector2(0f, 1f);
    uvs[2] = new Vector2(1f, 1f);

    // top
    uvs[6] = new Vector2(0f, 0f);
    uvs[2] = new Vector2(1f, 0f);
    uvs[7] = new Vector2(0f, 1f);
    uvs[3] = new Vector2(1f, 1f);

    // right
    uvs[7] = new Vector2(0f, 0f);
    uvs[3] = new Vector2(1f, 0f);
    uvs[4] = new Vector2(0f, 1f);
    uvs[0] = new Vector2(1f, 1f);

    mesh.uv = uvs;
  }
}

} // namespace Thesis