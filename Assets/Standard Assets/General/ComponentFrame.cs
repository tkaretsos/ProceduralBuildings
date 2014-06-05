using UnityEngine;

namespace Thesis {

public class ComponentFrame : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public BuildingMesh parentBuilding
  {
    get { return parentFace.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentFrame (FaceComponent parent)
  {
    parentComponent = parent;

    boundaries = new Vector3[8];
    for (var i = 0; i < 4; ++i)
    {
      boundaries[i] = parentComponent.boundaries[i];
      // subtract a small amount to prevent overlaping triangles
      boundaries[i + 4] = boundaries[i] - parentComponent.depth * parentComponent.normal;
    }

    FindMeshOrigin(boundaries[0],
                   boundaries[6],
                   boundaries[2],
                   boundaries[4]);

    for (var i = 0; i < boundaries.Length; ++i)
      boundaries[i] -= meshOrigin;
  }

  /*************** METHODS ***************/

  public override void FindVertices ()
  {
    vertices = new Vector3[boundaries.Length << 1];
    for (var i = 0; i < boundaries.Length; ++i)
      vertices[i] = vertices[i + 8] = boundaries[i];
  }

  public override void FindTriangles ()
  {
    triangles = new int[3 * boundaries.Length];
    var i = 0;

    // right
    triangles[i++] =  0;
    triangles[i++] =  4;
    triangles[i++] =  7;

    triangles[i++] =  0;
    triangles[i++] =  7;
    triangles[i++] =  3;

    // left
    triangles[i++] =  1;
    triangles[i++] =  2;
    triangles[i++] =  6;

    triangles[i++] =  1;
    triangles[i++] =  6;
    triangles[i++] =  5;

    // bottom
    triangles[i++] =  8;  // 0 + 8;
    triangles[i++] =  9;  // 1 + 8;
    triangles[i++] = 13;  // 5 + 8;

    triangles[i++] =  8;  // 0 + 8;
    triangles[i++] = 13;  // 5 + 8;
    triangles[i++] = 12;  // 4 + 8;

    // top
    triangles[i++] = 10;  // 2 + 8;
    triangles[i++] = 11;  // 3 + 8;
    triangles[i++] = 14;  // 6 + 8;

    triangles[i++] = 14;  // 6 + 8;
    triangles[i++] = 11;  // 3 + 8;
    triangles[i++] = 15;  // 7 + 8;
  }

  public override void Draw ()
  {
    base.Draw();

    gameObject.transform.position = meshOrigin + parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }
}

} // namespace Thesis