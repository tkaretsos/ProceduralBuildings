using UnityEngine;

namespace Thesis {

public sealed class NeoclassicalDoor : Base.Door
{
  public NeoclassicalDoor(Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
  : base (parent, position)
  {
    height = ((Neoclassical) parentBuilding).doorHeight;

    var new_dr = dr + 0.4f * parentFace.right;
    var new_dl = dl - 0.4f * parentFace.right;

    boundaries = new Vector3[4];
    boundaries[0] = new_dr;
    boundaries[1] = new_dl;
    boundaries[2] = new Vector3(new_dl.x, new_dl.y + height, new_dl.z);
    boundaries[3] = new Vector3(new_dr.x, new_dr.y + height, new_dr.z);

    frame = new Base.DoorFrame(this);
    body = new Base.DoorBody(this);
  }

  public override void Draw ()
  {
    base.Draw();

    frame.FindVertices();
    frame.FindTriangles();
    frame.Draw();

    body.FindVertices();
    body.FindTriangles();
    body.Draw();
  }
}

} // namespace Thesis