using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Thesis {

public sealed class MaterialManager
{
  private static readonly MaterialManager _instance = new MaterialManager();
  public static MaterialManager Instance
  {
    get { return _instance; }
  }

  private Dictionary<string, Material> _materials;

  private Dictionary<string, List<Material>> _collections;

  private MaterialManager ()
  {
    _materials = new Dictionary<string,Material>();
    _collections = new Dictionary<string,List<Material>>();
  }

  public void Init ()
  {
    Object[] mats = Resources.LoadAll("Materials", typeof(Material));
    for (var i = 0; i < mats.Length; ++i)
      _materials.Add(mats[i].name, (Material) mats[i]);
  }

  public void Add (string name, Material material)
  {
    if (!_materials.ContainsKey(name))
    {
      material.name = name;
      _materials.Add(name, material);
    }
  }

  public void AddToCollection (string name, Material material)
  {
    if (_collections.ContainsKey(name))
    {
      if (!_collections[name].Contains(material))
      {
        material.name = name;
        _collections[name].Add(material);
      }
    }
    else
    {
      var list = new List<Material>();
      material.name = name;
      list.Add(material);
      _collections.Add(name, list);
    }
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

  public void AddToCollection (string collectionName, string shaderName, 
                               ProceduralTexture texture)
  {
    var mat = new Material(Shader.Find(shaderName));
    mat.mainTexture = texture.content;
    if (_collections.ContainsKey(collectionName))
    {
      mat.name = collectionName + "_" + (_collections[collectionName].Count + 1).ToString();
      _collections[collectionName].Add(mat);
    }
    else
    {
      mat.name = collectionName + "_1";
      var list = new List<Material>();
      list.Add(mat);
      _collections.Add(collectionName, list);
    }
  }

  public void Set (string name, Material material)
  {
    if (_materials.ContainsKey(name))
      _materials[name] = material;
  }

  public Material Get (string name)
  {
    return _materials.ContainsKey(name) ? _materials[name] : null;
  }

  public List<Material> GetCollection (string name)
  {
    return _collections.ContainsKey(name) ? _collections[name] : null;
  }
}

} // namespace Thesis