using UnityEngine;
using System.Collections.Generic;

namespace Thesis {

public sealed class BuildingManager
{
  private static readonly BuildingManager _instance = new BuildingManager();
  public static BuildingManager Instance
  {
    get { return _instance; }
  }

  public List<Building> neo = new List<Building>();

  private BuildingManager () { }

  public void Init () { }

  public void CreateNeoclassical (BuildMode mode)
  {
    switch (mode)
    {      
      case BuildMode.Many:
        for (int i = 0; i < 25; ++i)
        {
          float x_mod = i * 30f;
          for (int j = 0; j < 25; ++j)
          {
            float z_mod = j * 15f;
            int dice = Util.RollDice(new float[] { 0.25f, 0.25f, 0.25f, 0.25f });
            switch (dice)
            {
              case 1:
                Build(new Vector3(x_mod + 9f, 0f, z_mod + 3.5f),
                      new Vector3(x_mod + 9f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 3.5f));
                break;

              case 2:
                Build(new Vector3(x_mod + 11f, 0f, z_mod + 4f),
                      new Vector3(x_mod + 11f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 4f));
                break;

              case 3:
                Build(new Vector3(x_mod + 15f, 0f, z_mod + 6f),
                      new Vector3(x_mod + 15f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 6f));
                break;

              case 4:
                Build(new Vector3(x_mod + 19f, 0f, z_mod + 8f),
                      new Vector3(x_mod + 19f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 8f));
                break;
            }            
          }
        }
        break;

      case BuildMode.Two:
        Build(new Vector3(8f, 0f, 4f),
              new Vector3(8f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 4f));
        break;

      case BuildMode.Three:
        Build(new Vector3(11f, 0f, 4f),
              new Vector3(11f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 4f));

        //Build(new Vector3(20f, 0f, 4f),
        //      new Vector3(37f, 0f, 4f),
        //      new Vector3(37f, 0f, 0f),
        //      new Vector3(20f, 0f, 0f));
        break;

      case BuildMode.Four:
        Build(new Vector3(15f, 0f, 6f),
              new Vector3(15f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 6f));
        break;

      case BuildMode.Five:
        Build(new Vector3(19f, 0f, 8f),
              new Vector3(19f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 8f));
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Building n in neo)
      n.Destroy();
    neo.Clear();
  }

  private void Build (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    var n = new Building(p1, p2, p3, p4);
    n.buildingMesh.FindVertices();
    n.buildingMesh.FindTriangles();
    n.buildingMesh.Draw();
    n.CombineSubmeshes();
    n.gameObject.SetActiveRecursively(true);
    neo.Add(n);
  }
}

} // namespace Thesis