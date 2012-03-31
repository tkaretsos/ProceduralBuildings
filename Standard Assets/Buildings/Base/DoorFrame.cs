using System;
using UnityEngine;
using System.Collections.Generic;

namespace Base
{

public sealed class DoorFrame : Drawable
{
  /*************** FIELDS ***************/

  public readonly Door parentDoor;

  public List<Vector3> boundaries = new List<Vector3>();

  public Building parentBuilding
  {
    get { return parentDoor.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public DoorFrame (Door parent)
    : base("door_frame", "WindowFrameMaterial", false)
  {
    parentDoor = parent;

    foreach (var point in parentDoor.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - parentDoor.depth * parentDoor.normal);
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
      6, 1, 2
    };
  }

  public override void Draw ()
  {
    base.Draw();

    transform.parent = parentBuilding.transform;
  }
}

}
