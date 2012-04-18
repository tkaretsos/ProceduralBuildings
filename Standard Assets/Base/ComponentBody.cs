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

  public Building parentBuilding
  {
    get { return parentComponent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentBody (FaceComponent parent, string name, string materialName)
  {
    parentComponent = parent;
    this.name = name;
    this.materialName = materialName;

    boundaries = new Vector3[parentComponent.boundaries.Length];
    for (var i = 0; i < parentComponent.boundaries.Length; ++i)
      boundaries[i] = parentComponent.boundaries[i] -
                      parentComponent.depth * parentComponent.normal;
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

    gameObject.transform.parent = parentBuilding.gameObject.transform;
  }
}

} // namespace Base
} // namespace Thesis