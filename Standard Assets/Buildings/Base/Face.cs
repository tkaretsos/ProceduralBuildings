using System;
using UnityEngine;
using System.Collections.Generic;

namespace Base {

/// <summary>
/// A class that represents a face of a building.
/// </summary>
public class Face
{
  /*************** FIELDS ***************/

  /// <summary>
  /// The normal vector of the face's surface.
  /// </summary>
  private Vector3 _normal;
  public Vector3 normal { get { return _normal; } }

  /// <summary>
  /// The right vector of the face. Helps with the calculation of components placement.
  /// </summary>
  private Vector3 _right;
  public Vector3 right { get { return _right;  } }

  /// <summary>
  /// The width of the face.
  /// </summary>
  public float width;

  /// <summary>
  /// The boundaries of the face.
  /// </summary>
  public List<Vector3> boundaries = new List<Vector3>();

  /// <summary>
  /// A list containing all of the components attached to this face.
  /// </summary>
  public List<FaceComponent> faceComponents = new List<FaceComponent>();

  /// <summary>
  /// The number of the components on each floor.
  /// </summary>
  public int componentsPerFloor = 0;

  /// <summary>
  /// An array containing all vertices of the attached components.
  /// </summary>
  public Vector3[] vertices;

  public List<int> triangles = new List<int>();
  
  /// <summary>
  /// Stores how many vertices there are in one "row".
  /// </summary>
  /// <description>
  /// This number is the sum of the components on each floor times two.
  /// Helps with the calculation of the vertices' indexes and triangles.
  /// </description>
  public int verticesPerRow = 0;

  /// <summary>
  /// The building that has this face.
  /// </summary>
  public readonly Building parentBuilding;

  public int verticesModifier;

  public Dictionary<int, Type> pattern = new Dictionary<int, Type>();

  public List<int> doorIndexes = new List<int>();

  public List<int> balconyIndexes = new List<int>();
  
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
    parentBuilding = parent;
  
    boundaries.Add(dr);
    boundaries.Add(dl);
    boundaries.Add(new Vector3(dl.x, dl.y + parentBuilding.height, dl.z));
    boundaries.Add(new Vector3(dr.x, dr.y + parentBuilding.height, dr.z));
  
    _right = new Vector3(boundaries[0].x - boundaries[1].x,
                         0f,
                         boundaries[0].z - boundaries[1].z);
    width = _right.magnitude;
    _normal = Vector3.Cross(Vector3.up, _right);

    _right.Normalize();
    _normal.Normalize();
  }
  
  
  /*************** METHODS ***************/
  
  public virtual void ConstructFaceComponents (float component_width, float inbetween_space) {}

  /// <summary>
  /// Creates an array that contains the vertices of the FaceComponents
  /// attached to this Face. The vertices are put in an order so that it
  /// will be easy to form the triangles of the mesh.
  /// </summary>
  /// <returns>
  /// The vertices array.
  /// </returns>
  public void FindVertices ()
  {
    // calculate the size of the array of vertices
    // in case the face has 0 components return the array with 0 Length
    verticesModifier = 2 * (parentBuilding.floorNumber + 1);
    vertices = new Vector3[8 * componentsPerFloor * parentBuilding.floorNumber + verticesModifier];

    for (int i = 0; i < verticesModifier; i += 2)
    {
      vertices[i] = new Vector3(boundaries[0].x,
                                boundaries[0].y + (i / 2) * parentBuilding.floorHeight,
                                boundaries[0].z);

      vertices[i + 1] = new Vector3(boundaries[1].x,
                                    boundaries[1].y + (i / 2) * parentBuilding.floorHeight,
                                    boundaries[1].z);
    }

    if (componentsPerFloor == 0) return;

    int double_cpf = 2 * componentsPerFloor;
    for (var floor = 1; floor <= parentBuilding.floorNumber; ++floor)
      for (var cp = 0; cp < componentsPerFloor; ++cp)
      {
        int cpn = cp + componentsPerFloor * (floor - 1);
        int indexModifier = (floor - 1) * 8 * componentsPerFloor + 2 * cp + verticesModifier;

        vertices[indexModifier] = new Vector3(faceComponents[cpn].boundaries[0].x,
                                              parentBuilding.boundaries[0].y + (floor - 1) * parentBuilding.floorHeight,
                                              faceComponents[cpn].boundaries[0].z);

        vertices[indexModifier + 1] = new Vector3(faceComponents[cpn].boundaries[1].x,
                                                  parentBuilding.boundaries[0].y + (floor - 1) * parentBuilding.floorHeight,
                                                  faceComponents[cpn].boundaries[1].z);

        vertices[indexModifier + double_cpf] = faceComponents[cpn].boundaries[0];

        vertices[indexModifier + double_cpf + 1] = faceComponents[cpn].boundaries[1];

        vertices[indexModifier + double_cpf * 2] = faceComponents[cpn].boundaries[3];

        vertices[indexModifier + double_cpf * 2 + 1] = faceComponents[cpn].boundaries[2];

        vertices[indexModifier + double_cpf * 3] = new Vector3(faceComponents[cpn].boundaries[3].x,
                                                               floor * parentBuilding.floorHeight,
                                                               faceComponents[cpn].boundaries[3].z);

        vertices[indexModifier + double_cpf * 3 + 1] = new Vector3(faceComponents[cpn].boundaries[2].x,
                                                                   floor * parentBuilding.floorHeight,
                                                                   faceComponents[cpn].boundaries[2].z);
      }
  }

  public ComponentCoordinate IndexToCoordinate (int index)
  {
    return new ComponentCoordinate(index / parentBuilding.floorNumber, 
                                   index % parentBuilding.floorNumber);
  }
}

} // namespace Base
