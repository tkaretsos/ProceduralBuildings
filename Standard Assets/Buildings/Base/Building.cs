using UnityEngine;
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
  /// A list that contains the triangles that form the building.
  /// </summary>
  public List<int> triangles = new List<int>();

  /// <summary>
  /// A list that contains all the vertices of the building.
  /// </summary>
  public List<Vector3> vertices = new List<Vector3>();

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
  private int _floorNumber = 0;
  public int floorNumber
  {
    get { return _floorNumber; }
    set
    {
      _floorNumber = value;
      if (_floorHeight > 0f)
      {
        _height = _floorHeight * _floorNumber;
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
      if (_floorNumber > 0)
      {
        _height = _floorHeight * _floorNumber;
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
    : base("building", "BuildingMaterial", false)
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
  public override Vector3[] FindVertices ()
  {
    if (boundaries.Count != 8) throw new Exception("Building doesnt have enough boundaries.");

    for (var i = 0; i < 4; ++i)
      vertices.Add(boundaries[i]);

    for (var floor = 1; floor < floorNumber; ++floor)
      for (var i = 0; i < 4; ++i)
        vertices.Add(new Vector3(
                            boundaries[i].x,
                            floor * floorHeight,
                            boundaries[i].z
                    ));

    for (var i = 4; i < 8; ++i)
      vertices.Add(boundaries[i]);

    foreach (Face face in faces)
      vertices.AddRange(face.FindVertices());

    return vertices.ToArray();
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
  public override int[] FindTriangles ()
  {
    var offset = 4 * (floorNumber + 1);
    var floorNoX4 = 4 * floorNumber;

    //roof
    triangles.Add(offset - 4);
    triangles.Add(offset - 3);
    triangles.Add(offset - 2);

    triangles.Add(offset - 4);
    triangles.Add(offset - 2);
    triangles.Add(offset - 1);

    for (var face = 0; face < 4; ++face)
    {
      var face1_mod4 = (face + 1) % 4;

      if (faces[face].componentsPerFloor == 0)
      {
        triangles.Add(face);
        triangles.Add(face1_mod4);
        triangles.Add(face + floorNoX4);

        triangles.Add(face + floorNoX4);
        triangles.Add(face1_mod4);
        triangles.Add(face1_mod4 + floorNoX4);

        continue;
      }

      for (var floor = 0; floor < floorNumber; ++floor)
      {
        // wall right and left of components

        // wall between most right component (looking at the visible side of the face) 
        // and the face edge
        triangles.Add(face + floor * 4);
        triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor);
        triangles.Add(face + floor * 4 + 4);

        triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor);
        triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);
        triangles.Add(face + floor * 4 + 4);

        // wall between each component
        var index = offset + 1;
        for (var i = 1; i < faces[face].componentsPerFloor; ++i)
        {
          triangles.Add(index + floor * 8 * faces[face].componentsPerFloor);
          triangles.Add(index + 1 + floor * 8 * faces[face].componentsPerFloor);
          triangles.Add(index + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);

          triangles.Add(index + 1 + floor * 8 * faces[face].componentsPerFloor);
          triangles.Add(index + 1 + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);
          triangles.Add(index + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);
          
          index += 2;
        }

        // wall between most left component (looking at the visible side of the face) 
        // and the face edge
        triangles.Add(index + floor * 8 * faces[face].componentsPerFloor);
        triangles.Add(face1_mod4 + floor * 4);
        triangles.Add(index + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);

        triangles.Add(face1_mod4 + floor * 4);
        triangles.Add(face1_mod4 + floor * 4 + 4);
        triangles.Add(index + floor * 8 * faces[face].componentsPerFloor + 6 * faces[face].componentsPerFloor);

        // wall over and under each component
        for (var i = 0; i < faces[face].componentsPerFloor; ++i)
        {
          // under
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 1 + 2 * i);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * faces[face].componentsPerFloor + 2 * i);

          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 1 + 2 * i);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 1 + 2 * i + 2 * faces[face].componentsPerFloor);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * faces[face].componentsPerFloor + 2 * i);

          // over
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 4 * faces[face].componentsPerFloor);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 4 * faces[face].componentsPerFloor + 1);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 6 * faces[face].componentsPerFloor);

          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 4 * faces[face].componentsPerFloor + 1);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 6 * faces[face].componentsPerFloor + 1);
          triangles.Add(offset + floor * 8 * faces[face].componentsPerFloor + 2 * i + 6 * faces[face].componentsPerFloor);
        }
      }

      offset += faces[face].vertices.Length;
    }

    // roof
    //triangles.Add(4); triangles.Add(5); triangles.Add(6);
    //triangles.Add(4); triangles.Add(6); triangles.Add(7);

    //var offset = 8;
    //for (var face = 0; face < 4; ++face)
    //{
    //  var face1_mod_4 = (face + 1) % 4;

    //  if (faces[face].componentsPerFloor == 0)
    //  {
    //    triangles.Add(face);
    //    triangles.Add(face1_mod_4);
    //    triangles.Add(face + 4);

    //    triangles.Add(face + 4);
    //    triangles.Add(face1_mod_4);
    //    triangles.Add(face1_mod_4 + 4);

    //    continue;
    //  }

      // wall between components and edges
      //triangles.Add(face);
      //triangles.Add(offset);
      //triangles.Add(face + 4);

      //triangles.Add(offset);
      //triangles.Add(offset + faces[face].indexModifier);
      //triangles.Add(face + 4);

      //triangles.Add(offset + faces[face].verticesPerRow - 1);
      //triangles.Add(face1_mod_4);
      //triangles.Add(face1_mod_4 + 4);

      //triangles.Add(offset + faces[face].verticesPerRow - 1);
      //triangles.Add(face1_mod_4 + 4);
      //triangles.Add(offset + faces[face].verticesPerRow - 1 + faces[face].indexModifier);

      // wall between components (from ground to roof)
      //var index = offset + 1;
      //for (var i = 1; i < faces[face].componentsPerFloor; ++i)
      //{
      //  triangles.Add(index);
      //  triangles.Add(index + 1);
      //  triangles.Add(index + faces[face].indexModifier);

      //  triangles.Add(index + 1);
      //  triangles.Add(index + 1 + faces[face].indexModifier);
      //  triangles.Add(index + faces[face].indexModifier);

      //  index += 2;
      //}

      // wall inbetween components
      //for (var i = 0; i < faces[face].componentsPerFloor; ++i)
      //  for (var j = 0; j <= floorNumber; ++j)
      //  {
      //    var adjustment = 2 * (i + j * faces[face].verticesPerRow) + offset;

      //    triangles.Add(adjustment);
      //    triangles.Add(adjustment + 1);
      //    triangles.Add(adjustment + faces[face].verticesPerRow);

      //    triangles.Add(adjustment + faces[face].verticesPerRow);
      //    triangles.Add(adjustment + 1);
      //    triangles.Add(adjustment + 1 + faces[face].verticesPerRow);
      //  }

    //  offset += faces[face].vertices.Length;
    //}

    return triangles.ToArray();
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
      lkv.Sort(delegate(KeyValuePair<int, float> x, KeyValuePair<int, float> y)
      {
        return y.Value.CompareTo(x.Value);
      });
    else
      lkv.Sort(delegate(KeyValuePair<int, float> x, KeyValuePair<int, float> y)
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

  /// <summary>
  /// Combines all the WindowFrame objects of the building
  /// into one mesh. The WindowFrame objects are then destroyed.
  /// </summary>
  public void CombineWindowFrames ()
  {
    windowFrameCombiner = new GameObject("window_frame_combiner");
    windowFrameCombiner.transform.parent = transform;
    windowFrameCombiner.active = false;
    var meshFilter = windowFrameCombiner.AddComponent<MeshFilter>();
    var meshRenderer = windowFrameCombiner.AddComponent<MeshRenderer>();
    meshRenderer.sharedMaterial = Resources.Load("Materials/WindowFrameMaterial",
                                                 typeof(Material)) as Material;

    List<Window> windows = new List<Window>();
    foreach (Base.Face face in faces)
      foreach (Base.FaceComponent fc in face.faceComponents)
        if (fc.GetType().IsSubclassOf(typeof(Window)))
          windows.Add(fc as Window);

    MeshFilter[] meshFilters = new MeshFilter[windows.Count];
    for (var i = 0; i < windows.Count; ++i)
    {
      meshFilters[i] = windows[i].windowFrame.meshFilter;
      GameObject.Destroy(windows[i].windowFrame.gameObject);
    }

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for (var i = 0; i < meshFilters.Length; ++i)
    {
      combine[i].mesh = meshFilters[i].sharedMesh;
      combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    meshFilter.mesh = new Mesh();
    meshFilter.mesh.CombineMeshes(combine);
  }

  public void CombineWindowGlasses ()
  {
    windowGlassCombiner = new GameObject("window_glass_combiner");
    windowGlassCombiner.transform.parent = transform;
    windowGlassCombiner.active = false;
    var meshFilter = windowGlassCombiner.AddComponent<MeshFilter>();
    var meshRenderer = windowGlassCombiner.AddComponent<MeshRenderer>();
    meshRenderer.sharedMaterial = Resources.Load("Materials/WindowGlassMaterial",
                                                 typeof(Material)) as Material;

    List<Window> windows = new List<Window>();
    foreach (Base.Face face in faces)
      foreach (Base.FaceComponent fc in face.faceComponents)
        if (fc.GetType().IsSubclassOf(typeof(Window)))
          windows.Add(fc as Window);

    MeshFilter[] meshFilters = new MeshFilter[windows.Count];
    for (var i = 0; i < windows.Count; ++i)
    {
      meshFilters[i] = windows[i].windowGlass.meshFilter;
      GameObject.Destroy(windows[i].windowGlass.gameObject);
    }

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for (var i = 0; i < meshFilters.Length; ++i)
    {
      combine[i].mesh = meshFilters[i].sharedMesh;
      combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    meshFilter.mesh = new Mesh();
    meshFilter.mesh.CombineMeshes(combine);
  }
}

} // namespace Base
