using UnityEngine;

namespace Thesis {
namespace Base {

public class BalconyFloor : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly BalconyDoor parentBalconyDoor;

  public float height;

  public float width;

  public float depth;

  public Neoclassical parentBuilding
  {
    get { return (Neoclassical) parentBalconyDoor.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public BalconyFloor (BalconyDoor parent)
    : base("balcony_floor", "Building")
  {
    parentBalconyDoor = parent;

    height = parentBuilding.balconyFloorHeight;
    width = parentBuilding.balconyFloorWidth;
    depth = parentBuilding.balconyFloorDepth;

    var tmp_right = parentBalconyDoor.parentFace.right * width;
    var tmp_normal = parentBalconyDoor.parentFace.normal * depth;

    boundaries = new Vector3[8];
    boundaries[0] = parentBalconyDoor.boundaries[1] - tmp_right + tmp_normal;
    boundaries[1] = parentBalconyDoor.boundaries[1] - tmp_right - tmp_normal;
    boundaries[2] = parentBalconyDoor.boundaries[0] + tmp_right - tmp_normal;
    boundaries[3] = parentBalconyDoor.boundaries[0] + tmp_right + tmp_normal;

    for (var i = 0; i < 4; ++i)
      boundaries[i + 4] = boundaries[i] + Vector3.up * height;
  }

  public override void FindVertices ()
  {
    vertices = boundaries;
  }

  public override void FindTriangles ()
  {
    triangles = new int[] {
      // bottom
      0, 2, 1,
      0, 3, 2,

      // top
      4, 5, 6,
      4, 6, 7,

      // front
      0, 4, 7,
      0, 7, 3,

      // left side
      0, 1, 4,
      4, 1, 5,

      // right side
      3, 7, 6,
      3, 6, 2
    };
  }

  public override void Draw ()
  {
    base.Draw();

    gameObject.transform.parent = parentBuilding.gameObject.transform;
  }
}

} // namespace Base
} // namespace Thesis