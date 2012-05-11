using UnityEngine;

namespace Thesis {

public class Door : FaceComponent
{
  public Door (Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position) 
    : base(parent, position)
  {
    height = ((NeoBuildingMesh) parentBuilding).doorHeight;
    depth = 0.4f;

    dr += 0.4f * parentFace.right;
    dl -= 0.4f * parentFace.right;

    var ul = new Vector3(dl.x, dl.y + height, dl.z);
    var ur = new Vector3(dr.x, dr.y + height, dr.z);

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = ul;
    boundaries[3] = ur;

    frame = new DoorFrame(this);
    frame.name = "neo_door_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new DoorBody(this);
    body.name = "neo_door_body";
    body.material = ((Neoclassical) parentBuilding.parent).doorMaterial;
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