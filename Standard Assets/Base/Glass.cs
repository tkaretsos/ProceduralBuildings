using System.Collections.Generic;
using UnityEngine;

using ICombinable = Thesis.Interface.ICombinable;
using IDrawable = Thesis.Interface.IDrawable;

namespace Thesis {
namespace Base {

public sealed class Glass : ComponentBody, IDrawable, ICombinable
{
  /*************** CONSTRUCTORS ***************/

  public Glass (FaceComponent parent)
    : base(parent, "window_glass", "Glass")
  { }
}

} // namespace Base
} // namespace Thesis