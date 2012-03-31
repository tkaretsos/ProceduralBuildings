using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base {

public sealed class WindowBody : Drawable
{
  /*************** FIELDS ***************/

  public readonly Window parentWindow;

  public List<Vector3> boundaries = new List<Vector3>();

  public Face parentFace
  {
    get { return parentWindow.parentFace; }
  }

  public Building parentBuilding
  {
    get { return parentFace.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public WindowBody (Window parent)
    : base("window_glass", "WindowGlass", false)
  {
    parentWindow = parent;

    foreach (var point in parentWindow.boundaries)
      boundaries.Add(point - parentWindow.depth * parentWindow.normal + parentBuilding.meshOrigin);
  }

  /*************** METHODS ***************/

  public override Vector3[] FindVertices ()
  {
    return boundaries.ToArray();
  }

  public override int[] FindTriangles ()
  {
    return new int[] {
      0, 1, 3,
      1, 2, 3
    };
  }

  public override void Draw ()
  {
    base.Draw();

    transform.parent = parentBuilding.transform;
  }
}

} // namespace Base
