namespace Base
{

public class Door : FaceComponent
{
  public DoorFrame doorFrame;

  public DoorBody doorBody;

  public Door (Face parent, int floor) : base(parent, floor)
  {
    depth = 0.4f;
  }
}

} // namespace Base
