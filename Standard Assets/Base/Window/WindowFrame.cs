using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public sealed class WindowFrame : ComponentFrame
{
  /*************** FIELDS ***************/

  public float width = 0.1f;

  /*************** CONSTRUCTORS ***************/

  public WindowFrame (Base.Window parent)
    : base(parent)
  { }
}

} // namespace Base
} // namespace Thesis