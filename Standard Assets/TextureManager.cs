using System;
using System.Reflection;
using System.Collections.Generic;
using Thesis.Base;

namespace Thesis {

public sealed class TextureManager
{
  private static readonly TextureManager _instance = new TextureManager();
  public static TextureManager Instance
  {
    get { return _instance; }
  }
  
  private Dictionary<string, ProceduralTexture> _textures;

  private Dictionary<string, List<ProceduralTexture>> _collections;

  private TextureManager ()
  {
    _textures = new Dictionary<string, ProceduralTexture>();
    _collections = new Dictionary<string,List<ProceduralTexture>>();
  }

  public void Add (string name, ProceduralTexture texture)
  {
    if (!_textures.ContainsKey(name))
    {
      texture.content.name = name;
      _textures.Add(name, texture);
    }
  }

  public void AddToCollection (string name, ProceduralTexture texture)
  {
    if (_collections.ContainsKey(name))
    {
      texture.content.name = name + "_" + (_collections[name].Count + 1).ToString();
      _collections[name].Add(texture);
    }
    else
    {
      var list = new List<ProceduralTexture>();
      texture.content.name = name + "_1";
      list.Add(texture);
      _collections.Add(name, list);
    }
  }

  public void Set (string name, ProceduralTexture texture)
  {
    if (_textures.ContainsKey(name))
      _textures[name] = texture;
  }

  public ProceduralTexture Get (string name)
  {
    return _textures.ContainsKey(name) ? _textures[name] : null;
  }

  public List<ProceduralTexture> GetCollection (string name)
  {
    return _collections.ContainsKey(name) ? _collections[name] : null;
  }
}

} // namespace Thesis