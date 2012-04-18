
namespace Thesis {
namespace Base {

public sealed class Glass : ComponentBody
{
  /*************** CONSTRUCTORS ***************/

  public Glass (FaceComponent parent)
    : base(parent, "window_glass", "Glass")
  { }
}

} // namespace Base
} // namespace Thesis