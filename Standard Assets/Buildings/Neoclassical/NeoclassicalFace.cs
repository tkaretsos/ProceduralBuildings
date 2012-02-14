using UnityEngine;
using System.Collections;

public class NeoclassicalFace : Face {

  public NeoclassicalFace (Building parent, Vector3 dr, Vector3 dl)
  : base (parent, dr, dl)
  {}

  public override void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    _components_per_floor = Mathf.CeilToInt(_width / (component_width + inbetween_space));
    float fixed_space = (_width - componentsPerFloor * component_width) / (componentsPerFloor + 1);

    for (int floor = 0; floor < parentBuilding.floorNumber; ++floor)
    {
      float offset = fixed_space;
      for (int i = 0; i < _components_per_floor; ++i)
      {
        Vector3 dr = _boundaries[0] - _right * offset + (new Vector3(0f, floor * parentBuilding.floorHeight, 0f));
        Vector3 dl = dr - _right * component_width;
        offset += component_width;
        _face_components.Add(new NeoclassicalWindow(this, dr, dl));
        offset += fixed_space;
      }
    }
  }
}
