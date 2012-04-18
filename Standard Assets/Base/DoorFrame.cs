
namespace Thesis {
namespace Base {

public sealed class DoorFrame : ComponentFrame
{
  /*************** CONSTRUCTORS ***************/

  public DoorFrame (Door parent)
    : base(parent, "door_frame", "ComponentFrame")
  { }

  /*************** METHODS ***************/

  public override void FindTriangles ()
  {
    triangles = new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2
    };
  }
}

} // namespace Base
} // namespace Thesis