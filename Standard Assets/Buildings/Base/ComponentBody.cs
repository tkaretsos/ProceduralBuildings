using System.Collections.Generic;
using UnityEngine;

namespace Base
{

public class ComponentBody : Drawable
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
    get { return parentComponent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentBody (FaceComponent parent, string name)
    : base(name)
  {
    parentComponent = parent;

    foreach (var point in parentComponent.boundaries)
      boundaries.Add(point - parentComponent.depth * parentComponent.normal + 
                             parentBuilding.meshOrigin);
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
