
namespace Base
{

public class Balcony : FaceComponent
{
  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  public Balcony (Face parent, int floor) : base(parent, floor)
  {
    depth = 0.2f;
  }
}

}
