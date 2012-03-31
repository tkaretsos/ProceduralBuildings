using System.Collections.Generic;
using UnityEngine;

namespace Base 
{

public sealed class WindowBody : ComponentBody
{
  /*************** CONSTRUCTORS ***************/

  public WindowBody (Window parent)
    : base(parent, "window_glass")
  {
    active = false;
    material = Resources.Load("Materials/WindowGlass", typeof(Material)) as Material;
  }
}

} // namespace Base
