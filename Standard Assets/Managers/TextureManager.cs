using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Thesis {

public sealed class TextureManager
{
  private static readonly TextureManager _instance = new TextureManager();
  public static TextureManager Instance
  {
    get { return _instance; }
  }

  private static bool _isInitialized;
  
  private Dictionary<string, ProceduralTexture> _textures;

  private Dictionary<string, List<ProceduralTexture>> _collections;

  private TextureManager ()
  {
    _textures = new Dictionary<string, ProceduralTexture>();
    _collections = new Dictionary<string,List<ProceduralTexture>>();
    _isInitialized = false;
  }

  public void Init ()
  {
    if (!_isInitialized)
    {
      Object[] texs = Resources.LoadAll("Textures/Door",
                                        typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_neo_door",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/Shutter",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_neo_shutter",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/Roof",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_roof",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/RoofBase",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_roof_base",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/RoofDecor",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_roof_decor",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/CompDecor",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_comp_decor",
                        new ProceduralTexture((Texture2D) texs[i]));

      texs = Resources.LoadAll("Textures/CompDecorSimple",
                                typeof(Texture2D));
      for (var i = 0; i < texs.Length; ++i)
        AddToCollection("tex_comp_decor_simple",
                        new ProceduralTexture((Texture2D) texs[i]));

      CreateBalconyRailTexture();
      CreateWindowTextures();
      CreateBalconyBodyTextures();

      _isInitialized = true;
    }
  }

  public void Unload ()
  {
    _textures.Clear();
    foreach (List<ProceduralTexture> l in _collections.Values)
      l.Clear();
    _collections.Clear();
    _isInitialized = false;
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

  private void CreateBalconyRailTexture ()
  {
    var tex = new ProceduralTexture();
    tex.content = new Texture2D(1024, 512);
    tex.Clear();

    var ratio = 2f;
    var outBorderSize = 0.02f;
    var inBorderSize = 0.018f;
    var spaceBetweenBorders = 0.06f;

    int vOutBorderWidth    = Mathf.FloorToInt(tex.content.width * outBorderSize);
    int topOutBorderWidth  = Mathf.FloorToInt(tex.content.height * outBorderSize * 2 * ratio);
    int vInBorderWidth     = Mathf.FloorToInt(tex.content.width * inBorderSize);
    int hInBorderWidth     = Mathf.FloorToInt(tex.content.height * inBorderSize * ratio);
    int vInBorderOffset    = Mathf.FloorToInt(tex.content.width * spaceBetweenBorders) +
                                              vOutBorderWidth;
    int botInBorderOffset  = Mathf.FloorToInt(tex.content.height * spaceBetweenBorders * ratio);

    var halfWidth = tex.content.width >> 1;

    int topOutBorderY    = tex.content.height - (topOutBorderWidth >> 1);
    int botInBorderY     = botInBorderOffset + (hInBorderWidth >> 1);
    int topInBorderY     = tex.content.height - botInBorderY - topOutBorderWidth;
    int leftOutBorderX   = (vOutBorderWidth >> 1) - 1;
    int rightOutBorderX  = tex.content.width - leftOutBorderX;
    int leftInBorderX    = vInBorderOffset + (vInBorderWidth >> 1);
    int rightInBorderX   = tex.content.width - leftInBorderX;

    // top out border
    tex.lines.Add(new TextureLine(0, topOutBorderY,
                                  tex.content.width, topOutBorderY,
                                  Color.black, topOutBorderWidth));
    // bot in border
    tex.lines.Add(new TextureLine(0, botInBorderY,
                                  tex.content.width, botInBorderY,
                                  Color.black, hInBorderWidth));
    // top in border
    tex.lines.Add(new TextureLine(0, topInBorderY,
                                  tex.content.width, topInBorderY,
                                  Color.black, hInBorderWidth));
    // left out border
    tex.lines.Add(new TextureLine(leftOutBorderX, 0,
                                  leftOutBorderX, tex.content.height,
                                  Color.black, vOutBorderWidth));
    // right out border
    tex.lines.Add(new TextureLine(rightOutBorderX, 0,
                                  rightOutBorderX, tex.content.height,
                                  Color.black, vOutBorderWidth));
    // left in border
    tex.lines.Add(new TextureLine(leftInBorderX, 0,
                                  leftInBorderX, tex.content.width,
                                  Color.black, vInBorderWidth));
    // right in border
    tex.lines.Add(new TextureLine(rightInBorderX, 0,
                                  rightInBorderX, tex.content.width,
                                  Color.black, vInBorderWidth));
    // middle border
    tex.lines.Add(new TextureLine(halfWidth, 0,
                                  halfWidth, tex.content.height,
                                  Color.black, vInBorderWidth));
    // left inner box
    tex.lines.Add(new TextureLine(leftInBorderX, botInBorderY + 2,
                                  halfWidth + 1, topInBorderY,
                                  Color.black, vInBorderWidth));

    tex.lines.Add(new TextureLine(leftInBorderX, topInBorderY - 1,
                                  halfWidth + 1, botInBorderY,
                                  Color.black, vInBorderWidth));
    // right inner box
    tex.lines.Add(new TextureLine(halfWidth + 1, botInBorderY,
                                  rightInBorderX, topInBorderY,
                                  Color.black, vInBorderWidth));

    tex.lines.Add(new TextureLine(halfWidth + 1, topInBorderY,
                                  rightInBorderX, botInBorderY,
                                  Color.black, vInBorderWidth));

    tex.Draw();

    TextureManager.Instance.Add("tex_neo_balcony", tex);
  }

  private void CreateWindowTextures ()
  {
    var color = Color.white;
    var th = 22;
    ProceduralTexture tex;
    int h;

    // 1st texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    h = (tex.content.height * 3) >> 2;
    tex.lines.Add(new TextureLine(0, h, tex.content.width, h,
                                  color, th));
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, h,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_window", tex);
    // 1st texture

    // 2nd texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    h = (tex.content.height * 3) >> 2;
    tex.lines.Add(new TextureLine(0, h, tex.content.width, h,
                                  color, th));
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, h,
                                  color, th << 1));
    tex.lines.Add(new TextureLine(0, h >> 1, tex.content.width, h >> 1,
                                  color, th));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_window", tex);
    // 2nd texture

    // 3rd texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 2, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, tex.content.height,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_window", tex);
    // 3rd texture

    // 4th texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 3, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, (tex.content.height * 3) >> 2,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_window", tex);
    // 4th texture

    // 5th texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 3, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, tex.content.height,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_window", tex);
    // 5th texture
  }

  private void CreateBalconyBodyTextures ()
  {
    var color = Color.white;
    var th = 22;
    ProceduralTexture tex;
    int h;

    // 1st texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    h = (tex.content.height * 3) >> 2;
    tex.lines.Add(new TextureLine(0, h, tex.content.width, h,
                                  color, th));
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, h,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_balcony_door", tex);
    // 1st texture

    // 2nd texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    h = (tex.content.height * 3) >> 2;
    tex.lines.Add(new TextureLine(0, h, tex.content.width, h,
                                  color, th));
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, h,
                                  color, th << 1));
    tex.lines.Add(new TextureLine(0, h >> 1, tex.content.width, h >> 1,
                                  color, th));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_balcony_door", tex);
    // 2nd texture

    // 3rd texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 3, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, tex.content.height,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_balcony_door", tex);
    // 3rd texture

    // 4th texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 4, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, (tex.content.height << 2) / 5,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_balcony_door", tex);
    // 4th texture

    // 5th texture
    tex = new ProceduralTexture(512, 1024);
    tex.SetBackgroundColor(new Color32(25, 0, 143, 255));
    AddBorders(ref tex, color, th);
    AddBalancedLines(ref tex, 4, color, th);
    tex.lines.Add(new TextureLine(tex.content.width >> 1, 0,
                                  tex.content.width >> 1, tex.content.height,
                                  color, th << 1));
    tex.Draw();
    TextureManager.Instance.AddToCollection("tex_neo_balcony_door", tex);
    // 5th texture
  }

  private void AddBorders (ref ProceduralTexture tex, Color color, int thickness)
  {
    tex.lines.Add(new TextureLine(thickness >> 1, 0,
                                  thickness >> 1, tex.content.height,
                                  color, thickness));
    tex.lines.Add(new TextureLine(0, thickness >> 1,
                                  tex.content.width, thickness >> 1,
                                  color, thickness));
    tex.lines.Add(new TextureLine(tex.content.width - (thickness >> 1), 0,
                                  tex.content.width - (thickness >> 1), tex.content.height,
                                  color, thickness));
    tex.lines.Add(new TextureLine(0, tex.content.height - (thickness >> 1),
                                  tex.content.width, tex.content.height - (thickness >> 1),
                                  color, thickness));
  }

  private void AddBalancedLines (ref ProceduralTexture tex, int count,
                                 Color color, int thickness)
  {
    int interval = tex.content.height / (count + 1);
    int h = 0;
    for (int i = 0; i < count; ++i)
    {
      h+= interval;
      tex.lines.Add(new TextureLine(0, h,
                                    tex.content.width, h,
                                    color, thickness));
    }
  }
}

} // namespace Thesis