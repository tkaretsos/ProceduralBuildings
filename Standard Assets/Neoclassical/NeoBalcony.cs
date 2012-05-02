using UnityEngine;

namespace Thesis {

public sealed class NeoBalcony : Base.Balcony
{
  /*************** CONSTRUCTORS ***************/

  public NeoBalcony (Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
    : base(parent, position)
  {
    height = ((NeoBuildingMesh) parentBuilding).balconyHeight;
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
    body.material = MaterialManager.Instance.Get("Glass");
    parentBuilding.parent.AddCombinable(body.material.name, body);

    balconyFloor = new Base.BalconyFloor(this);
    balconyFloor.name = "neo_balcony_floor";
    balconyFloor.material = MaterialManager.Instance.Get("Building");
    parentBuilding.parent.AddCombinable(balconyFloor.material.name, balconyFloor);

    balconyRail = new Base.BalconyRail(balconyFloor);
    balconyRail.name = "neo_balcony_rail";
    balconyRail.material = MaterialManager.Instance.Get("mat_neo_balcony_rail");
    parentBuilding.parent.AddCombinable(balconyRail.material.name, balconyRail);
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
  }
}

} // namespace Thesis