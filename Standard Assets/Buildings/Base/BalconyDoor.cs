
namespace Base {

public class BalconyDoor : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  /*************** CONSTRUCTORS ***************/

  public BalconyDoor (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.2f;
  }
}

}
