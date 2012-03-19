using UnityEngine;

public sealed class NeoclassicalWindow : Base.Window
{
  public NeoclassicalWindow (Base.Face parent, Vector3 dr, Vector3 dl)
    : base (parent)
  {
    //height = parentBuilding.floorHeight * 0.6f;
    height = ((Neoclassical) parentBuilding).windowHeight;

    //float height_modifier = 0.2f * parentFace.parentBuilding.floorHeight;
    float height_modifier = parentBuilding.floorHeight / 2 - height / 2;

    boundaries.Add(new Vector3(dr.x, dr.y + height_modifier, dr.z));
    boundaries.Add(new Vector3(dl.x, dl.y + height_modifier, dl.z));
    boundaries.Add(new Vector3(dl.x, dl.y + height + height_modifier, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height + height_modifier, dr.z));

    windowFrame = new Base.WindowFrame(this);
    windowGlass = new Base.WindowGlass(this);
  }

  public override void Draw ()
  {
    base.Draw();

    windowFrame.Draw();
    windowGlass.Draw();
  }
}
