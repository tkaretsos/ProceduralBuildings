using UnityEngine;
using System.Collections.Generic;

public sealed class NeoclassicalFace : Base.Face
{
  public List<int> doorIndexes = new List<int>();

  public List<int> balconyIndexes = new List<int>();

  public NeoclassicalFace (Base.Building parent, Vector3 dr, Vector3 dl)
    : base (parent, dr, dl)
  {}

  public override void ConstructFaceComponents (float component_width, float inbetween_space)
  {
    componentsPerFloor = Mathf.CeilToInt(width / (component_width + inbetween_space));
    float fixed_space = (width - componentsPerFloor * component_width) / (componentsPerFloor + 1);
    while (fixed_space < 1.5f)
    {
      componentsPerFloor -= 1;
      fixed_space = (width - componentsPerFloor * component_width) / (componentsPerFloor + 1);
    }

    for (int floor = 0; floor < parentBuilding.floorNumber; ++floor)
    {
      float offset = fixed_space;
      for (int i = 0; i < componentsPerFloor; ++i)
      {
        Vector3 dr = boundaries[0] - right * offset + (new Vector3(0f, floor * parentBuilding.floorHeight, 0f));
        Vector3 dl = dr - right * component_width;
        offset += component_width;
        faceComponents.Add(new NeoclassicalWindow(this, dr, dl, floor));
        offset += fixed_space;
      }
    }
   }

  public override void ConstructDoors ()
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
      faceComponents[index] = new NeoclassicalDoor(
                                this,
                                new Vector3(
                                  faceComponents[index].boundaries[0].x,
                                  parentBuilding.boundaries[0].y,
                                  faceComponents[index].boundaries[0].z),
                                new Vector3(
                                  faceComponents[index].boundaries[1].x,
                                  parentBuilding.boundaries[0].y,
                                  faceComponents[index].boundaries[1].z),
                                faceComponents[index].atFloor
                              );      
  }

  public override void ConstructBalconies ()
  {
    int dice;
    if (doorIndexes.Count > 0)
      switch (parentBuilding.floorNumber)
      {
        // two floors
        case 2:
          switch (componentsPerFloor)
          {
            case 3:
              balconyIndexes.Add(doorIndexes[0] + componentsPerFloor);
              break;

            case 4:
              dice = Util.RollDice(new float[] { 0.5f, 0.25f, 0.25f });
              if (dice == 1)
                balconyIndexes.AddRange(new int[] { 5, 6 });
              else if (dice == 2)
                balconyIndexes.Add(doorIndexes[0] + 4);
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
              balconyIndexes.Add(doorIndexes[0] + componentsPerFloor);
              switch (dice)
              {
                case 1:
                  balconyIndexes.Add(doorIndexes[0] + 2 * componentsPerFloor);
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

    foreach (int i in balconyIndexes)
      faceComponents[i] = new NeoclassicalBalcony(
                            this,
                            new Vector3(faceComponents[i].boundaries[0].x,
                                        faceComponents[i].atFloor * parentBuilding.floorHeight,
                                        faceComponents[i].boundaries[0].z),
                            new Vector3(faceComponents[i].boundaries[1].x,
                                        faceComponents[i].atFloor * parentBuilding.floorHeight,
                                        faceComponents[i].boundaries[1].z),
                            faceComponents[i].atFloor
                          );
  }
}
