using UnityEngine;

namespace Thesis {

public class Window : FaceComponent
{
  /*************** FIELDS ***************/

  public Shutter rightShutter;

  public Shutter leftShutter;

  public ComponentDecor decor;

  /*************** CONSTRUCTORS ***************/

  public Window (Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position) 
    : base(parent, position)
  {
    height = parentBuilding.windowHeight;
    depth = 0.2f;
    width = (dr - dl).magnitude;
    float height_modifier = parentBuilding.floorHeight / 2.5f - height / 2;

    boundaries = new Vector3[4];
    boundaries[0] = new Vector3(dr.x, dr.y + height_modifier, dr.z);
    boundaries[1] = new Vector3(dl.x, dl.y + height_modifier, dl.z);
    boundaries[2] = new Vector3(dl.x, dl.y + height + height_modifier, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + height + height_modifier, dr.z);

    frame = new WindowFrame(this);
    frame.name = "neo_window_frame";
    frame.material = MaterialManager.Instance.Get("ComponentFrame");
    parentBuilding.parent.AddCombinable(frame.material.name, frame);

    body = new WindowBody(this);
    body.name = "neo_window_body";
    body.material = parentBuilding.parent.windowMaterial;
    parentBuilding.parent.AddCombinable(body.material.name, body);

    rightShutter = new Shutter(this, ShutterSide.Right);
    rightShutter.name = "right_shutter";
    rightShutter.material = parentBuilding.parent.shutterMaterial;
    parentBuilding.parent.AddCombinable(rightShutter.material.name, rightShutter);

    leftShutter = new Shutter(this, ShutterSide.Left);
    leftShutter.name = "left_shutter";
    leftShutter.material = parentBuilding.parent.shutterMaterial;
    parentBuilding.parent.AddCombinable(leftShutter.material.name, leftShutter);

    if (position.floor >= 1)
    {
      decor = new ComponentDecor(this);
      decor.name = "window_decor";
      decor.material = parentBuilding.parent.compDecorMaterial;
      parentBuilding.parent.AddCombinable(decor.material.name, decor);
    }
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

    //Shutter.SetAngles();

    rightShutter.FindVertices();
    rightShutter.FindTriangles();
    rightShutter.Draw();

    leftShutter.FindVertices();
    leftShutter.FindTriangles();
    leftShutter.Draw();

    if (decor != null)
    {
      decor.FindVertices();
      decor.FindTriangles();
      decor.Draw();
    }
  }

  public override void Destroy()
  {
    base.Destroy();

    rightShutter.Destroy();
    leftShutter.Destroy();

    if (decor != null)
      decor.Destroy();
  }
}

} // namespace Thesis