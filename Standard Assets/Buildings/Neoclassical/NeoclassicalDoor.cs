using UnityEngine;

public class NeoclassicalDoor : Base.FaceComponent {

  public NeoclassicalDoor(Base.Face parent, Vector3 dr, Vector3 dl)
  : base (parent)
  {
    height = parentBuilding.floorHeight * 0.75f;

    boundaries.Add(dr);
    boundaries.Add(dl);
    boundaries.Add(new Vector3(dl.x, dl.y + height, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height, dr.z));
  }
}
