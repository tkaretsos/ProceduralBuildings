using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
  private enum BuildingMode
  {
    Many,
    Two,
    Three,
    Four,
    Five
  }

  private List<Neoclassical> neo = new List<Neoclassical>();

	void Start ()
  {
	  CreateNeoclassical();
	}

	void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Many);
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.Alpha4))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.Alpha5))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Five);
    }
	}

  private void CreateNeoclassical(BuildingMode mode = BuildingMode.Two)
  {
    switch (mode)
    {
      case BuildingMode.Many:
        for (int i = 0; i < 25; ++i)
        {
          float x_mod = i * 15f;
          for (int j = 0; j < 25; ++j)
          {
            float z_mod = j * 9f;
            var n = new Neoclassical(
              new Vector3(x_mod + 9f, 0f, z_mod + 3.5f),
              new Vector3(x_mod + 9f, 0f, z_mod), 
              new Vector3(x_mod     , 0f, z_mod), 
              new Vector3(x_mod     , 0f, z_mod+ 3.5f));
            n.SetActiveRecursively(true);
            neo.Add(n);
          }
        }
        break;

      case BuildingMode.Two:
        var a = new Neoclassical(
          new Vector3(8f, 0f, 4f), 
          new Vector3(8f, 0f, 0f), 
          new Vector3(0f, 0f, 0f), 
          new Vector3(0f, 0f, 4f));
        neo.Add(a);
        break;

      case BuildingMode.Three:
        var b = new Neoclassical(
          new Vector3(11f, 0f, 4f), 
          new Vector3(11f, 0f, 0f), 
          new Vector3(0f , 0f, 0f), 
          new Vector3(0f , 0f, 4f));
        neo.Add(b);
        break;

      case BuildingMode.Four:
        var c = new Neoclassical(
          new Vector3(15f, 0f, 6f),
          new Vector3(15f, 0f, 0f),
          new Vector3(0f , 0f, 0f),
          new Vector3(0f , 0f, 6f));
        neo.Add(c);
        break;

      case BuildingMode.Five:
        var d = new Neoclassical(
          new Vector3(19f, 0f, 8f),
          new Vector3(19f, 0f, 0f),
          new Vector3(0f , 0f, 0f),
          new Vector3(0f , 0f, 8f));
        neo.Add(d);
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Neoclassical n in neo)
      Destroy(n.gameObject);
    neo.Clear();
  }
}
