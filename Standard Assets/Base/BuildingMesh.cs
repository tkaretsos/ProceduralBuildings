using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public class BuildingMesh : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly Building parent;

  public List<Face> faces = new List<Face>();

  private int _floorCount = 0;
  public int floorCount
  {
    get { return _floorCount; }
    set
    {
      _floorCount = value;
      if (_floorHeight > 0f)
        height = _floorHeight * _floorCount;
    }
  }

  private float _floorHeight = 0f;
  public float floorHeight
  {
    get { return _floorHeight; }
    set
    {
      _floorHeight = value;
      if (_floorCount > 0)
        height = _floorHeight * _floorCount;
    }
  }

  public float height = 0f;

  /// <summary>
  /// Stores the indexes of faces in sorted order.
  /// </summary>
  public int[] sortedFaces;
  
  /*************** CONSTRUCTORS ***************/

  public BuildingMesh (Building parent)
  {
    this.parent = parent;
  }

  /*************** METHODS ***************/

  public virtual void ConstructFaces ()
  {
    throw new System.NotImplementedException();
  }

  public override void FindVertices ()
  {
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

    // copy the vertices of the faces to this.vertices
    // index starts from 4 because of roof vertices
    int index = 4;
    for (int i = 0; i < 4; ++i)
    {
      System.Array.Copy(faces[i].vertices, 0, vertices, index, faces[i].vertices.Length);
      index += faces[i].vertices.Length;
    }
  }

  public override void FindTriangles ()
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
        triangles[tris_index++] = offset + 2;

        triangles[tris_index++] = offset;
        triangles[tris_index++] = offset + 2;
        triangles[tris_index++] = offset + 3;
      }
      else
      {
        for (int floor = 0; floor < floorCount; ++floor)
        {
          int fixedOffset = offset + faces[face].edgeVerticesCount + 8 * faces[face].componentsPerFloor * floor;
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
  }

  public override void Draw ()
  {
    base.Draw();

    foreach (Base.Face face in faces)
      foreach (Base.FaceComponent component in face.faceComponents)
        component.Draw();

    gameObject.transform.position = meshOrigin;
    gameObject.transform.parent = parent.gameObject.transform;
  }

  /// <summary>
  /// Sorts the faces of the building by width.
  /// </summary>
  public void SortFaces (bool descending = true)
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

    sortedFaces = new int[lkv.Count];
    for (int i = 0; i < lkv.Count; ++i)
      sortedFaces[i] = lkv[i].Key;
  }
}

} // namespace Base
} // namespace Thesis