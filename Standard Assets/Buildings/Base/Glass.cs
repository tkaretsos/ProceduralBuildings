using System.Collections.Generic;
using UnityEngine;

namespace Base 
{

public sealed class Glass : ComponentBody
{
  private NeoclassicalBalconyDoor neoclassicalBalcony;

  /*************** CONSTRUCTORS ***************/

  public Glass (FaceComponent parent)
    : base(parent, "window_glass")
  {
    active = false;
    material = Resources.Load("Materials/Glass", typeof(Material)) as Material;
  }
}

} // namespace Base
