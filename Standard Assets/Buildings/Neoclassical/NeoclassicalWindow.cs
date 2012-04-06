using UnityEngine;

public sealed class NeoclassicalWindow : Base.Window
{
  /*************** CONSTRUCTORS ***************/

  public NeoclassicalWindow (Base.Face parent, Vector3 dr, Vector3 dl, int floor)
    : base (parent, floor)
  {
    height = ((Neoclassical) parentBuilding).windowHeight;
    float height_modifier = parentBuilding.floorHeight / 2 - height / 2;

    boundaries.Add(new Vector3(dr.x, dr.y + height_modifier, dr.z));
    boundaries.Add(new Vector3(dl.x, dl.y + height_modifier, dl.z));
    boundaries.Add(new Vector3(dl.x, dl.y + height + height_modifier, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height + height_modifier, dr.z));

    windowFrame = new Base.WindowFrame(this);
    windowGlass = new Base.Glass(this);
  }

  /*************** METHODS ***************/

  public override void Draw ()
  {
    base.Draw();

    windowFrame.Draw();
    windowGlass.Draw();
  }
}
