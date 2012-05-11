using UnityEngine;

namespace Thesis {

public class Balcony : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFloor balconyFloor;

  public BalconyRail balconyRail;

  public Shutter rightShutter;

  public Shutter leftShutter;

  /*************** CONSTRUCTORS ***************/

  public Balcony (Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position) 
    : base(parent, position)
  {
    height = parentBuilding.balconyHeight;
    width = (dr - dl).magnitude;
    depth = 0.2f;

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = new Vector3(dl.x, dl.y + height, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + height, dr.z);

    frame = new BalconyFrame(this);
    frame.name = "neo_balcony_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new BalconyBody(this);
    body.name = "neo_balcony_body";
    body.material = parentBuilding.parent.balconyDoorMaterial;
    parentBuilding.parent.AddCombinable(body.material.name, body);

    balconyFloor = new BalconyFloor(this);
    balconyFloor.name = "neo_balcony_floor";
    balconyFloor.material = MaterialManager.Instance.Get("Building");
    parentBuilding.parent.AddCombinable(balconyFloor.material.name, balconyFloor);

    balconyRail = new BalconyRail(balconyFloor);
    balconyRail.name = "neo_balcony_rail";
    balconyRail.material = MaterialManager.Instance.Get("mat_neo_balcony_rail");
    parentBuilding.parent.AddCombinable(balconyRail.material.name, balconyRail);

    rightShutter = new Shutter(this, ShutterSide.Right);
    rightShutter.name = "bal_right_shutter";
    rightShutter.material = parentBuilding.parent.shutterMaterial;
    parentBuilding.parent.AddCombinable(rightShutter.material.name, rightShutter);

    leftShutter = new Shutter(this, ShutterSide.Left);
    leftShutter.name = "bal_left_shutter";
    leftShutter.material = parentBuilding.parent.shutterMaterial;
    parentBuilding.parent.AddCombinable(leftShutter.material.name, leftShutter);
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

    balconyRail.FindVertices();
    balconyRail.FindTriangles();
    balconyRail.Draw();

    Shutter.SetAngles();

    rightShutter.FindVertices();
    rightShutter.FindTriangles();
    rightShutter.Draw();

    leftShutter.FindVertices();
    leftShutter.FindTriangles();
    leftShutter.Draw();
  }

  public override void Destroy()
  {
    base.Destroy();

    balconyFloor.Destroy();
    balconyRail.Destroy();
    rightShutter.Destroy();
    leftShutter.Destroy();
  }
}

} // namespace Thesis