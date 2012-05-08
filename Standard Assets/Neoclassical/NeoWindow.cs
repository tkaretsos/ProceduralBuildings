using UnityEngine;

namespace Thesis {

public sealed class NeoWindow : Base.Window
{
  public Base.Shutter rightShutter;

  public Base.Shutter leftShutter;

  /*************** CONSTRUCTORS ***************/

  public NeoWindow (Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
    : base (parent, position)
  {
    height = ((NeoBuildingMesh) parentBuilding).windowHeight;
    depth = 0.2f;
    width = (dr - dl).magnitude;
    float height_modifier = parentBuilding.floorHeight / 2.5f - height / 2;

    boundaries = new Vector3[4];
    boundaries[0] = new Vector3(dr.x, dr.y + height_modifier, dr.z);
    boundaries[1] = new Vector3(dl.x, dl.y + height_modifier, dl.z);
    boundaries[2] = new Vector3(dl.x, dl.y + height + height_modifier, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + height + height_modifier, dr.z);

    frame = new Base.WindowFrame(this);
    frame.name = "neo_window_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new Base.WindowBody(this);
    body.name = "neo_window_body";
    body.material = ((Neoclassical) parentBuilding.parent).windowMaterial;
    parentBuilding.parent.AddCombinable(body.material.name, body);

    rightShutter = new Base.Shutter(this, ShutterSide.Right);
    rightShutter.name = "right_shutter";
    rightShutter.material = ((Neoclassical) parentBuilding.parent).shutterMaterial;
    parentBuilding.parent.AddCombinable(rightShutter.material.name, rightShutter);

    leftShutter = new Base.Shutter(this, ShutterSide.Left);
    leftShutter.name = "left_shutter";
    leftShutter.material = ((Neoclassical) parentBuilding.parent).shutterMaterial;
    parentBuilding.parent.AddCombinable(leftShutter.material.name, leftShutter);
  }

  /*************** METHODS ***************/

  public override void Draw ()
  {
    //base.Draw();

    frame.FindVertices();
    frame.FindTriangles();
    frame.Draw();

    body.FindVertices();
    body.FindTriangles();
    body.Draw();

    rightShutter.FindVertices();
    rightShutter.FindTriangles();
    rightShutter.Draw();

    leftShutter.FindVertices();
    leftShutter.FindTriangles();
    leftShutter.Draw();
  }
}

} // namespace Thesis