using UnityEngine;

namespace Thesis {

public class Roof : DrawableObject
{
  public readonly BuildingMesh parentMesh;

  public float width;

  public float height;

  public Roof (BuildingMesh parent)
  {
    parentMesh = parent;   
  }

  public override void Draw()
  {
    base.Draw();

    gameObject.transform.parent = parentMesh.parent.gameObject.transform;
    gameObject.transform.position = parentMesh.meshOrigin;
  }
}

} // namespace Thesis