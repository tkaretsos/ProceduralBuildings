using UnityEngine;

public sealed class NeoclassicalBalcony : Base.Balcony
{
  /*************** CONSTRUCTORS ***************/

  public NeoclassicalBalcony (Base.Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position)
    : base(parent, position)
  {
    height = ((Neoclassical) parentBuilding).balconyHeight;

    boundaries.Add(dr);
    boundaries.Add(dl);
    boundaries.Add(new Vector3(dl.x, dl.y + height, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height, dr.z));

    frame = new Base.BalconyFrame(this);
    body = new Base.Glass(this);
  }

  /*************** METHODS ***************/

  public override void Draw ()
  {
    base.Draw();

    body.Draw();
    frame.Draw();
  }
}
