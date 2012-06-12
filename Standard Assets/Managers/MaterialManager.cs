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

  private static bool _isInitialized;

  private Dictionary<string, Material> _materials;

  private Dictionary<string, List<Material>> _collections;

  private MaterialManager ()
  {
    _materials = new Dictionary<string,Material>();
    _collections = new Dictionary<string,List<Material>>();
    _isInitialized = false;
  }

  public void Init ()
  {
    if (!_isInitialized)
    {
      CreateDoorShutter();
      CreateWindowBalcony();
      CreateRoofRelated();
      CreateWalls();
      CreateCity();

      Material mat;
      foreach (ProceduralTexture tex
                in TextureManager.Instance.GetCollection("tex_comp_decor"))
      {
        mat = new Material(Shader.Find("Transparent/Cutout/Diffuse"));
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_comp_decor", mat);
      }

      foreach (ProceduralTexture tex
                in TextureManager.Instance.GetCollection("tex_comp_decor_simple"))
      {
        mat = new Material(Shader.Find("Diffuse"));
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_comp_decor_simple", mat);
      }

      mat = new Material(Shader.Find("Diffuse"));
      mat.name = "ComponentFrame";
      mat.color = new Color32(186, 189, 189, 255);
      Add(mat.name, mat);

      // lines
      mat = new Material(Shader.Find("VertexLit"));
      mat.name = "line_block";
      mat.SetColor("_Color", Color.green);
      mat.SetColor("_SpecColor", Color.green);
      mat.SetColor("_Emission", Color.green);
      Add(mat.name, mat);

      mat = new Material(Shader.Find("VertexLit"));
      mat.name = "line_lot";
      mat.SetColor("_Color", Color.cyan);
      mat.SetColor("_SpecColor", Color.cyan);
      mat.SetColor("_Emission", Color.cyan);
      Add(mat.name, mat);

      mat = new Material(Shader.Find("VertexLit"));
      mat.name = "line_sidewalk";
      mat.SetColor("_Color", Color.red);
      mat.SetColor("_SpecColor", Color.red);
      mat.SetColor("_Emission", Color.red);
      Add(mat.name, mat);

      //Testing();
      _isInitialized = true;
    }
  }

  public void Unload ()
  {
    foreach (Material m in _materials.Values)
        Object.DestroyImmediate(m, true);
    _materials.Clear();
    foreach (List<Material> l in _collections.Values)
    {
      foreach (Material m in l)
        Object.DestroyImmediate(m, true);
      l.Clear();
    }
    _collections.Clear();
    _isInitialized = false;
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

  public Material FindByTextureName (string name)
  {
    foreach (Material m in _materials.Values)
    {
      if (m.mainTexture != null)
        if (m.mainTexture.name == name)
          return m;
    }

    foreach (List<Material> l in _collections.Values)
      foreach (Material m in l)
      {
        if (m.mainTexture != null)
          if (m.mainTexture.name == name)
            return m;
      }

    return null;
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
                in TextureManager.Instance.GetCollection("tex_door"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.color = color;
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_door", mat);
      }

      foreach (ProceduralTexture tex
                in TextureManager.Instance.GetCollection("tex_shutter"))
      {
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.color = color;
        mat.mainTexture = tex.content;
        MaterialManager.Instance.AddToCollection("mat_shutter", mat);
      }
    }
  }

  private void CreateWindowBalcony ()
  {
    foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_window"))
      MaterialManager.Instance.AddToCollection("mat_window", "Diffuse", tex);

    foreach (ProceduralTexture tex in TextureManager.Instance.GetCollection("tex_balcony_door"))
      MaterialManager.Instance.AddToCollection("mat_balcony_door",
                                               "Diffuse", tex);

    MaterialManager.Instance.Add("mat_balcony_rail",
                                 "Transparent/Cutout/Diffuse",
                                 TextureManager.Instance.Get("tex_balcony"));
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
        MaterialManager.Instance.AddToCollection("mat_roof_decor", mat);
      }
  }

  private void CreateWalls ()
  {
    Material mat;
    foreach (Color color in ColorManager.Instance.GetCollection("col_walls"))
    {
      mat = new Material(Shader.Find("Diffuse"));
      mat.color = color;
      AddToCollection("mat_walls", mat);
    }
  }

  private void CreateCity ()
  {
    Material mat = new Material(Shader.Find("Diffuse"));
    mat.color = new Color32(55, 55, 55, 255);
    _instance.Add("mat_road", mat);

    mat = new Material(Shader.Find("Diffuse"));
    mat.color = Color.gray;
    _instance.Add("mat_sidewalk", mat);

    mat = new Material(Shader.Find("Diffuse"));
    mat.color = new Color32(0, 55, 0, 255);
    _instance.Add("mat_lot", mat);
  }

  private void Testing ()
  {
    Material m = new Material(Shader.Find("Diffuse"));
    m.mainTexture = TextureManager.Instance.Get("tile3").content;
    MaterialManager.Instance.Add("mat_roof", m);

    Material n = new Material(Shader.Find("Diffuse"));
    n.mainTexture = TextureManager.Instance.Get("tex_roof_base").content;
    MaterialManager.Instance.Add("mat_roof_base", n);

    m = new Material(Shader.Find("Transparent/Cutout/Diffuse"));
    m.mainTexture = TextureManager.Instance.Get("decor").content;
    MaterialManager.Instance.Add("mat_decor", m);
  }
}

} // namespace Thesis