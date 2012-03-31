using System;
using UnityEngine;
using System.Collections.Generic;

namespace Base
{

public sealed class DoorFrame : ComponentFrame
{
  public DoorFrame (Door parent)
    : base(parent, "door_frame")
  {
    material = Resources.Load("Materials/ComponentFrame", typeof(Material)) as Material;
    active = true;

    foreach (var point in parentComponent.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - parentComponent.depth * parentComponent.normal);
  }

  /*************** METHODS ***************/

  public override int[] FindTriangles ()
  {
    return new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2
    };
  }
}

}
