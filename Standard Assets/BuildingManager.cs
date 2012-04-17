using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Thesis {

public sealed class BuildingManager
{
  public List<Neoclassical> neo = new List<Neoclassical>();

  public void CreateNeoclassical (BuildMode mode)
  {
    Neoclassical n;
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
                n = new Neoclassical(
                    new Vector3(x_mod + 9f, 0f, z_mod + 3.5f),
                    new Vector3(x_mod + 9f, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod + 3.5f));
                n.FindVertices();
                n.FindTriangles();
                n.Draw();
                n.Optimize();
                n.gameObject.SetActiveRecursively(true);
                neo.Add(n);
                break;

              case 2:
                n = new Neoclassical(
                    new Vector3(x_mod + 11f, 0f, z_mod + 4f),
                    new Vector3(x_mod + 11f, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod + 4f));
                n.FindVertices();
                n.FindTriangles();
                n.Draw();
                n.Optimize();
                n.gameObject.SetActiveRecursively(true);
                neo.Add(n);
                break;

              case 3:
                n = new Neoclassical(
                    new Vector3(x_mod + 15f, 0f, z_mod + 6f),
                    new Vector3(x_mod + 15f, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod + 6f));
                n.FindVertices();
                n.FindTriangles();
                n.Draw();
                n.Optimize();
                n.gameObject.SetActiveRecursively(true);
                neo.Add(n);
                break;

              case 4:
                n = new Neoclassical(
                    new Vector3(x_mod + 19f, 0f, z_mod + 8f),
                    new Vector3(x_mod + 19f, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod),
                    new Vector3(x_mod, 0f, z_mod + 8f));
                n.FindVertices();
                n.FindTriangles();
                n.Draw();
                n.Optimize();
                n.gameObject.SetActiveRecursively(true);
                neo.Add(n);
                break;
            }            
          }
        }
        break;

      case BuildMode.Two:
        n = new Neoclassical(
            new Vector3(8f, 0f, 4f),
            new Vector3(8f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 4f));
        n.FindVertices();
        n.FindTriangles();
        n.Draw();
        n.Optimize();
        n.gameObject.SetActiveRecursively(true);
        neo.Add(n);
        break;

      case BuildMode.Three:
        n = new Neoclassical(
            new Vector3(11f, 0f, 4f),
            new Vector3(11f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 4f));
        n.FindVertices();
        n.FindTriangles();
        n.Draw();
        n.Optimize();
        n.gameObject.SetActiveRecursively(true);
        neo.Add(n);
        break;

      case BuildMode.Four:
        n = new Neoclassical(
            new Vector3(15f, 0f, 6f),
            new Vector3(15f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 6f));
        n.FindVertices();
        n.FindTriangles();
        n.Draw();
        n.Optimize();
        n.gameObject.SetActiveRecursively(true);
        neo.Add(n);
        break;

      case BuildMode.Five:
        n = new Neoclassical(
            new Vector3(19f, 0f, 8f),
            new Vector3(19f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 8f));
        n.FindVertices();
        n.FindTriangles();
        n.Draw();
        n.Optimize();
        n.gameObject.SetActiveRecursively(true);
        neo.Add(n);
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