using UnityEngine;
using System.Collections.Generic;

public sealed class NeoclassicalFace : Base.Face
{
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
        faceComponents.Add(new NeoclassicalWindow(this, dr, dl));
        offset += fixed_space;
      }
    }
   }

  public override void ConstructDoors ()
  {
    var doorIndexes = new List<int>();
    
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
        {
          doorIndexes.Add(0);
          doorIndexes.Add(4);
        }
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
        {
          doorIndexes.Add(0);
          doorIndexes.Add(componentsPerFloor - 1);
        }
        break;
    }

    foreach (int index in doorIndexes)
    {
      faceComponents[index] = new NeoclassicalDoor(
                                this,
                                new Vector3(
                                  faceComponents[index].boundaries[0].x,
                                  parentBuilding.boundaries[0].y,
                                  faceComponents[index].boundaries[0].z),
                                new Vector3(
                                  faceComponents[index].boundaries[1].x,
                                  parentBuilding.boundaries[0].y,
                                  faceComponents[index].boundaries[1].z)
                              );      
    }
  }
}
