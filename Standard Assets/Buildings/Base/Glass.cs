using System.Collections.Generic;
using UnityEngine;

namespace Base {

public sealed class Glass : ComponentBody, ICombinable
{
  /*************** CONSTRUCTORS ***************/

  public Glass (FaceComponent parent)
    : base(parent, "window_glass")
  {
    active = false;
    material = Resources.Load("Materials/Glass", typeof(Material)) as Material;
  }

  GameObject ICombinable.gameObject
  {
    get { return gameObject; }
  }

  MeshFilter ICombinable.meshFilter
  {
    get { return meshFilter; }
  }
}

} // namespace Base
