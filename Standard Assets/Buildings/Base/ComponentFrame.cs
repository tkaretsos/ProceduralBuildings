using System;
using UnityEngine;
using System.Collections.Generic;

namespace Base
{

public class ComponentFrame : Drawable
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public List<Vector3> boundaries = new List<Vector3>();

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public Building parentBuilding
  {
    get { return parentComponent.parentFace.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentFrame (FaceComponent parent, string name)
    : base(name)
  {
    parentComponent = parent;

    foreach (var point in parentComponent.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - parentComponent.depth * parentComponent.normal);
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

}
