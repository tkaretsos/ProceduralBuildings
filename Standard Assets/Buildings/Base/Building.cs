using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Exception = System.Exception;

public class Building
{
  // fields

  protected int           _floor_number = 0;
  protected float         _floor_height = 0f;
  protected List<Face>    _faces        = new List<Face>();
  protected List<Vector3> _boundaries   = new List<Vector3>();

  private float         _height = 0f;
  private List<int>     _triangles;
  private List<Vector3> _vertices;
  private bool          _has_door = false;

  
  // properties
  
  /// <summary>
  /// Gets the height.
  /// </summary>
  /// <value>
  /// The height.
  /// </value>
  public float height
  {
    get { return _height; }
  }
  
  /// <summary>
  /// Gets or sets the height of the floor.
  /// </summary>
  /// <value>
  /// The height of the floor.
  /// </value>
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
  /// Gets or sets the floor number.
  /// </summary>
  /// <value>
  /// The floor number.
  /// </value>
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
  
  public Vector3[] boundariesArray
  {
    get { return _boundaries.ToArray(); }
  }

  public bool hasDoor
  {
    get { return _has_door; }
    set { _has_door = value; }
  }
  
  
  // constructors
  
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
  public Building (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    _boundaries.Add(p1);
    _boundaries.Add(p2);
    _boundaries.Add(p3);
    _boundaries.Add(p4);
  }
  
  
  // methods
  
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

  public Vector3[] FindVertices ()
  {
    if (_boundaries.Count != 8) throw new Exception("Building doesnt have enough boundaries.");

    _vertices = _boundaries;
    foreach (Face face in _faces)
      _vertices.AddRange(face.FindVertices());

    return _vertices.ToArray();
  }

  public int[] FindTriangles ()
  {
    _triangles = new List<int>();

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
      {
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
      }

      offset += _faces[face].vertices.Length;
    }

    return _triangles.ToArray();
  }

  private void CalculateRoofBoundaries ()
  {
    if (_boundaries.Count > 4)
      _boundaries.RemoveRange(4, 4);

    for (int i = 0; i < 4; ++i)
      _boundaries.Add(new Vector3(_boundaries[i].x,
                                  _boundaries[i].y + _height,
                                  _boundaries[i].z));
  }

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
}
