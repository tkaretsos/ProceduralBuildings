using UnityEngine;
using System.Collections.Generic;

public class Face
{
  // fields
  
  private readonly Building _parent;
  private Vector3 _normal;
  private Vector3 _right;
  private float _width;
  private bool _is_free;
  private List<Vector3> _boundaries = new List<Vector3>();
  private List<FaceComponent> _face_components = new List<FaceComponent>();
  
  
  // properties
  
  /// <summary>
  /// Gets the parent mesh.
  /// </summary>
  /// <value>
  /// The parent mesh.
  /// </value>
  public Building Parent
  {
    get { return _parent; }
  }
  
  
  // constructors
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Face"/> class,
  /// from the given points in clockwise order.
  /// </summary>
  /// <param name='parent'>
  /// The parent mesh of this face.
  /// </param>
  /// <param name='dr'>
  /// Down-right point of the face.
  /// </param>
  /// <param name='dl'>
  /// Down-left point of the face.
  /// </param>
  public Face (Building parent, Vector3 dr, Vector3 dl)
  {
    _parent = parent;
  
    _boundaries.Add(dr);
    _boundaries.Add(dl);
    _boundaries.Add(new Vector3(dl.x, dl.y + _parent.Height, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _parent.Height, dr.z));
  
    _right = new Vector3(_boundaries[0].x - _boundaries[1].x,
                         0f,
                         _boundaries[0].z - _boundaries[1].z);
    _width = _right.magnitude;
    _right.Normalize();
  
    _normal = Vector3.Cross(Vector3.up, _right);
    _normal.Normalize();
  }
  
  
  // methods
  
  public void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    int components_no = Mathf.CeilToInt(_width / (component_width + inbetween_space));
    float fixed_space = (_width - components_no * component_width) / (components_no + 1);

    for (int floor = 0; floor < _parent.FloorNumber; ++floor)
    {
      float offset = fixed_space;
      for (int i = 0; i < components_no; ++i)
      {
        Vector3 dr = _boundaries[0] - _right * offset + (new Vector3(0f, floor * _parent.FloorHeight, 0f));
        Vector3 dl = dr - _right * component_width;
        offset += component_width;
        _face_components.Add(new FaceComponent(this, dr, dl, 3f / 5f));
        offset += fixed_space;
      }
    }
  }
  
  
  public void Draw ()
  {
//    GL.PushMatrix();
//    GL.Begin(GL.QUADS);
//    foreach (Vector3 v in _boundaries)
//      GL.Vertex(v);
//    GL.End();
//    GL.PopMatrix();
    foreach (FaceComponent fc in _face_components)
      fc.Draw();
  }
}
