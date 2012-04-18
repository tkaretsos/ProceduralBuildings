
namespace Thesis {
namespace Base {

public sealed class DoorBody : ComponentBody
{
  /*************** CONSTRUCTORS ***************/

  public DoorBody (Door parent)
    : base(parent, "door_body", "DoorBody")
  {
    //FindMeshOrigin(parent.boundaries[0],
    //               parent.boundaries[2],
    //               parent.boundaries[1],
    //               parent.boundaries[3]);

    //for (int i = 0; i < 4; ++i)
    //  Util.PrintVector((i + 1).ToString() + " -", boundaries[i]);
  }
}

} // namespace Base
} // namespace Thesis