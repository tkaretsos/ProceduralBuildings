using UnityEngine;
using System.Collections.Generic;

public class Face
{
  // fields
  protected Vector3             _right;
  protected float               _width;
  protected List<Vector3>       _boundaries           = new List<Vector3>();
  protected List<FaceComponent> _face_components      = new List<FaceComponent>();
  protected int                 _components_per_floor = 0;

  private Vector3           _normal;
  private Vector3[]         _vertices;
  private int               _index_modifier       = 0;
  private int               _vertices_per_row     = 0;
  private readonly Building _parent_building;

  
  // properties
  
  /// <summary>
  /// Gets the parent mesh.
  /// </summary>
  /// <value>
  /// The parent mesh.
  /// </value>
  public Building parentBuilding
  {
    get { return _parent_building; }
  }

  public Vector3[] vertices
  {
    get { return _vertices; }
  }

  public int indexModifier
  {
    get { return _index_modifier; }
  }

  public int verticesPerRow
  {
    get { return _vertices_per_row; }
  }

  public int componentsPerFloor
  {
    get { return _components_per_floor; }
    protected set { _components_per_floor = value; }
  }

  public float width
  {
    get { return _width; }
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
    _parent_building = parent;
  
    _boundaries.Add(dr);
    _boundaries.Add(dl);
    _boundaries.Add(new Vector3(dl.x, dl.y + _parent_building.height, dl.z));
    _boundaries.Add(new Vector3(dr.x, dr.y + _parent_building.height, dr.z));
  
    _right = new Vector3(_boundaries[0].x - _boundaries[1].x,
                         0f,
                         _boundaries[0].z - _boundaries[1].z);
    _width = _right.magnitude;
    _right.Normalize();
  
    _normal = Vector3.Cross(Vector3.up, _right);
    _normal.Normalize();
  }
  
  
  // methods
  
  public virtual void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    _components_per_floor = Mathf.CeilToInt(_width / (component_width + inbetween_space));
    float fixed_space = (_width - _components_per_floor * component_width) / (_components_per_floor + 1);

    for (int floor = 0; floor < _parent_building.floorNumber; ++floor)
    {
      float offset = fixed_space;
      for (int i = 0; i < _components_per_floor; ++i)
      {
        Vector3 dr = _boundaries[0] - _right * offset + (new Vector3(0f, floor * _parent_building.floorHeight, 0f));
        Vector3 dl = dr - _right * component_width;
        offset += component_width;
        _face_components.Add(new FaceComponent(this, dr, dl, 7f / 10f));
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
  public Vector3[] FindVertices ()
  {
    _vertices = new Vector3[4 * _components_per_floor * (_parent_building.floorNumber + 1)];
    if (_components_per_floor == 0) return _vertices;
    _vertices_per_row = 2 * _components_per_floor;
    int index = 0;

    // a modifier to calculate the correct index of the vertices of the roof edge
    // given the respective index of the vertex of the ground edge of a face.
    // for example on a face of one floor and 2 face components this number will be 12
    // 2 floors and 3 components per floor this will give 20.
    _index_modifier = 2 * _components_per_floor * (2 * _parent_building.floorNumber + 1);

    for (int i = 0; i < _components_per_floor; ++i)
    {
      _vertices[index] = new Vector3(_face_components[i].boundaries[0].x,
                                     _parent_building.boundariesArray[0].y,
                                     _face_components[i].boundaries[0].z);

      _vertices[index + 1] = new Vector3(_face_components[i].boundaries[1].x,
                                         _parent_building.boundariesArray[0].y,
                                         _face_components[i].boundaries[1].z);

      _vertices[index + _index_modifier] = new Vector3(_face_components[i].boundaries[0].x,
                                                       _parent_building.height,
                                                       _face_components[i].boundaries[0].z);

      _vertices[index + _index_modifier + 1] = new Vector3(_face_components[i].boundaries[1].x,
                                                           _parent_building.height,
                                                           _face_components[i].boundaries[1].z);

      index += 2;
    }

    foreach (FaceComponent fc in _face_components)
    {
      _vertices[index]     = fc.boundaries[0];
      _vertices[index + 1] = fc.boundaries[1];

      _vertices[index + _vertices_per_row]     = fc.boundaries[3];
      _vertices[index + _vertices_per_row + 1] = fc.boundaries[2];

      if ((index += 2) % _vertices_per_row == 0)
        index += _vertices_per_row;
    }

    return _vertices;
  }
}
