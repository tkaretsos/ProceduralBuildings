
namespace Thesis {
namespace Base {

public class BalconyDoor : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFloor balconyFloor;

  /*************** CONSTRUCTORS ***************/

  public BalconyDoor (Face parent, ComponentCoordinate position) 
    : base(parent, position)
  { }
}

} // namespaces Base
} // namespace Thesis