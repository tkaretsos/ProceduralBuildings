using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Exception = System.Exception;

public class Building
{
  // fields
  
  private float _height = 0f;
  private float _floor_height = 0f;
  private int _floor_number = 0;
#pragma warning disable 0414
  private readonly BuildingType _type;
#pragma warning restore 0414
  private List<Vector3> _vertices;
  private List<int> _triangles;
  private List<Vector3> _boundaries = new List<Vector3>();
  private List<Face> _faces = new List<Face>();
  
  
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
  
  public ReadOnlyCollection<Face> faces
  {
    get { return _faces.AsReadOnly(); }
  }
  
  public Vector3[] boundariesArray
  {
    get { return _boundaries.ToArray(); }
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
  public Building (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, BuildingType type)
  {
    _boundaries.Add(p1);
    _boundaries.Add(p2);
    _boundaries.Add(p3);
    _boundaries.Add(p4);
    _type = type;
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
    foreach (Face face in faces)
    {
      face.FindVertices();
      _vertices.AddRange(face.vertices);
    }

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
      // wall between components and edges
      _triangles.Add(face);
      _triangles.Add(offset);
      _triangles.Add(face + 4);

      _triangles.Add(offset);
      _triangles.Add(offset + faces[face].indexModifier);
      _triangles.Add(face + 4);

      _triangles.Add(offset + faces[face].verticesPerRow - 1);
      _triangles.Add((face + 1) % 4);
      _triangles.Add((face + 1) % 4 + 4);

      _triangles.Add(offset + faces[face].verticesPerRow - 1);
      _triangles.Add((face + 1) % 4 + 4);
      _triangles.Add(offset + faces[face].verticesPerRow - 1 + faces[face].indexModifier);

      // wall between components (from ground to roof)
      int index = 1;
      for (int i = 1; i < faces[face].componentsPerFloor; ++i)
      {
        _triangles.Add(offset + index);
        _triangles.Add(offset + index + 1);
        _triangles.Add(offset + index + faces[face].indexModifier);

        _triangles.Add(offset + index + 1);
        _triangles.Add(offset + index + 1 + faces[face].indexModifier);
        _triangles.Add(offset + index + faces[face].indexModifier);

        index += 2;
      }

      // wall inbetween components
      for (int i = 0; i < faces[face].componentsPerFloor; ++i)
      {
        for (int j = 0; j <= floorNumber; ++j)
        {
          _triangles.Add(offset + j * faces[face].verticesPerRow * 2 + i * 2);
          _triangles.Add(offset + 1 + j * faces[face].verticesPerRow * 2 + i * 2);
          _triangles.Add(offset + faces[face].verticesPerRow + j * faces[face].verticesPerRow * 2 + i * 2);

          _triangles.Add(offset + faces[face].verticesPerRow + j * faces[face].verticesPerRow * 2 + i * 2);
          _triangles.Add(offset + 1 + j * faces[face].verticesPerRow * 2 + i * 2);
          _triangles.Add(offset + 1 + faces[face].verticesPerRow + j * faces[face].verticesPerRow * 2 + i * 2);
        }
      }

      offset += faces[face].vertices.Length;
    }

    return _triangles.ToArray();
  }

  private void CalculateRoofBoundaries ()
  {
    if (_boundaries.Count > 4)
      _boundaries.RemoveRange(4,4);

    for (int i = 0; i < 4; ++i)
      _boundaries.Add(new Vector3(_boundaries[i].x,
                                  _boundaries[i].y + _height,
                                  _boundaries[i].z));
  }

  /// <summary>
  /// Draw the current mesh.
  /// </summary>
  /// <param name='material'>
  /// A UnityEngine.Material.
  /// </param>
  public void Draw (Material material)
  {
    material.SetPass(0);

    // draw roof
    GL.PushMatrix();
    GL.Begin(GL.QUADS);
      for (int i = 4; i < 8; ++i)
        GL.Vertex(_boundaries[i]);
    GL.End();
    GL.PopMatrix();
    
    // draw faces
    foreach (Face face in _faces)
      face.Draw();
  }
}
