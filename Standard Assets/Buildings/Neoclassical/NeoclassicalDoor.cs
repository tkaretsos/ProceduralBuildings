using UnityEngine;

public class NeoclassicalDoor : FaceComponent {

  public NeoclassicalDoor(Face parent, Vector3 dr, Vector3 dl)
  : base (parent)
  {
    _height = parentBuilding.floorHeight * 0.75f;

    _boundaries.Add(dr);
    _boundaries.Add(dl);
    _boundaries.Add(new Vector3(dl.x, dl.y + _height, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _height, dr.z));
  }
}
