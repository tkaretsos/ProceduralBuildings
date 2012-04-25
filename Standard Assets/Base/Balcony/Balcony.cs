
namespace Thesis {
namespace Base {

public class Balcony : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFloor balconyFloor;

  /*************** CONSTRUCTORS ***************/

  public Balcony (Face parent, ComponentCoordinate position) 
    : base(parent, position)
  { }
}

} // namespaces Base
} // namespace Thesis