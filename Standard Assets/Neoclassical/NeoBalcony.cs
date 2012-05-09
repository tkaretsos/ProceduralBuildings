using UnityEngine;

namespace Thesis {

public sealed class NeoBalcony : Base.Balcony
{
  public Base.Shutter rightShutter;

  public Base.Shutter leftShutter;

  /*************** CONSTRUCTORS ***************/

  public NeoBalcony (Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
    : base(parent, position)
  {
    height = ((NeoBuildingMesh) parentBuilding).balconyHeight;
    width = (dr - dl).magnitude;
    depth = 0.2f;

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = new Vector3(dl.x, dl.y + height, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + height, dr.z);

    frame = new Base.BalconyFrame(this);
    frame.name = "neo_balcony_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new Base.BalconyBody(this);
    body.name = "neo_balcony_body";
    body.material = ((Neoclassical) parentBuilding.parent).balconyDoorMaterial;
    parentBuilding.parent.AddCombinable(body.material.name, body);

    balconyFloor = new Base.BalconyFloor(this);
    balconyFloor.name = "neo_balcony_floor";
    balconyFloor.material = MaterialManager.Instance.Get("Building");
    parentBuilding.parent.AddCombinable(balconyFloor.material.name, balconyFloor);

    balconyRail = new Base.BalconyRail(balconyFloor);
    balconyRail.name = "neo_balcony_rail";
    balconyRail.material = MaterialManager.Instance.Get("mat_neo_balcony_rail");
    parentBuilding.parent.AddCombinable(balconyRail.material.name, balconyRail);

    rightShutter = new Base.Shutter(this, ShutterSide.Right);
    rightShutter.name = "bal_right_shutter";
    rightShutter.material = ((Neoclassical) parentBuilding.parent).shutterMaterial;
    parentBuilding.parent.AddCombinable(rightShutter.material.name, rightShutter);

    leftShutter = new Base.Shutter(this, ShutterSide.Left);
    leftShutter.name = "bal_left_shutter";
    leftShutter.material = ((Neoclassical) parentBuilding.parent).shutterMaterial;
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

    var angles = 0;
    switch (Util.RollDice(new float[] {0.33f, 0.33f, 0.34f}))
    {
      case 1:
        angles = 150;
        break;

      case 2:
        angles = Random.Range(130, 150);
        break;

      default:
        break;
    }

    rightShutter.FindVertices();
    rightShutter.FindTriangles();
    rightShutter.angles = angles;
    rightShutter.Draw();

    leftShutter.FindVertices();
    leftShutter.FindTriangles();
    leftShutter.angles = angles;
    leftShutter.Draw();
  }

  public override void Destroy()
  {
    base.Destroy();

    rightShutter.Destroy();
    leftShutter.Destroy();
  }
}

} // namespace Thesis