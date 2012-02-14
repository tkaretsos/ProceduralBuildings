using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class FaceComponent
{
  private readonly Face _parent_face;
  private float _height;
  private List<Vector3> _boundaries = new List<Vector3>();


  // properties

  public ReadOnlyCollection<Vector3> boundaries
  {
    get { return _boundaries.AsReadOnly(); }
  }

  // constructors

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
    _height = _parent_face.parentBuilding.floorHeight * height_modifier;
  
    _boundaries.Add(dr);
    _boundaries.Add(dl);
    _boundaries.Add(new Vector3(dl.x, dl.y + _height, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _height, dr.z));
  }
}
