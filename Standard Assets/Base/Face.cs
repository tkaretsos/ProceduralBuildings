using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Thesis {

public class Face : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly BuildingMesh parentBuilding;

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
  /// The number of vertices on the face's vertical edges, including
  /// the boundaries.
  /// </summary>
  public int edgeVerticesCount = 0;

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
  public Face (BuildingMesh parent, Vector3 dr, Vector3 dl)
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
    componentsPerFloor = Mathf.CeilToInt(width / (component_width + inbetween_space));
    float fixed_space = (width - componentsPerFloor * component_width) / (componentsPerFloor + 1);
    while (fixed_space < 1.75f)
    {
      componentsPerFloor -= 1;
      fixed_space = (width - componentsPerFloor * component_width) / (componentsPerFloor + 1);
    }

    if (parentBuilding.faces[parentBuilding.sortedFaces[0]] == this)
      FindDoorIndexes();
    FindBalconyIndexes();

    float offset;
    int index;
    Vector3 dr;
    Vector3 dl;
    ConstructorInfo[] ctors;
    for (int floor = 0; floor < parentBuilding.floorCount; ++floor)
    {
      offset = fixed_space;
      for (int component = 0; component < componentsPerFloor; ++component)
      {
        index = floor * componentsPerFloor + component;
        dr = boundaries[0] - right * offset + (new Vector3(0f, floor * parentBuilding.floorHeight, 0f));
        dl = dr - right * component_width;
        offset += component_width;
        if (!pattern.ContainsKey(index))
          pattern[index] = typeof(Window);

        ctors = pattern[index].GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        faceComponents.Add((FaceComponent) ctors[0].Invoke(new object[] { this, dr, dl, IndexToCoordinate(index) }));

        offset += fixed_space;
      }
    }
  }

  public override void FindVertices ()
  {
    if (componentsPerFloor == 0)
    {
      edgeVerticesCount = boundaries.Length;
      vertices = boundaries;
      return;
    }

    edgeVerticesCount = 2 * (parentBuilding.floorCount + 1);
    vertices = new Vector3[8 * componentsPerFloor * parentBuilding.floorCount + edgeVerticesCount];

    for (int i = 0; i < edgeVerticesCount; i += 2)
    {
      vertices[i] = new Vector3(boundaries[0].x,
                                boundaries[0].y + (i >> 1) * parentBuilding.floorHeight,
                                boundaries[0].z);

      vertices[i + 1] = new Vector3(boundaries[1].x,
                                    boundaries[1].y + (i >> 1) * parentBuilding.floorHeight,
                                    boundaries[1].z);
    }

    int double_cpf = componentsPerFloor << 1;
    for (var floor = 1; floor <= parentBuilding.floorCount; ++floor)
      for (var cp = 0; cp < componentsPerFloor; ++cp)
      {
        int cpn = cp + componentsPerFloor * (floor - 1);
        int indexModifier = (floor - 1) * (componentsPerFloor << 3) + (cp << 1) + edgeVerticesCount;

        vertices[indexModifier] = new Vector3(
          faceComponents[cpn].boundaries[0].x,
          parentBuilding.boundaries[0].y + (floor - 1) * parentBuilding.floorHeight,
          faceComponents[cpn].boundaries[0].z);

        vertices[indexModifier + 1] = new Vector3(
          faceComponents[cpn].boundaries[1].x,
          parentBuilding.boundaries[0].y + (floor - 1) * parentBuilding.floorHeight,
          faceComponents[cpn].boundaries[1].z);

        vertices[indexModifier + double_cpf] = faceComponents[cpn].boundaries[0];

        vertices[indexModifier + double_cpf + 1] = faceComponents[cpn].boundaries[1];

        vertices[indexModifier + (double_cpf << 1)] = faceComponents[cpn].boundaries[3];

        vertices[indexModifier + (double_cpf << 1) + 1] = faceComponents[cpn].boundaries[2];

        vertices[indexModifier + double_cpf * 3] = new Vector3(
          faceComponents[cpn].boundaries[3].x,
          floor * parentBuilding.floorHeight - parentBuilding.meshOrigin.y,
          faceComponents[cpn].boundaries[3].z);

        vertices[indexModifier + double_cpf * 3 + 1] = new Vector3(
          faceComponents[cpn].boundaries[2].x,
          floor * parentBuilding.floorHeight - parentBuilding.meshOrigin.y,
          faceComponents[cpn].boundaries[2].z);
      }
  }

  private void FindDoorIndexes ()
  {    
    switch (componentsPerFloor)
    {
      case 1:
        doorIndexes.Add(0);
        break;

      case 2:
        doorIndexes.Add(Util.RollDice(new float[] { 0.5f, 0.5f }, new int[] { 0, 1 }));
        break;

      case 3:
        doorIndexes.Add(1);
        break;

      case 4:
        doorIndexes.Add(Util.RollDice(new float[] { 0.5f, 0.5f }, new int[] { 0, 3 }));
        break;

      case 5:
        if (Util.RollDice(new float[] { 0.2f, 0.8f }, new int[] { 1, 2 }) == 1)
          doorIndexes.Add(2);
        else
          doorIndexes.AddRange(new int[] { 0, 4 });
        break;

      default:
        if (Util.RollDice(new float[] { 0.2f, 0.8f }, new int[] { 1, 2 }) == 1)
        {
          if (componentsPerFloor % 2 == 0)
            doorIndexes.Add(Util.RollDice(new float[] { 0.5f, 0.5f }, new int[] { 0, componentsPerFloor - 1 }));
          else
            doorIndexes.Add((componentsPerFloor - 1) / 2);
        }
        else
          doorIndexes.AddRange(new int[] { 0, componentsPerFloor - 1 });
        break;
    }

    foreach (int index in doorIndexes)
      pattern[index] = typeof(Door);
  }

  private void FindBalconyIndexes ()
  {
    int dice;
    
    switch (parentBuilding.floorCount)
    {
      // two floors
      case 2:
        switch (componentsPerFloor)
        {
          case 3:
            if (doorIndexes.Count > 0)
              balconyIndexes.Add(doorIndexes[0] + componentsPerFloor);
            else if (Util.RollDice(new float[] { 0.3f, 0.7f }) == 1)
              balconyIndexes.Add(4);
            break;

          case 4:
            dice = Util.RollDice(new float[] { 0.3f, 0.4f, 0.3f });
            if (dice == 1)
              balconyIndexes.AddRange(new int[] { 5, 6 });
            else if (dice == 2)
              balconyIndexes.AddRange(new int[] { 4, 7 });
            break;

          case 5:
            dice = Util.RollDice(new float[] { 0.25f, 0.25f, 0.25f, 0.25f });
            switch (dice)
            {
              case 1:
                balconyIndexes.AddRange(new int[] { 5, 7, 9 });
                break;
                  
              case 2:
                balconyIndexes.AddRange(new int[] { 5, 9 });
                break;

              case 3:
                balconyIndexes.AddRange(new int[] { 6, 7, 8 });
                break;

              default:
                break;
            }
            break;

          default:
            break;
        }
        break;

      // three floors
      case 3:
        switch (componentsPerFloor)
        {
          case 3:
            dice = Util.RollDice(new float[] { 0.5f, 0.25f, 0.25f });
            if (doorIndexes.Count > 0)
              balconyIndexes.Add(4);
            switch (dice)
            {
              case 1:
                balconyIndexes.Add(7);
                break;

              case 2:
                balconyIndexes.AddRange(new int[] { 6, 8 });
                break;

              default:
                break;
            }
            break;

          case 4:
            dice = Util.RollDice(new float[] { 0.33f, 0.34f, 0.33f });
            switch (dice)
            {
              case 1:
                balconyIndexes.AddRange(new int[] { 5, 6 });
                break;

              case 2:
                balconyIndexes.AddRange(new int[] { 9, 10 });
                break;

              case 3:
                balconyIndexes.AddRange(new int[] { 5, 6, 9, 10 });
                break;
            }
            break;

          case 5:
            dice = Util.RollDice(new float[] { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f });
            switch (dice)
            {
              case 1:
                balconyIndexes.AddRange(new int[] { 6, 7, 8 });
                break;

              case 2:
                balconyIndexes.AddRange(new int[] { 11, 12, 13 });
                break;

              case 3:
                balconyIndexes.AddRange(new int[] { 6, 7, 8, 11, 12, 13 });
                break;

              case 4:
                balconyIndexes.AddRange(new int[] { 6, 8, 11, 12, 13 });
                break;

              case 5:
                balconyIndexes.AddRange(new int[] { 5, 9, 11, 12, 13 });
                break;
            }
            break;
        }
        break;

      default:
        break;
    }
  }

  public ComponentCoordinate IndexToCoordinate (int index)
  {
    return new ComponentCoordinate(index / parentBuilding.floorCount,
                                   index % parentBuilding.floorCount);
  }

  public override void Destroy()
  {
    base.Destroy();

    foreach (FaceComponent fc in faceComponents)
      fc.Destroy();
  }
}

} // namespace Thesis