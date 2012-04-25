using UnityEngine;

namespace Thesis {

public sealed class NeoBalconyDoor : Base.BalconyDoor
{
  /*************** CONSTRUCTORS ***************/

  public NeoBalconyDoor (Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
    : base(parent, position)
  {
    height = ((NeoBuildingMesh) parentBuilding).balconyHeight;

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = new Vector3(dl.x, dl.y + height, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + height, dr.z);

    frame = new Base.BalconyFrame(this);
    body = new Base.Glass(this);

    balconyFloor = new Base.BalconyFloor(this);
  }

  /*************** METHODS ***************/

  public override void Draw ()
  {
    //base.Draw();

    body.FindVertices();
    body.FindTriangles();
    body.Draw();

    frame.FindVertices();
    frame.FindTriangles();
    frame.Draw();

    balconyFloor.FindVertices();
    balconyFloor.FindTriangles();
    balconyFloor.Draw();
  }
}

} // namespace Thesis