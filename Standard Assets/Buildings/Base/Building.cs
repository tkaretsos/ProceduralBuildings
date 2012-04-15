using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Exception = System.Exception;

namespace Base {

public class Building : Drawable
{
  /*************** FIELDS ***************/

  /// <summary>
  /// A list containing the faces of this building.
  /// </summary>
  public List<Face> faces = new List<Face>();

  /// <summary>
  /// The ground and roof boundaries (vertices) of the building.
  /// </summary>
  public List<Vector3> boundaries = new List<Vector3>();

  /// <summary>
  /// An array that contains the triangles that form the building.
  /// </summary>
  int[] triangles;

  /// <summary>
  /// An array that contains all the vertices of the building.
  /// </summary>
  public Vector3[] vertices;

  /// <summary>
  /// A flag that tells whether the building has door(s) or not.
  /// </summary>
  public bool hasDoor = false;

  /// <summary>
  /// The game object that is created after combining all the
  /// building's window frames.
  /// </summary>
  public GameObject windowFrameCombiner;

  /// <summary>
  /// The game object that is created after combining all the
  /// building's window glasses.
  /// </summary>
  public GameObject windowGlassCombiner;

  /// <summary>
  /// The number of floors of the building.
  /// </summary>
  private int _floorCount = 0;
  public int floorCount
  {
    get { return _floorCount; }
    set
    {
      _floorCount = value;
      if (_floorHeight > 0f)
      {
        _height = _floorHeight * _floorCount;
        CalculateRoofBoundaries();
      }
    }
  }

  /// <summary>
  /// The height of each floor.
  /// </summary>
  private float _floorHeight = 0f;
  public float floorHeight
  {
    get { return _floorHeight; }
    set
    {
      _floorHeight = value;
      if (_floorCount > 0)
      {
        _height = _floorHeight * _floorCount;
        CalculateRoofBoundaries();
      }
    }
  }

  /// <summary>
  /// The total height of the building.
  /// </summary>
  private float _height = 0f; 
  public float height { get { return _height; } }

  public int[] sortedFaces;

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
  public Building (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    : base("building", "Building", false)
  {
    FindMeshOrigin(p1, p2, p3, p4);
    boundaries.Add(p1 - meshOrigin);
    boundaries.Add(p2 - meshOrigin);
    boundaries.Add(p3 - meshOrigin);
    boundaries.Add(p4 - meshOrigin);
  }
  
  
  /*************** METHODS ***************/
  
  /// <summary>
  /// Constructs the faces of the building.
  /// </summary>
  public virtual void ConstructFaces ()
  {
    faces.Add(new Face(this, boundaries[0], boundaries[1]));
    faces.Add(new Face(this, boundaries[1], boundaries[2]));
    faces.Add(new Face(this, boundaries[2], boundaries[3]));
    faces.Add(new Face(this, boundaries[3], boundaries[0]));
  }

  /// <summary>
  /// Finds all the vertices required for the rendering of the building.
  /// </summary>
  /// <description>
  /// Puts together the building's boundaries and the vertices
  /// of each face of the building.
  /// </description>
  /// <returns>
  /// The vertices of the building in an array.
  /// </returns>
  /// <exception>
  /// An exception is thrown when the boundaries of the building
  /// have not been calculated.
  /// </exception>
  public override Vector3[] FindVertices ()
  {
    if (boundaries.Count != 8) throw new Exception("Building doesnt have enough boundaries.");

    int vert_count = 0;
    for (int i = 0; i < 4; ++i)
    {
      faces[i].FindVertices();
      vert_count += faces[i].vertices.Length;
    }

    vertices = new Vector3[vert_count + 4];

    // add roof vertices first
    for (int i = 0; i < 4; ++i)
      vertices[i] = boundaries[i + 4];

    // index starts from 4 because of roof vertices
    int index = 4;
    for (int i = 0; i < 4; ++i)
    {
      System.Array.Copy(faces[i].vertices, 0, vertices, index, faces[i].vertices.Length);
      index += faces[i].vertices.Length;
    }

    return vertices;
  }

  /// <summary>
  /// Calculates the triangles that are required for the rendering of the building.
  /// </summary>
  /// <description>
  /// The calculations starts by adding the 2 triangles of the roof.
  /// The tris for the rest building are calculated per face and per floor.
  /// </description>
  public override int[] FindTriangles ()
  {
    // roof tris
    int tris_count = 2;
    for (int i = 0; i < 4; ++i)
      if (faces[i].componentsPerFloor == 0)
        tris_count += 2;
      else
        tris_count += floorCount * (6 * faces[i].componentsPerFloor + 2);

    triangles = new int[tris_count * 3];

    int tris_index = 0;

    int offset = 4;

    // roof
    triangles[tris_index++] = 0;
    triangles[tris_index++] = 1;
    triangles[tris_index++] = 3;

    triangles[tris_index++] = 1;
    triangles[tris_index++] = 2;
    triangles[tris_index++] = 3;

    for (int face = 0; face < 4; ++face)
    {
      if (faces[face].componentsPerFloor == 0)
      {
        triangles[tris_index++] = offset;
        triangles[tris_index++] = offset + 1;
        triangles[tris_index++] = offset + faces[face].verticesModifier - 2;

        triangles[tris_index++] = offset + 1;
        triangles[tris_index++] = offset + faces[face].verticesModifier - 1;
        triangles[tris_index++] = offset + faces[face].verticesModifier - 2;
      }
      else
      {
        for (int floor = 0; floor < floorCount; ++floor)
        {
          int fixedOffset = offset + faces[face].verticesModifier + 8 * faces[face].componentsPerFloor * floor;
          int cpfX6 = 6 * faces[face].componentsPerFloor;
          int floorX2 = 2 * floor;

          triangles[tris_index++] = offset + floorX2;
          triangles[tris_index++] = fixedOffset;
          triangles[tris_index++] = offset + floorX2 + 2;

          triangles[tris_index++] = fixedOffset;
          triangles[tris_index++] = fixedOffset + cpfX6;
          triangles[tris_index++] = offset + floorX2 + 2;

          // wall between each component
          int index = fixedOffset + 1;
          for (int i = 1; i < faces[face].componentsPerFloor; ++i)
          {
            triangles[tris_index++] = index;
            triangles[tris_index++] = index + 1;
            triangles[tris_index++] = index + cpfX6;

            triangles[tris_index++] = index + 1;
            triangles[tris_index++] = index + cpfX6 + 1;
            triangles[tris_index++] = index + cpfX6;

            index += 2;
          }

          triangles[tris_index++] = index;
          triangles[tris_index++] = offset + floorX2 + 1;
          triangles[tris_index++] = index + cpfX6;

          triangles[tris_index++] = offset + floorX2 + 1;
          triangles[tris_index++] = offset + floorX2 + 3;
          triangles[tris_index++] = index + cpfX6;

          // wall over and under each component
          for (int i = 0; i < faces[face].componentsPerFloor; ++i)
          {
            int extOffset = fixedOffset + 2 * i;

            // under
            triangles[tris_index++] = extOffset;
            triangles[tris_index++] = extOffset + 1;
            triangles[tris_index++] = extOffset + 2 * faces[face].componentsPerFloor;

            triangles[tris_index++] = extOffset + 1;
            triangles[tris_index++] = extOffset + 2 * faces[face].componentsPerFloor + 1;
            triangles[tris_index++] = extOffset + 2 * faces[face].componentsPerFloor;

            // over
            triangles[tris_index++] = extOffset + 4 * faces[face].componentsPerFloor;
            triangles[tris_index++] = extOffset + 4 * faces[face].componentsPerFloor + 1;
            triangles[tris_index++] = extOffset + cpfX6;

            triangles[tris_index++] = extOffset + 4 * faces[face].componentsPerFloor + 1;
            triangles[tris_index++] = extOffset + cpfX6 + 1;
            triangles[tris_index++] = extOffset + cpfX6;
          }
        }
      }

      offset += faces[face].vertices.Length;
    }

    return triangles;
  }

  /// <summary>
  /// Creates a  gameObject that is responsible for the rendering of the building.
  /// </summary>
  public override void Draw ()
  {
    base.Draw();

    foreach (Base.Face face in faces)
      foreach (Base.FaceComponent component in face.faceComponents)
        component.Draw();
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
    for (int i = 0; i < faces.Count; ++i)
      lkv.Add(new KeyValuePair<int, float>(i, faces[i].width));

    if (descending)
      lkv.Sort(delegate (KeyValuePair<int, float> x, KeyValuePair<int, float> y)
      {
        return y.Value.CompareTo(x.Value);
      });
    else
      lkv.Sort(delegate (KeyValuePair<int, float> x, KeyValuePair<int, float> y)
      {
        return x.Value.CompareTo(y.Value);
      });

    int[] ret = new int[lkv.Count];
    for (int i = 0; i < lkv.Count; ++i)
      ret[i] = lkv[i].Key;

    return ret;
  }

  /// <summary>
  /// Calculates the center of the quadrangle base of the building.
  /// Used for properly creating the gameObject and serves as the origin
  /// of the created mesh.
  /// </summary>
  /// <returns>The origin of the building gameObject's mesh</returns>
  public Vector3 FindMeshOrigin (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
  {
    var par = (p2.x - p0.x) * (p1.z - p3.z) - (p2.z - p0.z) * (p1.x - p3.x);

    var ar_x = (p2.x * p0.z - p2.z * p0.x) * (p1.x - p3.x) -
               (p2.x - p0.x) * (p1.x * p3.z - p1.z * p3.x);

    var ar_z = (p2.x * p0.z - p2.z * p0.x) * (p1.z - p3.z) -
               (p2.z - p0.z) * (p1.x * p3.z - p1.z * p3.x);

    meshOrigin = new Vector3(ar_x / par, 0f, ar_z / par);
    return meshOrigin;
  }

  /// <summary>
  /// Helper method that calculates the roof boundaries.
  /// </summary>
  private void CalculateRoofBoundaries ()
  {
    if (boundaries.Count > 4)
      boundaries.RemoveRange(4, 4);

    for (int i = 0; i < 4; ++i)
      boundaries.Add(new Vector3(boundaries[i].x,
                                 boundaries[i].y + _height,
                                 boundaries[i].z));
  }
}

} // namespace Base
