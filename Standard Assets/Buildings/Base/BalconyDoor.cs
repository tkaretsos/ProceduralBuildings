
namespace Base
{

public class BalconyDoor : FaceComponent
{
  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  public BalconyDoor (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.2f;
  }
}

}
