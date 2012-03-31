using UnityEngine;
using System.Collections.Generic;

namespace Base {

public sealed class WindowFrame : ComponentFrame
{
  /*************** CONSTRUCTORS ***************/

  public WindowFrame (Base.Window parent)
    : base(parent, "window_frame")
  {
    material = Resources.Load("Materials/ComponentFrame", typeof(Material)) as Material;
    active = false;

    foreach (var point in parentComponent.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - parentComponent.depth * parentComponent.normal);
  }
}

} // namespace Base
