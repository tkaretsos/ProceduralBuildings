
namespace Base {

public class BalconyDoor : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  public BalconyFloor balconyFloor;

  /*************** CONSTRUCTORS ***************/

  public BalconyDoor (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.2f;
  }
}

}
