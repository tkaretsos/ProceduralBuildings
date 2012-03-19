using UnityEngine;

public class NeoclassicalDoor : Base.Door
{
  public NeoclassicalDoor(Base.Face parent, Vector3 dr, Vector3 dl)
  : base (parent)
  {
    //height = parentBuilding.floorHeight * 0.9f;
    height = ((Neoclassical) parentBuilding).doorHeight;

    var new_dr = dr + 0.4f * parentFace.right;
    var new_dl = dl - 0.4f * parentFace.right;

    boundaries.Add(new_dr);
    boundaries.Add(new_dl);
    boundaries.Add(new Vector3(new_dl.x, new_dl.y + height, new_dl.z));
    boundaries.Add(new Vector3(new_dr.x, new_dr.y + height, new_dr.z));
  }
}
