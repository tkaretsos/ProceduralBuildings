using UnityEngine;
using System.Collections.Generic;

namespace Base {

public sealed class WindowFrame : Drawable
{
  /*************** FIELDS ***************/

  public readonly Base.Window parentWindow;

  public List<Vector3> boundaries = new List<Vector3>();

  public Base.Face parentFace
  {
    get { return parentWindow.parentFace; }
  }

  public Base.Building parentBuilding
  {
    get { return parentWindow.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public WindowFrame (Base.Window parent)
    : base("window_frame", "WindowFrameMaterial", false)
  {
    parentWindow = parent;

    foreach (var point in parentWindow.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - parentWindow.depth * parentWindow.normal);
  }

  /*************** METHODS ***************/

  public override Vector3[] FindVertices ()
  {
    return boundaries.ToArray();
  }

  public override int[] FindTriangles ()
  {
    return new int[] {
      0, 4, 7,
      0, 7, 3,
      7, 6, 2,
      7, 2, 3,
      6, 5, 1,
      6, 1, 2,
      0, 1, 5,
      0, 5, 4
    };
  }

  public override void Draw ()
  {
    base.Draw();

    transform.parent = parentBuilding.transform;
  }
}

} // namespace Base
