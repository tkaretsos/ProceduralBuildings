using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public sealed class WindowFrame : ComponentFrame
{
  /*************** FIELDS ***************/

  public float width = 0.1f;

  /*************** CONSTRUCTORS ***************/

  public WindowFrame (Base.Window parent)
    : base(parent)
  {
    int index = boundaries.Length; // == 8
    System.Array.Resize<Vector3>(ref boundaries, boundaries.Length + 10);

    boundaries[index++] = new Vector3((boundaries[4].x + boundaries[5].x) / 2,
                                       boundaries[4].y,
                                      (boundaries[4].z + boundaries[5].z) / 2);

    boundaries[index++] = new Vector3((boundaries[6].x + boundaries[7].x) / 2,
                                       boundaries[6].y,
                                      (boundaries[6].z + boundaries[7].z) / 2);

    Vector3 up_width = Vector3.up * width;
    Vector3 right_width = parentFace.right * width;

    boundaries[index++] = boundaries[8] + up_width - right_width;
    boundaries[index++] = boundaries[5] + up_width + right_width;
    boundaries[index++] = boundaries[6] - up_width + right_width;
    boundaries[index++] = boundaries[9] - up_width - right_width;

    boundaries[index++] = boundaries[4] + up_width - right_width;
    boundaries[index++] = boundaries[8] + up_width + right_width;
    boundaries[index++] = boundaries[9] - up_width + right_width;
    boundaries[index++] = boundaries[7] - up_width - right_width;
  }

  public override void FindTriangles ()
  {
    triangles = new int[] {
      // frame between window and wall
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4,
      
      // frame on window (left shutter)
       8, 5, 10,
      10, 5, 11,
      11, 5,  6,
      11, 6, 12,
      12, 6,  9,
      12, 9, 13,
      13, 9, 10,
      10, 9,  8,

      // frame on window (right shutter)
       4, 8, 15,
      14, 4, 15,
       8, 9, 15,
      15, 9, 16,
      16, 9,  7,
      16, 7, 17,
      17, 7, 14,
      14, 7,  4
    };
  }
}

} // namespace Base
} // namespace Thesis