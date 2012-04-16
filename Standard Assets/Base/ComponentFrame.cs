using UnityEngine;
using System.Collections.Generic;
using ICombinable = Thesis.Interface.ICombinable;

namespace Thesis {
namespace Base {

public class ComponentFrame : Drawable, ICombinable
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

  GameObject ICombinable.gameObject
  {
    get { return gameObject; }
  }

  MeshFilter ICombinable.meshFilter
  {
    get { return meshFilter; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentFrame (FaceComponent parent, string name)
    : base(name)
  {
    parentComponent = parent;

    foreach (var point in parentComponent.boundaries)
      boundaries.Add(point + parentBuilding.meshOrigin);

    for (var i = 0; i < 4; ++i)
      boundaries.Add(boundaries[i] - (parentComponent.depth - 0.001f) * parentComponent.normal);
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

    transform.parent = parentBuilding.gameObject.transform;
  }
}

} // namespace Base
} // namespace Thesis