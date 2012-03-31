using System;
using UnityEngine;
using System.Collections.Generic;

namespace Base
{

public sealed class DoorBody : Drawable
{
  /*************** FIELDS ***************/

  public readonly Door parentDoor;

  public List<Vector3> boundaries = new List<Vector3>();

  public Face parentFace
  {
    get { return parentDoor.parentFace; }
  }

  public Building parentBuilding
  {
    get { return parentDoor.parentFace.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public DoorBody (Door parent)
    : base("door_body", "DoorBodyMaterial")
  {
    parentDoor = parent;

    foreach (var point in parentDoor.boundaries)
      boundaries.Add(point - parentDoor.depth * parentDoor.normal + parentBuilding.meshOrigin);
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

}
