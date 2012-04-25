
namespace Thesis {
namespace Base {

public class Window : FaceComponent
{
  public WindowFrame windowFrame;

  public Window (Face parent, ComponentCoordinate position) : base(parent, position)
  {
    depth = 0.2f;
  }
}

} // namespace Base
} // namespace Thesis