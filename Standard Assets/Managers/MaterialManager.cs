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

    CreateDoorShutter();
    CreateWindowBalcony();
    CreateRoofRelated();

    Testing();
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

  private void CreateDoorShutter ()
  {
    foreach (Color color in ColorManager.Instance.GetCollection("col_components"))
    {
      foreach (ProceduralTexture tex
                in TextureManager.Instance.GetCollection("tex_neo_door"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.color = color;
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_neo_door", mat);
      }

      foreach (ProceduralTexture tex
                in TextureManager.Instance.GetCollection("tex_neo_shutter"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.color = color;
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_neo_shutter", mat);
      }
    }
  }

  private void CreateWindowBalcony ()
  {
    foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_neo_window"))
      MaterialManager.Instance.AddToCollection("mat_neo_window", "Diffuse", tex);

    foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_neo_balcony_door"))
      MaterialManager.Instance.AddToCollection("mat_neo_balcony_door",
                                               "Diffuse", tex);

    MaterialManager.Instance.Add("mat_neo_balcony_rail",
                                 "Transparent/Cutout/Diffuse",
                                 TextureManager.Instance.Get("tex_neo_balcony"));
  }

  private void CreateRoofRelated()
  {
    if (TextureManager.Instance.GetCollection("tex_roof") != null)
      foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_roof"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_roof", mat);
      }

    if (TextureManager.Instance.GetCollection("tex_roof_base") != null)
      foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_roof_base"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_roof_base", mat);
      }

    if (TextureManager.Instance.GetCollection("tex_roof_decor") != null)
      foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_roof_decor"))
      {
        Material mat = new Material(Shader.Find("Transparent/Cutout/Diffuse"));
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_roof", mat);
      }
  }

  private void Testing ()
  {
    Material m = new Material(Shader.Find("Diffuse"));
    m.mainTexture = TextureManager.Instance.Get("tile3").content;
    MaterialManager.Instance.Add("mat_neo_roof", m);

    Material n = new Material(Shader.Find("Diffuse"));
    n.mainTexture = TextureManager.Instance.Get("tex_roof_base").content;
    MaterialManager.Instance.Add("mat_roof_base", n);

    m = new Material(Shader.Find("Transparent/Cutout/Diffuse"));
    m.mainTexture = TextureManager.Instance.Get("decor").content;
    MaterialManager.Instance.Add("mat_decor", m);
  }
}

} // namespace Thesis