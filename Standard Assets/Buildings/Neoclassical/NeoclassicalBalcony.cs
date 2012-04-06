using UnityEngine;

public sealed class NeoclassicalBalcony : Base.Balcony
{
  /*************** CONSTRUCTORS ***************/

  public NeoclassicalBalcony (Base.Face parent, Vector3 dr, Vector3 dl, int floor)
    : base(parent, floor)
  {
    height = ((Neoclassical) parentBuilding).balconyHeight;

    boundaries.Add(dr);
    boundaries.Add(dl);
    boundaries.Add(new Vector3(dl.x, dl.y + height, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height, dr.z));

    balconyFrame = new Base.BalconyFrame(this);
    balconyGlass = new Base.Glass(this);
  }

  /*************** METHODS ***************/

  public override void Draw ()
  {
    base.Draw();

    balconyGlass.Draw();
    balconyFrame.Draw();
  }
}
