using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Base {

public class FaceComponent
{
  /// <summary>
  /// The face which this component is attached to.
  /// </summary>
  private readonly Face _parent_face;

  /// <summary>
  /// The height of this component.
  /// </summary>
  protected float _height;

  /// <summary>
  /// The boundaries of this component, which are four points on the parent face.
  /// </summary>
  protected List<Vector3> _boundaries = new List<Vector3>();


  // properties

  /// <summary>
  /// Returns a read only collection of the boundaries list.
  /// </summary>
  public ReadOnlyCollection<Vector3> boundaries
  {
    get { return _boundaries.AsReadOnly(); }
  }

  public Face parentFace
  {
    get { return _parent_face; }
  }

  /// <summary>
  /// Returns the parent building, to which this component is attached.
  /// </summary>
  public Building parentBuilding
  {
    get { return _parent_face.parentBuilding; }
  }

  // constructors

  public FaceComponent (Face parent)
  {
    _parent_face = parent;
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
  public FaceComponent (Face parent, Vector3 dr, Vector3 dl, float height_modifier = 1f)
  {
    _parent_face = parent;
    _height = parentBuilding.floorHeight * height_modifier;
  
    _boundaries.Add(dr);
    _boundaries.Add(dl);
    _boundaries.Add(new Vector3(dl.x, dl.y + _height, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _height, dr.z));
  }
}

} // namespace Base
