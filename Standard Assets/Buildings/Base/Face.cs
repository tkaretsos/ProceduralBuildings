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

  /// <summary>
  /// A modifier to calculate the correct index of the vertices of the roof edge.
  /// </summary>
  /// <description>
  /// given the respective index of the vertex of the ground edge of a face.
  /// For example on a face of one floor and 2 face components this number will be 12.
  /// On a face of 2 floors and 3 components per floor this will give 20.
  /// </description>
  public int indexModifier = 0;

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
  public Vector3[] FindVertices ()
  {
    // calculate the size of the array of vertices
    // in case the face has 0 components return the array with 0 Length
    vertices = new Vector3[4 * componentsPerFloor * (parentBuilding.floorNumber + 1)];
    if (componentsPerFloor == 0) return vertices;

    // store the required vertices on faces top and bottom edges
    // this is necessary in order to form the required triangles
    indexModifier = 2 * componentsPerFloor * (2 * parentBuilding.floorNumber + 1);
    int index = 0;
    for (int i = 0; i < componentsPerFloor; ++i)
    {
      // bottom edge
      vertices[index] = new Vector3(faceComponents[i].boundaries[0].x,
                                    parentBuilding.boundaries[0].y,
                                    faceComponents[i].boundaries[0].z);

      vertices[index + 1] = new Vector3(faceComponents[i].boundaries[1].x,
                                        parentBuilding.boundaries[0].y,
                                        faceComponents[i].boundaries[1].z);

      // top edge
      vertices[index + indexModifier] = new Vector3(faceComponents[i].boundaries[0].x,
                                                    parentBuilding.height,
                                                    faceComponents[i].boundaries[0].z);

      vertices[index + indexModifier + 1] = new Vector3(faceComponents[i].boundaries[1].x,
                                                        parentBuilding.height,
                                                        faceComponents[i].boundaries[1].z);

      index += 2;
    }

    // store the vertices of the components attached to this face
    verticesPerRow = 2 * componentsPerFloor;
    foreach (FaceComponent fc in faceComponents)
    {
      vertices[index]     = fc.boundaries[0];
      vertices[index + 1] = fc.boundaries[1];

      vertices[index + verticesPerRow]     = fc.boundaries[3];
      vertices[index + verticesPerRow + 1] = fc.boundaries[2];

      if ((index += 2) % verticesPerRow == 0)
        index += verticesPerRow;
    }

    return vertices;
  }
}

} // namespace Base
