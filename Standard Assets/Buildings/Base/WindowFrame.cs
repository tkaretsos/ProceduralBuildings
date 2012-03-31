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
  }
}

} // namespace Base
