using UnityEngine;

public class NeoclassicalWindow : Base.FaceComponent {

  public NeoclassicalWindow (Base.Face parent, Vector3 dr, Vector3 dl)
  : base (parent)
  {
    _height = parentBuilding.floorHeight * 0.5f;

    float height_modifier = 0.25f * parentFace.parentBuilding.floorHeight;

    _boundaries.Add(new Vector3(dr.x, dr.y + height_modifier, dr.z));
    _boundaries.Add(new Vector3(dl.x, dl.y + height_modifier, dl.z));
    _boundaries.Add(new Vector3(dl.x, dl.y + _height + height_modifier, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _height + height_modifier, dr.z));
  }
}
