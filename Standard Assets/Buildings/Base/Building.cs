using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Exception = System.Exception;

namespace Base {

public class Building
{
  /*************** FIELDS ***************/

  /// <summary>
  /// The number of floors of the building.
  /// </summary>
  protected int _floor_number = 0;

  /// <summary>
  /// The height of each floor.
  /// </summary>
  protected float _floor_height = 0f;

  /// <summary>
  /// A list containing the faces of this building.
  /// </summary>
  protected List<Face> _faces = new List<Face>();

  /// <summary>
  /// The ground and roof boundaries (vertices) of the building.
  /// </summary>
  protected List<Vector3> _boundaries   = new List<Vector3>();

  /// <summary>
  /// The total height of the building.
  /// </summary>
  private float _height = 0f;

  /// <summary>
  /// A list that contains the triangles that form the building.
  /// </summary>
  private List<int> _triangles = new List<int>();

  /// <summary>
  /// A list that contains all the vertices of the building.
  /// </summary>
  private List<Vector3> _vertices;

  /// <summary>
  /// A flag that tells whether the building has door(s) or not.
  /// </summary>
  private bool _has_door = false;

  /// <summary>
  /// The gameObject which is responsible for the rendering of the building.
  /// </summary>
  private GameObject _game_object;

  /// <summary>
  /// The material which will be used for the rendering.
  /// </summary>
  private Material _material;


  /*************** PROPERTIES ***************/
  
  /// <summary>
  /// The total height of the building.
  /// </summary>
  public float height
  {
    get { return _height; }
  }
  
  /// <summary>
  /// The height of each floor.
  /// </summary>
  public float floorHeight
  {
    get { return _floor_height; }
    protected set
    {
      _floor_height = value;
      if (_floor_number > 0)
      {
        _height = _floor_height * _floor_number;
        CalculateRoofBoundaries();
      }
    }
  }
  
  /// <summary>
  /// The number of floors.
  /// </summary>
  public int floorNumber
  {
    get { return _floor_number; }
    protected set
    {
      _floor_number = value;
      if (_floor_height > 0f)
      {
        _height = _floor_height * _floor_number;
        CalculateRoofBoundaries();
      }
    }
  }

  /// <summary>
  /// The boundaries of this building in array.
  /// </summary>
  public Vector3[] boundariesArray
  {
    get { return _boundaries.ToArray(); }
  }

  /// <summary>
  /// Flag that indicates whether this building has door(s).
  /// </summary>
  public bool hasDoor
  {
    get { return _has_door; }
    set { _has_door = value; }
  }

  /// <summary>
  /// The gameObject of the building, which is responsible for the rendering.
  /// </summary>
  public GameObject gameObject
  {
    get { return _game_object; }
  }

  
  /*************** CONSTRUCTORS ***************/
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Building"/> class.
  /// The given Vector3 points must be given in clockwise order (required
  /// for the correct calculation of the surface's normal).
  /// </summary>
  /// <param name='p1'>
  /// A point in space.
  /// </param>
  /// <param name='p2'>
  /// A point in space.
  /// </param>
  /// <param name='p3'>
  /// A point in space.
  /// </param>
  /// <param name='p4'>
  /// A point in space.
  /// </param>
  /// <param name='type'>
  /// The type of the building.
  /// </param>
  public Building (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Material material)
  {
    _boundaries.Add(p1);
    _boundaries.Add(p2);
    _boundaries.Add(p3);
    _boundaries.Add(p4);

    _material = material;
  }
  
  
  /*************** METHODS ***************/
  
  /// <summary>
  /// Constructs the faces of the building.
  /// </summary>
  public virtual void ConstructFaces ()
  {
    _faces.Add(new Face(this, _boundaries[0], _boundaries[1]));
    _faces.Add(new Face(this, _boundaries[1], _boundaries[2]));
    _faces.Add(new Face(this, _boundaries[2], _boundaries[3]));
    _faces.Add(new Face(this, _boundaries[3], _boundaries[0]));
  }

  /// <summary>
  /// Finds all the vertices required for the rendering of the building.
  /// </summary>
  /// <description>
  /// Actually puts together the building's boundaries and the vertices
  /// of each face of the building.
  /// </description>
  /// <returns>
  /// The vertices of the building in an array.
  /// </returns>
  /// <exception>
  /// An exception is thrown when the boundaries of the building
  /// have not been calculated.
  /// </exception>
  private Vector3[] FindVertices ()
  {
    if (_boundaries.Count != 8) throw new Exception("Building doesnt have enough boundaries.");

    _vertices = _boundaries;
    foreach (Face face in _faces)
      _vertices.AddRange(face.FindVertices());

    return _vertices.ToArray();
  }

  /// <summary>
  /// Calculates the triangles that are required for the rendering of the building.
  /// </summary>
  /// <description>
  /// The calculations starts by adding the 2 triangles of the roof.
  /// Then the triangles of each face are added. For each face the triangles
  /// are added in a vertical manner. Firstly, the triangles between the top/bottom
  /// edges and the respective components are added. Then the triangles between top
  /// and bottom edges are added (long vertical stripes). Finally, the triangles
  /// between each component and its adjucent ones (top or/and bottom) are added.
  /// </description>
  private int[] FindTriangles ()
  {
    // roof
    _triangles.Add(4); _triangles.Add(5); _triangles.Add(6);
    _triangles.Add(4); _triangles.Add(6); _triangles.Add(7);

    int offset = 8;
    for (int face = 0; face < 4; ++face)
    {
      int face1_mod_4 = (face + 1) % 4;

      if (_faces[face].componentsPerFloor == 0)
      {
        _triangles.Add(face);
        _triangles.Add(face1_mod_4);
        _triangles.Add(face + 4);

        _triangles.Add(face + 4);
        _triangles.Add(face1_mod_4);
        _triangles.Add(face1_mod_4 + 4);

        continue;
      }

      // wall between components and edges
      _triangles.Add(face);
      _triangles.Add(offset);
      _triangles.Add(face + 4);

      _triangles.Add(offset);
      _triangles.Add(offset + _faces[face].indexModifier);
      _triangles.Add(face + 4);

      _triangles.Add(offset + _faces[face].verticesPerRow - 1);
      _triangles.Add(face1_mod_4);
      _triangles.Add(face1_mod_4 + 4);

      _triangles.Add(offset + _faces[face].verticesPerRow - 1);
      _triangles.Add(face1_mod_4 + 4);
      _triangles.Add(offset + _faces[face].verticesPerRow - 1 + _faces[face].indexModifier);

      // wall between components (from ground to roof)
      int index = offset + 1;
      for (int i = 1; i < _faces[face].componentsPerFloor; ++i)
      {
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + _faces[face].indexModifier);

        _triangles.Add(index + 1);
        _triangles.Add(index + 1 + _faces[face].indexModifier);
        _triangles.Add(index + _faces[face].indexModifier);

        index += 2;
      }

      // wall inbetween components
      for (int i = 0; i < _faces[face].componentsPerFloor; ++i)
        for (int j = 0; j <= floorNumber; ++j)
        {
          int adjustment = 2 * (i + j * _faces[face].verticesPerRow) + offset;

          _triangles.Add(adjustment);
          _triangles.Add(adjustment + 1);
          _triangles.Add(adjustment + _faces[face].verticesPerRow);

          _triangles.Add(adjustment + _faces[face].verticesPerRow);
          _triangles.Add(adjustment + 1);
          _triangles.Add(adjustment + 1 + _faces[face].verticesPerRow);
        }

      offset += _faces[face].vertices.Length;
    }

    return _triangles.ToArray();
  }

  /// <summary>
  /// Helper method that calculates the roof boundaries.
  /// </summary>
  private void CalculateRoofBoundaries ()
  {
    if (_boundaries.Count > 4)
      _boundaries.RemoveRange(4, 4);

    for (int i = 0; i < 4; ++i)
      _boundaries.Add(new Vector3(_boundaries[i].x,
                                  _boundaries[i].y + _height,
                                  _boundaries[i].z));
  }

  /// <summary>
  /// Sort the faces of the building by width.
  /// </summary>
  /// <returns>
  /// An array of int that contains the indexes of the faces sorted by width.
  /// </returns>
  /// <param name='descending'>
  /// The order for the sorting. <c>true</c> for descending, <c>false</c> for ascending.
  /// </param>
  public int[] GetSortedFaces (bool descending = true)
  {
    List<KeyValuePair<int, float>> lkv = new List<KeyValuePair<int, float>>();
    for (int i = 0; i < _faces.Count; ++i)
      lkv.Add(new KeyValuePair<int, float>(i, _faces[i].width));

    if (descending)
      lkv.Sort(delegate (KeyValuePair<int, float> x, KeyValuePair<int, float> y) {
        return y.Value.CompareTo(x.Value);
      });
    else
      lkv.Sort(delegate (KeyValuePair<int, float> x, KeyValuePair<int, float> y) {
        return x.Value.CompareTo(y.Value);
      });

    int[] ret = new int[lkv.Count];
    for (int i = 0; i < lkv.Count; ++i)
      ret[i] = lkv[i].Key;

    return ret;
  }

  /// <summary>
  /// Creates a  gameObject that is responsible for the rendering of the building.
  /// </summary>
  public void Render ()
  {
    _game_object = new GameObject();

    MeshRenderer _mesh_renderer = _game_object.AddComponent<MeshRenderer>();
    _mesh_renderer.sharedMaterial = _material;

    Mesh _mesh = new Mesh();
    _mesh.Clear();
    _mesh.vertices = FindVertices();
    _mesh.triangles = FindTriangles();
    // Assign UVs to shut the editor up -_-'
    _mesh.uv = new Vector2[_mesh.vertices.Length];
    for (int i = 0; i < _mesh.vertices.Length; ++i)
      _mesh.uv[i] = new Vector2(_mesh.vertices[i].x, _mesh.vertices[i].y);

    _mesh.RecalculateNormals();
    _mesh.Optimize();

    MeshFilter _mesh_filter = _game_object.AddComponent<MeshFilter>();
    _mesh_filter.sharedMesh = _mesh;
  }
}

} // namespace Base
