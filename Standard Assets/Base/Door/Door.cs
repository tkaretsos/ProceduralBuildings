
namespace Thesis {
namespace Base {

public class Door : FaceComponent
{
  public DoorFrame doorFrame;

  public DoorBody doorBody;

  public Door (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.4f;
  }
}

} // namespace Base
} // namespace Thesis