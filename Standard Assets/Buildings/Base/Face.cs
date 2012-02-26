using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A class that represents a face of a building.
/// </summary>
public class Face
{
  /*************** FIELDS ***************/

  /// <summary>
  /// The right vector of the face. Helps with the calculation of components placement.
  /// </summary>
  protected Vector3 _right;

  /// <summary>
  /// The width of the face.
  /// </summary>
  protected float _width;

  /// <summary>
  /// The boundaries of the face.
  /// </summary>
  protected List<Vector3> _boundaries = new List<Vector3>();

  /// <summary>
  /// A list containing all of the components attached to this face.
  /// </summary>
  protected List<FaceComponent> _face_components = new List<FaceComponent>();

  /// <summary>
  /// The number of the components on each floor.
  /// </summary>
  protected int _components_per_floor = 0;

  /// <summary>
  /// The normal vector of the face's surface.
  /// </summary>
  private Vector3 _normal;

  /// <summary>
  /// An array containing all vertices of the attached components.
  /// </summary>
  private Vector3[] _vertices;

  /// <summary>
  /// A modifier to calculate the correct index of the vertices of the roof edge.
  /// </summary>
  /// <description>
  /// given the respective index of the vertex of the ground edge of a face.
  /// For example on a face of one floor and 2 face components this number will be 12.
  /// On a face of 2 floors and 3 components per floor this will give 20.
  /// </description>
  private int _index_modifier = 0;

  /// <summary>
  /// Stores how many vertices there are in one "row".
  /// </summary>
  /// <description>
  /// This number is the sum of the components on each floor times two.
  /// Helps with the calculation of the vertices' indexes and triangles.
  /// </description>
  private int _vertices_per_row = 0;

  /// <summary>
  /// The building that has this face.
  /// </summary>
  private readonly Building _parent_building;

  
  /*************** PROPERTIES ***************/

  /// <summary>
  /// Gets the parent building.
  /// </summary>
  public Building parentBuilding
  {
    get { return _parent_building; }
  }

  /// <summary>
  /// Gets the array of vertices.
  /// </summary>
  public Vector3[] vertices
  {
    get { return _vertices; }
  }

  /// <summary>
  /// A modifier to calculate the correct index of the vertices of the roof edge.
  /// </summary>
  public int indexModifier
  {
    get { return _index_modifier; }
  }

  /// <summary>
  /// Gets the number of vertices in a row of components' top or bottom edges.
  /// </summary>
  public int verticesPerRow
  {
    get { return _vertices_per_row; }
  }

  /// <summary>
  /// The number of components per floor.
  /// </summary>
  public int componentsPerFloor
  {
    get { return _components_per_floor; }
    protected set { _components_per_floor = value; }
  }

  /// <summary>
  /// The width of the face.
  /// </summary>
  public float width
  {
    get { return _width; }
  }
  
  
  /*************** CONSTRUCTORS ***************/
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Face"/> class from the given points,
  /// in clockwise order. The clockwise order is required so that the normal of this face
  /// is properly calculated.
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
  
  
  /*************** METHODS ***************/
  
  public virtual void ConstructFaceComponents (float component_width, float inbetween_space)
  {
//    _components_per_floor = Mathf.CeilToInt(_width / (component_width + inbetween_space));
//    float fixed_space = (_width - _components_per_floor * component_width) / (_components_per_floor + 1);
//
//    for (int floor = 0; floor < _parent_building.floorNumber; ++floor)
//    {
//      float offset = fixed_space;
//      for (int i = 0; i < _components_per_floor; ++i)
//      {
//        Vector3 dr = _boundaries[0] - _right * offset + (new Vector3(0f, floor * _parent_building.floorHeight, 0f));
//        Vector3 dl = dr - _right * component_width;
//        offset += component_width;
//        _face_components.Add(new FaceComponent(this, dr, dl, 7f / 10f));
//        offset += fixed_space;
//      }
//    }
  }

  /// <summary>
  /// Creates an array that contains the vertices of the FaceComponents
  /// attached to this Face. The vertices are put in an order so that it
  /// will be easy to form the triangles of the mesh.
  /// </summary>
  /// <returns>
  /// The vertices array.
  /// </returns>
  public Vector3[] FindVertices ()
  {
    // calculate the size of the array of vertices
    // in case the face has 0 components return the array with 0 Length
    _vertices = new Vector3[4 * _components_per_floor * (_parent_building.floorNumber + 1)];
    if (_components_per_floor == 0) return _vertices;

    // store the required vertices on faces top and bottom edges
    // this is necessary in order to form the required triangles
    _index_modifier = 2 * _components_per_floor * (2 * _parent_building.floorNumber + 1);
    int index = 0;
    for (int i = 0; i < _components_per_floor; ++i)
    {
      // bottom edge
      _vertices[index] = new Vector3(_face_components[i].boundaries[0].x,
                                     _parent_building.boundariesArray[0].y,
                                     _face_components[i].boundaries[0].z);

      _vertices[index + 1] = new Vector3(_face_components[i].boundaries[1].x,
                                         _parent_building.boundariesArray[0].y,
                                         _face_components[i].boundaries[1].z);

      // top edge
      _vertices[index + _index_modifier] = new Vector3(_face_components[i].boundaries[0].x,
                                                       _parent_building.height,
                                                       _face_components[i].boundaries[0].z);

      _vertices[index + _index_modifier + 1] = new Vector3(_face_components[i].boundaries[1].x,
                                                           _parent_building.height,
                                                           _face_components[i].boundaries[1].z);

      index += 2;
    }

    // store the vertices of the components attached to this face
    _vertices_per_row = 2 * _components_per_floor;
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
