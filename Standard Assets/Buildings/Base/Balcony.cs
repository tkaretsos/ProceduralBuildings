
namespace Base
{

public class Balcony : FaceComponent
{
  public BalconyFrame balconyFrame;

  public Glass balconyGlass;

  public Balcony (Face parent) : base(parent)
  {
    depth = 0.2f;
  }
}

}
