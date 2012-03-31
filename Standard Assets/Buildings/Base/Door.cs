using System;

namespace Base
{

public class Door : FaceComponent
{
  public float depth;

  public DoorFrame doorFrame;

  public DoorBody doorBody;

  public Door (Face parent) : base(parent)
  {
    depth = 0.4f;
  }
}

} // namespace Base
