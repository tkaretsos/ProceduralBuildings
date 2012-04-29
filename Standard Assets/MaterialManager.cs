using System;
using System.Collections.Generic;
using UnityEngine;
using Thesis.Base;

using Object = UnityEngine.Object;

namespace Thesis {

public class MaterialManager
{
  private static readonly MaterialManager _instance = new MaterialManager();
  public static MaterialManager Instance
  {
    get { return _instance; }
  }

  private Dictionary<string, Material> _materials;

  private MaterialManager () { }

  public void Init ()
  {
    _materials = new Dictionary<string, Material>();
    Object[] mats = Resources.LoadAll("Materials", typeof(Material));
    for (var i = 0; i < mats.Length; ++i)
      _materials.Add(((Material) mats[i]).name, (Material) mats[i]);
  }

  public void Add (string name, Material material)
  {
    if (!_materials.ContainsKey(name))
      _materials.Add(name, material);
  }

  public void Add (string name, string shaderName, ProceduralTexture texture)
  {
    if (!_materials.ContainsKey(name))
    {
      var mat = new Material(Shader.Find(shaderName));
      mat.name = name;
      mat.mainTexture = texture.content;
      _materials.Add(mat.name, mat);
    }
  }

  public Material Get (string name)
  {
    return _materials.ContainsKey(name) ? _materials[name] : null;
  }
}

} // namespace Thesis