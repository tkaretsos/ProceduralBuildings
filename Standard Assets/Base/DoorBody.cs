using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public sealed class DoorBody : ComponentBody
{
  /*************** CONSTRUCTORS ***************/

  public DoorBody (Door parent)
    : base(parent, "door_body", "DoorBody")
  { }
}

} // namespace Base
} // namespace Thesis