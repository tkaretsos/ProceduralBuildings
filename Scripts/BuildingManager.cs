using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
  private enum BuildingMode
  {
    Many,
    Big,
    Small
  }

  [SerializeField]
  private Material _material;

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
      CreateNeoclassical(BuildingMode.Big);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Small);
    }
	}

  private void CreateNeoclassical(BuildingMode mode = BuildingMode.Small)
  {
    switch (mode)
    {
      case BuildingMode.Many:
        for (int i = 0; i < 5; ++i)
        {
          float x_mod = i * 15f;
          for (int j = 0; j < 5; ++j)
          {
            float z_mod = j * 9f;
            neo.Add(new Neoclassical(
              new Vector3(x_mod + 9f + Random.Range(0.5f, 1.5f), 0f, z_mod + 3.5f + Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod + 9f + Random.Range(0.5f, 1.5f), 0f, z_mod - Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod - Random.Range(0.5f, 1.5f), 0f, z_mod - Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod - Random.Range(0.5f, 1.5f), 0f, z_mod + 3.5f + Random.Range(0.5f, 1.5f)),
              _material
            ));
          }
        }
        break;

      case BuildingMode.Big:
        neo.Add(new Neoclassical(
          new Vector3(20f + Random.Range(0.5f, 1.5f), 0f, 8f + Random.Range(0.5f, 1.5f)),
          new Vector3(20f + Random.Range(0.5f, 1.5f), 0f, Random.Range(0.5f, 1.5f)),
          new Vector3(Random.Range(0.5f, 1.5f), 0f, -Random.Range(0.5f, 1.5f)),
          new Vector3(Random.Range(0.5f, 1.5f), 0f, 8f + Random.Range(0.5f, 1.5f)),
          _material
        ));
        break;

      case BuildingMode.Small:
        neo.Add(new Neoclassical(
          new Vector3(9f + Random.Range(0.25f, 0.75f), 0f, 3.5f + Random.Range(0.25f, 0.75f)),
          new Vector3(9f + Random.Range(0.25f, 0.75f), 0f, Random.Range(0.25f, 0.75f)),
          new Vector3(Random.Range(0.25f, 0.75f), 0f, -Random.Range(0.25f, 0.75f)),
          new Vector3(Random.Range(0.25f, 0.75f), 0f, 3.5f + Random.Range(0.25f, 0.75f)),
          _material
        ));
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
