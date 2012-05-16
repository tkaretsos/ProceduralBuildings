using UnityEngine;
using System.Collections.Generic;

namespace Thesis {

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

  private const float _componentWidthMin = 1.1f;
  private const float _componentWidthMax = 1.25f;
  private const float _componentSpaceMin = 2f;
  private const float _componentSpaceMax = 2.25f;

  public float windowHeight;

  public float doorHeight;

  public float balconyHeight;

  public float balconyFloorHeight;

  public float balconyFloorWidth;

  public float balconyFloorDepth;

  public Roof roof;

  public RoofBase roofBase;
  
  /*************** CONSTRUCTORS ***************/

  public BuildingMesh (Building parent, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    this.parent = parent;
    name = "neo_building_mesh";
    material = MaterialManager.Instance.Get("Building");

    parent.AddCombinable(material.name, this);

    floorHeight = Random.Range(3.8f, 4f);
    floorCount = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});

    FindMeshOrigin(p1, p3, p2, p4);

    boundaries = new Vector3[8];
    boundaries[0] = p1 - meshOrigin;
    boundaries[1] = p2 - meshOrigin;
    boundaries[2] = p3 - meshOrigin;
    boundaries[3] = p4 - meshOrigin;

    for (int i = 0; i < 4; ++i)
      boundaries[i + 4] = new Vector3(boundaries[i].x,
                                      boundaries[i].y + height,
                                      boundaries[i].z);

    ConstructFaces();
    ConstructFaceComponents();
    ConstructRoof();
  }



  /*************** METHODS ***************/

  public virtual void ConstructFaces ()
  {
    faces.Add(new Face(this, boundaries[0], boundaries[1]));
    faces.Add(new Face(this, boundaries[1], boundaries[2]));
    faces.Add(new Face(this, boundaries[2], boundaries[3]));
    faces.Add(new Face(this, boundaries[3], boundaries[0]));

    SortFaces();
  }

  public void ConstructFaceComponents ()
  {
    windowHeight = Random.Range(1.8f, 2f);
    doorHeight = Random.Range(2.8f, 3f);

    balconyHeight = windowHeight / 2 + floorHeight / 2.5f;
    balconyFloorHeight = 0.2f;
    balconyFloorDepth = 1f;
    balconyFloorWidth = 0.6f;

    float component_width = Random.Range(_componentWidthMin, _componentWidthMax);
    float inbetween_space = Random.Range(_componentSpaceMin, _componentSpaceMax);

    foreach (Face face in faces)
      face.ConstructFaceComponents(component_width, inbetween_space);
  }

  private void ConstructRoof()
  {
    roofBase = new RoofBase(this);
    roofBase.material = MaterialManager.Instance.GetCollection("mat_roof_base")[0];
    parent.AddCombinable(roofBase.material.name, roofBase);

    int n = Util.RollDice(new float[] { 0.33f, 0.33f, 0.34f });

    if (n == 1)
      roof = new FlatRoof(this);
    else if (n == 2)
      roof = new SinglePeakRoof(this);
    else
      roof = new DoublePeakRoof(this);

    roof.material = MaterialManager.Instance.GetCollection("mat_roof")[0];
    parent.AddCombinable(roof.material.name, roof);
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
    int tris_count = 0;
    for (int i = 0; i < 4; ++i)
      if (faces[i].componentsPerFloor == 0)
        tris_count += 2;
      else
        tris_count += floorCount * (6 * faces[i].componentsPerFloor + 2);

    triangles = new int[tris_count * 3];

    // triangles index
    int trin = 0;
    int offset = 4;

    for (int face = 0; face < 4; ++face)
    {
      if (faces[face].componentsPerFloor == 0)
      {
        triangles[trin++] = offset;
        triangles[trin++] = offset + 1;
        triangles[trin++] = offset + 2;

        triangles[trin++] = offset;
        triangles[trin++] = offset + 2;
        triangles[trin++] = offset + 3;
      }
      else
      {
        for (int floor = 0; floor < floorCount; ++floor)
        {
          int fixedOffset = offset + faces[face].edgeVerticesCount + 
                            (faces[face].componentsPerFloor << 3) * floor;
          int cpfX6 = 6 * faces[face].componentsPerFloor;
          int floorX2 = floor << 1;

          triangles[trin++] = offset + floorX2;
          triangles[trin++] = fixedOffset;
          triangles[trin++] = offset + floorX2 + 2;

          triangles[trin++] = fixedOffset;
          triangles[trin++] = fixedOffset + cpfX6;
          triangles[trin++] = offset + floorX2 + 2;

          // wall between each component
          int index = fixedOffset + 1;
          for (int i = 1; i < faces[face].componentsPerFloor; ++i)
          {
            triangles[trin++] = index;
            triangles[trin++] = index + 1;
            triangles[trin++] = index + cpfX6;

            triangles[trin++] = index + 1;
            triangles[trin++] = index + cpfX6 + 1;
            triangles[trin++] = index + cpfX6;

            index += 2;
          }

          triangles[trin++] = index;
          triangles[trin++] = offset + floorX2 + 1;
          triangles[trin++] = index + cpfX6;

          triangles[trin++] = offset + floorX2 + 1;
          triangles[trin++] = offset + floorX2 + 3;
          triangles[trin++] = index + cpfX6;

          // wall over and under each component
          for (int i = 0; i < faces[face].componentsPerFloor; ++i)
          {
            int extOffset = fixedOffset + (i << 1);

            // under
            triangles[trin++] = extOffset;
            triangles[trin++] = extOffset + 1;
            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 1);

            triangles[trin++] = extOffset + 1;
            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 1) + 1;
            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 1);

            // over
            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 2);
            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 2) + 1;
            triangles[trin++] = extOffset + cpfX6;

            triangles[trin++] = extOffset + (faces[face].componentsPerFloor << 2) + 1;
            triangles[trin++] = extOffset + cpfX6 + 1;
            triangles[trin++] = extOffset + cpfX6;
          }
        }
      }

      offset += faces[face].vertices.Length;
    }
  }

  public override void Draw ()
  {
    base.Draw();

    foreach (Face face in faces)
      foreach (FaceComponent component in face.faceComponents)
        component.Draw();

    gameObject.transform.position = meshOrigin;
    gameObject.transform.parent = parent.gameObject.transform;

    roof.FindVertices();
    roof.FindTriangles();
    roof.Draw();

    roofBase.FindVertices();
    roofBase.FindTriangles();
    roofBase.Draw();
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

  public override void Destroy()
  {
    base.Destroy();

    foreach (Face face in faces)
      face.Destroy();

    roof.Destroy();
    roofBase.Destroy();
  }
}

} // namespace Thesis