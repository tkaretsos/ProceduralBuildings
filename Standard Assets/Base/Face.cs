using System;
using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public class Face : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly Building parentBuilding;

  public Vector3 normal;

  public Vector3 right;

  public float width;

  /// <summary>
  /// A list containing all of the components attached to this face.
  /// </summary>
  public List<FaceComponent> faceComponents = new List<FaceComponent>();

  /// <summary>
  /// The number of the components on each floor.
  /// </summary>
  public int componentsPerFloor = 0;
  
  /// <summary>
  /// The sum of the components on each floor times two.
  /// Helps with the calculation of the vertices' indexes and triangles.
  /// </summary>
  public int verticesPerRow = 0;

  /// <summary>
  /// The number we need to add to the index of the vertices of the lowest
  /// (ground) row, in order to get the correct index of the vertices of the
  /// higher (roof) row.
  /// </summary>
  public int verticesModifier;

  /// <summary>
  /// Maps the position (index) of one component to its type.
  /// The indexing starts from the bottom right corner and goes
  /// to the left and then up.
  /// </summary>
  public Dictionary<int, Type> pattern = new Dictionary<int, Type>();

  public List<int> doorIndexes = new List<int>();

  public List<int> balconyIndexes = new List<int>();
  
  /*************** CONSTRUCTORS ***************/
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Face"/> class from the given points,
  /// in clockwise order. The clockwise order is required so that the normal of this face
  /// is properly calculated.
  /// </summary>
  public Face (Building parent, Vector3 dr, Vector3 dl)
  {
    parentBuilding = parent;

    boundaries = new Vector3[4];
    boundaries[0] = dr;
    boundaries[1] = dl;
    boundaries[2] = new Vector3(dl.x, dl.y + parentBuilding.height, dl.z);
    boundaries[3] = new Vector3(dr.x, dr.y + parentBuilding.height, dr.z);
  
    right = new Vector3(boundaries[0].x - boundaries[1].x,
                         0f,
                         boundaries[0].z - boundaries[1].z);
    width = right.magnitude;
    normal = Vector3.Cross(Vector3.up, right);

    right.Normalize();
    normal.Normalize();
  }

  /*************** METHODS ***************/
  
  public virtual void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    throw new System.NotImplementedException();
  }

  /// <summary>
  /// Creates an array that contains the vertices of the FaceComponents
  /// attached to this Face. The vertices are put in an order so that it
  /// will be easy to form the triangles of the mesh.
  /// </summary>
  public override void FindVertices ()
  {
    verticesModifier = 2 * (parentBuilding.floorCount + 1);
    vertices = new Vector3[8 * componentsPerFloor * parentBuilding.floorCount + verticesModifier];

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
    for (var floor = 1; floor <= parentBuilding.floorCount; ++floor)
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
                                                               floor * parentBuilding.floorHeight - parentBuilding.meshOrigin.y,
                                                               faceComponents[cpn].boundaries[3].z);

        vertices[indexModifier + double_cpf * 3 + 1] = new Vector3(faceComponents[cpn].boundaries[2].x,
                                                                   floor * parentBuilding.floorHeight - parentBuilding.meshOrigin.y,
                                                                   faceComponents[cpn].boundaries[2].z);
      }
  }

  public ComponentCoordinate IndexToCoordinate (int index)
  {
    return new ComponentCoordinate(index / parentBuilding.floorCount,
                                   index % parentBuilding.floorCount);
  }
}

} // namespace Base
} // namespace Thesis