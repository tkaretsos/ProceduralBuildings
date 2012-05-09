using UnityEngine;

namespace Thesis {
namespace Base {

public class FaceComponent : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly Face parentFace;

  public readonly ComponentCoordinate position;

  public float height;

  public float depth;

  public float width;

  public ComponentFrame frame;

  public ComponentBody body;

  public BuildingMesh parentBuilding
  {
    get { return parentFace.parentBuilding; }
  }

  public Vector3 normal
  {
    get { return parentFace.normal; }
  }

  /*************** CONSTRUCTORS ***************/

  public FaceComponent (Face parent, ComponentCoordinate position)
  {
    parentFace = parent;
    this.position = position;
  }

  public override void Destroy()
  {
    base.Destroy();

    frame.Destroy();
    body.Destroy();
  }
}

} // namespace Base
} // namespace Thesis