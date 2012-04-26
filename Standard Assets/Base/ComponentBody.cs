using UnityEngine;

namespace Thesis {
namespace Base {

public class ComponentBody : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public BuildingMesh parentBuilding
  {
    get { return parentComponent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentBody (FaceComponent parent)
  {
    parentComponent = parent;

    boundaries = new Vector3[4];
    for (var i = 0; i < parentComponent.boundaries.Length; ++i)
      boundaries[i] = parentComponent.boundaries[i] -
                      parentComponent.depth * parentComponent.normal;

    FindMeshOrigin(boundaries[0],
                   boundaries[2],
                   boundaries[1],
                   boundaries[3]);

    for (var i = 0; i < boundaries.Length; ++i)
      boundaries[i] -= meshOrigin;
  }

  /*************** METHODS ***************/

  public override void FindVertices ()
  {
    vertices = boundaries;
  }

  public override void FindTriangles ()
  {
    triangles = new int[] {
      0, 1, 3,
      1, 2, 3
    };
  }

  public override void Draw ()
  {
    base.Draw();

    gameObject.transform.position = meshOrigin + parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }
}

} // namespace Base
} // namespace Thesis