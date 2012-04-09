using UnityEngine;

public sealed class NeoclassicalDoor : Base.Door
{
  public NeoclassicalDoor(Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
  : base (parent, position)
  {
    height = ((Neoclassical) parentBuilding).doorHeight;

    var new_dr = dr + 0.4f * parentFace.right;
    var new_dl = dl - 0.4f * parentFace.right;

    boundaries.Add(new_dr);
    boundaries.Add(new_dl);
    boundaries.Add(new Vector3(new_dl.x, new_dl.y + height, new_dl.z));
    boundaries.Add(new Vector3(new_dr.x, new_dr.y + height, new_dr.z));

    doorFrame = new Base.DoorFrame(this);
    doorBody = new Base.DoorBody(this);
  }

  public override void Draw ()
  {
    base.Draw();

    doorFrame.Draw();
    doorBody.Draw();
  }
}
