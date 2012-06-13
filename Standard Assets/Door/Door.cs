using UnityEngine;

namespace Thesis {

public class Door : FaceComponent
{
  public Building masterParent
  {
    get { return parentBuilding.parent; }
  }

  public Door (Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position) 
    : base(parent, position)
  {
    height = parentBuilding.doorHeight;
    depth = 0.4f;

    float mod = 0.4f;
    if (masterParent.doorWidth > 0f)
      mod = (masterParent.doorWidth - (dr - dl).magnitude) / 2f;

    dr += mod * parentFace.right;
    dl -= mod * parentFace.right;

    var ul = new Vector3(dl.x, dl.y + height, dl.z);
    var ur = new Vector3(dr.x, dr.y + height, dr.z);

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = ul;
    boundaries[3] = ur;

    frame = new ComponentFrame(this);
    frame.name = "neo_door_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new DoorBody(this);
    body.name = "neo_door_body";
    body.material = parentBuilding.parent.doorMaterial;
    parentBuilding.parent.AddCombinable(body.material.name, body);
  }

  public override void Draw ()
  {
    frame.FindVertices();
    frame.FindTriangles();
    frame.Draw();

    body.FindVertices();
    body.FindTriangles();
    body.Draw();
  }
}

} // namespace Thesis