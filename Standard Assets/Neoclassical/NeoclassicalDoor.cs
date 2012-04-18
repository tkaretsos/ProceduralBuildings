using UnityEngine;

namespace Thesis {

public sealed class NeoclassicalDoor : Base.Door
{
  public NeoclassicalDoor(Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
  : base (parent, position)
  {
    height = ((Neoclassical) parentBuilding).doorHeight;

    dr += 0.4f * parentFace.right;
    dl -= 0.4f * parentFace.right;

    var ul = new Vector3(dl.x, dl.y + height, dl.z);
    var ur = new Vector3(dr.x, dr.y + height, dr.z);

    //FindMeshOrigin(dr, dl, ul, ur);
    
    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = ul;
    boundaries[3] = ur;

    //FindMeshOrigin(boundaries[0], boundaries[1], boundaries[2], boundaries[3]);

    frame = new Base.DoorFrame(this);
    body = new Base.DoorBody(this);
  }

  public override void Draw ()
  {
    //base.Draw();

    frame.FindVertices();
    frame.FindTriangles();
    frame.Draw();

    body.FindVertices();
    body.FindTriangles();
    body.Draw();
  }
}

} // namespace Thesis