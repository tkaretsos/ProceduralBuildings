using UnityEngine;

namespace Thesis {

public class Roof : DrawableObject
{
  public readonly BuildingMesh parentMesh;

  public RoofDecoration decor;

  public float width;

  public float height;

  public Roof (BuildingMesh parent)
  {
    parentMesh = parent;
    decor = null;
  }

  public override void Draw()
  {
    base.Draw();

    gameObject.transform.parent = parentMesh.parent.gameObject.transform;
    gameObject.transform.position = parentMesh.meshOrigin;

    if (decor != null)
    {
      decor.FindVertices();
      decor.FindTriangles();
      decor.Draw();
    }
  }

  public override void Destroy()
  {
    base.Destroy();

    decor.Destroy();
  }
}

} // namespace Thesis