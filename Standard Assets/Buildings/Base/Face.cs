using UnityEngine;
using System.Collections.Generic;

public class Face
{
  // fields
  
  private readonly Building _parent;
  private Vector3 _normal;
  private Vector3 _right;
  private Vector3[] _vertices;
  private float _width;
  private int _components_per_floor;
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

  public Vector3[] Vertices
  {
    get { return _vertices; }
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

    _components_per_floor = 0;
  }
  
  
  // methods
  
  public void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    _components_per_floor = Mathf.CeilToInt(_width / (component_width + inbetween_space));
    float fixed_space = (_width - _components_per_floor * component_width) / (_components_per_floor + 1);

    for (int floor = 0; floor < _parent.FloorNumber; ++floor)
    {
      float offset = fixed_space;
      for (int i = 0; i < _components_per_floor; ++i)
      {
        Vector3 dr = _boundaries[0] - _right * offset + (new Vector3(0f, floor * _parent.FloorHeight, 0f));
        Vector3 dl = dr - _right * component_width;
        offset += component_width;
        _face_components.Add(new FaceComponent(this, dr, dl, 3f / 5f));
        offset += fixed_space;
      }
    }
  }

  /// <summary>
  /// Creates an array of Vector3 objects that contains the vertices of
  /// the FaceComponents attached to this Face. The vertices are put in an
  /// order so that it will be easy to form the triangles of the building mesh.
  /// </summary>
  /// <returns>
  /// The vertices array.
  /// </returns>
  public void CreateVerticesArray ()
  {
    _vertices = new Vector3[4 * _components_per_floor * (_parent.FloorNumber + 1)];

    int double_cpf = 2 * _components_per_floor;
    int index = 0;
    foreach (FaceComponent fc in _face_components)
    {
      _vertices[index]     = fc.Boundaries[0];
      _vertices[index + 1] = fc.Boundaries[1];

      _vertices[index + double_cpf]     = fc.Boundaries[3];
      _vertices[index + double_cpf + 1] = fc.Boundaries[2];

      if ((index += 2) % double_cpf == 0)
        index += double_cpf;
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
