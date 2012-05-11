
namespace Thesis {

public sealed class DoorFrame : ComponentFrame
{
  /*************** CONSTRUCTORS ***************/

  public DoorFrame (Door parent)
    : base(parent)
  { }

  /*************** METHODS ***************/

  public override void FindTriangles ()
  {
    triangles = new int[18];
    var i = 0;

    // right
    triangles[i++] =  0;
    triangles[i++] =  4;
    triangles[i++] =  7;

    triangles[i++] =  0;
    triangles[i++] =  7;
    triangles[i++] =  3;

    // left
    triangles[i++] =  1;
    triangles[i++] =  2;
    triangles[i++] =  6;

    triangles[i++] =  1;
    triangles[i++] =  6;
    triangles[i++] =  5;

    // top
    triangles[i++] = 10;  // 2 + 8;
    triangles[i++] = 11;  // 3 + 8;
    triangles[i++] = 14;  // 6 + 8;

    triangles[i++] = 14;  // 6 + 8;
    triangles[i++] = 11;  // 3 + 8;
    triangles[i++] = 15;  // 7 + 8;
  }
}

} // namespace Thesis