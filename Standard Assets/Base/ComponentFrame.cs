using UnityEngine;

namespace Thesis {
namespace Base {

public class ComponentFrame : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public Building parentBuilding
  {
    get { return parentFace.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentFrame (FaceComponent parent, string name, string materialName)
  {
    parentComponent = parent;

    this.name = name;
    this.materialName = materialName;

    boundaries = new Vector3[8];
    for (var i = 0; i < 4; ++i)
    {
      boundaries[i] = parentComponent.boundaries[i];
      // subtract a small amount to prevent overlaping triangles
      boundaries[i + 4] = boundaries[i] - (parentComponent.depth - 0.001f) * parentComponent.normal;
    }
  }

  /*************** METHODS ***************/

  public override void FindVertices ()
  {
    vertices = boundaries;
  }

  public override void FindTriangles ()
  {
    triangles = new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4
    };
  }

  public override void Draw ()
  {
    base.Draw();

    gameObject.transform.parent = parentBuilding.gameObject.transform;
  }
}

} // namespace Base
} // namespace Thesis