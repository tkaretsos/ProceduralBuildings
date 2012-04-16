using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public class FaceComponent
{
  /*************** FIELDS ***************/

  /// <summary>
  /// The face which this component is attached to.
  /// </summary>
  public readonly Face parentFace;

  /// <summary>
  /// The height of this component.
  /// </summary>
  public float height;

  public float depth;

  public float width;

  public readonly ComponentCoordinate position;

  public ComponentFrame frame;

  public ComponentBody body;

  /// <summary>
  /// The boundaries of this component, which are four points on the parent face.
  /// </summary>
  public List<Vector3> boundaries = new List<Vector3>();

  /// <summary>
  /// Returns the parent building, to which this component is attached.
  /// </summary>
  public Building parentBuilding
  {
    get { return parentFace.parentBuilding; }
  }

  public Vector3 normal
  {
    get { return parentFace.normal; }
  }

  /*************** CONSTRUCTORS ***************/

  public FaceComponent (Face parent, ComponentCoordinate position)
  {
    parentFace = parent;
    this.position = position;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="FaceComponent"/> class.
  /// </summary>
  /// <param name='parent'>
  /// The parent face of this component.
  /// </param>
  /// <param name='width'>
  /// Width.
  /// </param>
  /// <param name='height'>
  /// Height.
  /// </param>
  /// <param name='dl'>
  /// Down-left point of the component.
  /// </param>
  /// <param name='dr'>
  /// Down-right point of the component.
  /// </param>
  public FaceComponent (Face parent, Vector3 dr, Vector3 dl, ComponentCoordinate position, float height_modifier = 1f)
  {
    parentFace = parent;
    this.position = position;
    height = parentBuilding.floorHeight * height_modifier;
  
    boundaries.Add(dr);
    boundaries.Add(dl);
    boundaries.Add(new Vector3(dl.x, dl.y + height, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + height, dr.z));

    width = (boundaries[0] - boundaries[1]).magnitude;
  }

  public virtual void Draw () { }
}

} // namespace Base
} // namespace Thesis