
namespace Thesis {
namespace Base {

public class Balcony : FaceComponent
{
  /*************** FIELDS ***************/

  public BalconyFloor balconyFloor;

  public BalconyRail balconyRail;

  /*************** CONSTRUCTORS ***************/

  public Balcony (Face parent, ComponentCoordinate position) 
    : base(parent, position)
  { }

  public override void Destroy()
  {
    base.Destroy();

    balconyFloor.Destroy();
    balconyRail.Destroy();
  }
}

} // namespaces Base
} // namespace Thesis