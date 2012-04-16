using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Thesis {

public sealed class BuildingManager
{
  public List<Neoclassical> neo = new List<Neoclassical>();

  public void CreateNeoclassical (BuildMode mode)
  {
    switch (mode)
    {
      case BuildMode.Many:
        //Thread thread = new Thread(new ThreadStart(BuildMany));
        //thread.Start();
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
                neo.Add(new Neoclassical(
                        new Vector3(x_mod + 9f, 0f, z_mod + 3.5f),
                        new Vector3(x_mod + 9f, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod + 3.5f)));
                break;

              case 2:
                neo.Add(new Neoclassical(
                        new Vector3(x_mod + 11f, 0f, z_mod + 4f),
                        new Vector3(x_mod + 11f, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod + 4f)));
                break;

              case 3:
                neo.Add(new Neoclassical(
                        new Vector3(x_mod + 15f, 0f, z_mod + 6f),
                        new Vector3(x_mod + 15f, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod + 6f)));
                break;

              case 4:
                neo.Add(new Neoclassical(
                        new Vector3(x_mod + 19f, 0f, z_mod + 8f),
                        new Vector3(x_mod + 19f, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod),
                        new Vector3(x_mod, 0f, z_mod + 8f)));
                break;
            }
          }
        }
        break;

      case BuildMode.Two:
        var a = new Neoclassical(
          new Vector3(8f, 0f, 4f),
          new Vector3(8f, 0f, 0f),
          new Vector3(0f, 0f, 0f),
          new Vector3(0f, 0f, 4f));
        neo.Add(a);
        break;

      case BuildMode.Three:
        var b = new Neoclassical(
          new Vector3(11f, 0f, 4f),
          new Vector3(11f, 0f, 0f),
          new Vector3(0f, 0f, 0f),
          new Vector3(0f, 0f, 4f));
        neo.Add(b);
        break;

      case BuildMode.Four:
        var c = new Neoclassical(
          new Vector3(15f, 0f, 6f),
          new Vector3(15f, 0f, 0f),
          new Vector3(0f, 0f, 0f),
          new Vector3(0f, 0f, 6f));
        neo.Add(c);
        break;

      case BuildMode.Five:
        var d = new Neoclassical(
          new Vector3(19f, 0f, 8f),
          new Vector3(19f, 0f, 0f),
          new Vector3(0f, 0f, 0f),
          new Vector3(0f, 0f, 8f));
        neo.Add(d);
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Neoclassical n in neo)
      GameObject.Destroy(n.gameObject);
    neo.Clear();
  }
}

} // namespace Thesis