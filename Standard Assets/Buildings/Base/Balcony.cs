
namespace Base
{

public class Balcony : FaceComponent
{
  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  public Balcony (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.2f;
  }
}

}
