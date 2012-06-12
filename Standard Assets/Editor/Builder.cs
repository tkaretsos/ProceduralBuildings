using UnityEngine;
using UnityEditor;
using Thesis;
using System.Collections.Generic;
using System;

using Object = UnityEngine.Object;

public class Builder : EditorWindow
{
  // dimensions stuff
  private Vector2 _p1 = Vector2.zero;
  private Vector2 _p2 = Vector2.zero;
  private Vector2 _p3 = Vector2.zero;
  private Vector2 _p4 = Vector2.zero;
  private float _width = 0f, _depth = 0f, _height = 0f;
  private int _floors = 0;
  
  private enum Area { noChoice, dimensions, points }
  private Area choice1 = Area.noChoice;

  private enum Heights
  {
    noChoice,
    floorCount,
    floorHeight,
    floorCountAndHeight
  }
  private Heights choice2 = Heights.noChoice;

  // component stuff
  private bool inputCompCount = false;
  private int componentCount = 0;
  private bool windowFoldout = false;
  private float windowHeight = 0f;
  private float windowWidth = 0f;
  private bool setWindowWidth = false;
  private bool setWindowHeight = false;
  private bool doorFoldout = false;
  private float doorHeight = 0f;
  private float doorWidth = 0f;
  private bool setDoorWidth = false;
  private bool setDoorHeight = false;
  private bool balconyFoldout = false;
  private bool setBalconyWidth = false;
  private float balconyWidth = 0f;
  private bool setBalconyHeight = false;
  private float balconyHeight = 0f;

  // roof stuff
  private enum myRType
  {
    nothing,
    Flat,
    SinglePeak,
    DoublePeak
  }
  private myRType rtype = myRType.nothing;
  private bool inputRoofDecor = false;
  private Object roofDecorTexture;
  private bool inputRoof = false;
  private Object roofTexture;
  private bool inputRoofBase = false;
  private Object roofBaseTexture;

  private Building _building = new Building();
  private Object texture;
  
  [MenuItem ("Window/Builder")]
  static void Init ()
  {
    //ColorManager.Instance.Init();
    //TextureManager.Instance.Init();
    //MaterialManager.Instance.Init();

    EditorWindow.GetWindow(typeof(Builder), false, "Builder");
  }

  void OnGUI ()
  {
    GUILayout.Space(20);
    EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Destroy"))
      {
        //if (_building != null)
        //{
        //  _building.Destroy();
        //  _building = null;
        //}
      }

      if (GUILayout.Button("Create"))
      {
        //if (_building != null)
        //  _building.Destroy();
        //_building = new Building();
        //_building.CreateBuilding();
        //_building.Draw();
      }
    EditorGUILayout.EndHorizontal();

    GUILayout.Space(20);
    BuildingDimensions();
    GUILayout.Space(10);
    ComponentParams();
    GUILayout.Space(10);
    RoofParams();
  }

  void OnDestroy ()
  {
    //ColorManager.Instance.Unload();
    //TextureManager.Instance.Unload();
    //MaterialManager.Instance.Unload();
  }

  void OnInspectorUpdate ()
  {
    //ColorManager.Instance.Init();
    //TextureManager.Instance.Init();
    //MaterialManager.Instance.Init();
    //Repaint();
  }
  
  private void BuildingDimensions ()
  {
    EditorGUILayout.LabelField("Dimensions", EditorStyles.boldLabel);

    choice1 = (Area) EditorGUILayout.EnumPopup("Area", choice1);
    switch (choice1)
    {
      case Area.dimensions:
        _building.startingPoints = null;

        _width = EditorGUILayout.FloatField("Width", _width);
        _depth = EditorGUILayout.FloatField("Depth", _depth);
        _building.width0 = _width;
        _building.width1 = _depth;
        break;

      case Area.points:
        _building.width0 = 0f;
        _building.width1 = 0f;

        _p1 = EditorGUILayout.Vector2Field("point 1", _p1);
        _p2 = EditorGUILayout.Vector2Field("point 2", _p2);
        _p3 = EditorGUILayout.Vector2Field("point 3", _p3);
        _p4 = EditorGUILayout.Vector2Field("point 4", _p4);
        _building.startingPoints = new Vector3[4];
        _building.startingPoints[0] = new Vector3(_p1.x, 0f, _p1.y);
        _building.startingPoints[1] = new Vector3(_p2.x, 0f, _p2.y);
        _building.startingPoints[2] = new Vector3(_p3.x, 0f, _p3.y);
        _building.startingPoints[3] = new Vector3(_p4.x, 0f, _p4.y);
        break;

      default:
        _building.width0 = 0f;
        _building.width1 = 0f;
        _building.startingPoints = null;
        break;
    }

    choice2 = (Heights) EditorGUILayout.EnumPopup("Height", choice2);
    switch (choice2)
    {
      case Heights.floorCount:
        _floors = EditorGUILayout.IntField("Floor Count", _floors);
        _building.floorCount = _floors;
        _building.floorHeight = 0f;
        break;

      case Heights.floorHeight:
        _height = EditorGUILayout.FloatField("Floor Height", _height);
        _building.floorHeight = _height;
        _building.floorCount = 0;
        break;

      case Heights.floorCountAndHeight:
        _floors = EditorGUILayout.IntField("Floor Count", _floors);
        _height = EditorGUILayout.FloatField("Floor Height", _height);
        _building.floorHeight = _height;
        _building.floorCount = _floors;
        break;

      default:
        _building.floorHeight = 0f;
        _building.floorCount = 0;
        break;
    }
  }

  private void ComponentParams ()
  {
    EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Components per floor");
    inputCompCount = EditorGUILayout.Toggle(inputCompCount);
    if (inputCompCount)
    {
      componentCount = EditorGUILayout.IntField(componentCount, GUILayout.Width(25));
      _building.componentsPerFloor = componentCount;
    }
    else
    {
      componentCount = 0;
      _building.componentsPerFloor = 0;
    }
    EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal();
    //EditorGUILayout.LabelField("Component distance");
    //inputCompDistance = EditorGUILayout.Toggle(inputCompDistance);
    //if (inputCompDistance)
    //{
    //  distance = EditorGUILayout.FloatField(distance, GUILayout.Width(25));
    //  _building.distance = distance;
    //}
    //else
    //{
    //  distance = 0f;
    //  _building.distance = 0f;
    //}
    //EditorGUILayout.EndHorizontal();

    // Window
    windowFoldout = EditorGUILayout.Foldout(windowFoldout, "Window");
    if (windowFoldout)
    {
      // width
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Width");
      setWindowWidth = EditorGUILayout.Toggle(setWindowWidth);
      if (setWindowWidth)
        windowWidth = EditorGUILayout.FloatField(windowWidth, GUILayout.Width(30));
      else
        windowWidth = 0f;
      EditorGUILayout.EndHorizontal();
      _building.windowWidth = windowWidth;

      // height
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Height");
      setWindowHeight = EditorGUILayout.Toggle(setWindowHeight);
      if (setWindowHeight)
        windowHeight = EditorGUILayout.FloatField(windowHeight, GUILayout.Width(30));
      else
        windowHeight = 0f;
      EditorGUILayout.EndHorizontal();
      _building.windowHeight = windowHeight;
      
      // material
      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField("Texture");
      //setWindowTexture = EditorGUILayout.Toggle(setWindowTexture);
      //if (setWindowTexture)
      //  windowTexture = EditorGUILayout.ObjectField(windowTexture,
      //                                              typeof(Texture),
      //                                              false,
      //                                              GUILayout.Height(55));
      //else
      //  windowTexture = null;
      //EditorGUILayout.EndHorizontal();
      // find material by windowtexture name
    }

    // Door
    doorFoldout = EditorGUILayout.Foldout(doorFoldout, "Door");
    if (doorFoldout)
    {
      // width
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Width");
      setDoorWidth = EditorGUILayout.Toggle(setDoorWidth);
      if (setDoorWidth)
        doorWidth = EditorGUILayout.FloatField(doorWidth, GUILayout.Width(30));
      else
        doorWidth = 0f;
      EditorGUILayout.EndHorizontal();
      _building.doorWidth = doorWidth;

      // height
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Height");
      setDoorHeight = EditorGUILayout.Toggle(setDoorHeight);
      if (setDoorHeight)
        doorHeight = EditorGUILayout.FloatField(doorHeight, GUILayout.Width(30));
      else
        doorHeight = 0f;
      EditorGUILayout.EndHorizontal();
      _building.doorHeight = doorHeight;

      // material
      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField("Texture");
      //setDoorTexture = EditorGUILayout.Toggle(setDoorTexture);
      //if (setDoorTexture)
      //  doorTexture = EditorGUILayout.ObjectField(doorTexture,
      //                                            typeof(Texture),
      //                                            false,
      //                                            GUILayout.Height(55));
      //else
      //  doorTexture = null;
      //EditorGUILayout.EndHorizontal();
      // find material by windowtexture name
    }

    // balcony
    balconyFoldout = EditorGUILayout.Foldout(balconyFoldout, "Balcony");
    if (balconyFoldout)
    {
      // width
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Width");
      setBalconyWidth = EditorGUILayout.Toggle(setBalconyWidth);
      if (setBalconyWidth)
        balconyWidth = EditorGUILayout.FloatField(balconyWidth, GUILayout.Width(30));
      else
        balconyWidth = 0f;
      EditorGUILayout.EndHorizontal();
      _building.balconyWidth = balconyWidth;

      // height
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Height");
      setBalconyHeight = EditorGUILayout.Toggle(setBalconyHeight);
      if (setBalconyHeight)
        balconyHeight = EditorGUILayout.FloatField(balconyHeight, GUILayout.Width(30));
      else
        balconyHeight = 0f;
      EditorGUILayout.EndHorizontal();
      _building.balconyHeight = balconyHeight;
    }
  }

  private void RoofParams ()
  {
    EditorGUILayout.LabelField("Roof", EditorStyles.boldLabel);

    rtype = (myRType) EditorGUILayout.EnumPopup("Roof type", rtype);
    switch (rtype)
    {
      case myRType.Flat:
        _building.roofType = Type.GetType(GetFullType(myRType.Flat));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Roof decor texture");
        inputRoofDecor = EditorGUILayout.Toggle(inputRoofDecor);
        if (inputRoofDecor)
          roofDecorTexture = EditorGUILayout.ObjectField(roofDecorTexture,
                                                         typeof(Texture),
                                                         false,
                                                         GUILayout.Height(55));
        else
        {
          _building.roofDecorMaterial = null;
          roofDecorTexture = null;
        }
        EditorGUILayout.EndHorizontal();
        // TODO find zhe material here
        break;

      case myRType.SinglePeak:
        _building.roofType = Type.GetType(GetFullType(myRType.SinglePeak));
        inputRoofDecor = false;
        _building.roofDecorMaterial = null;
        break;

      case myRType.DoublePeak:
        _building.roofType = Type.GetType(GetFullType(myRType.DoublePeak));
        inputRoofDecor = false;
        _building.roofDecorMaterial = null;
        break;

      default:
        _building.roofType = null;
        inputRoofDecor = false;
        _building.roofDecorMaterial = null;
        break;
    }

    // roof texture
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Roof texture");
    inputRoof = EditorGUILayout.Toggle(inputRoof);
    if (inputRoof)
      roofTexture = EditorGUILayout.ObjectField(roofTexture,
                                                typeof(Texture),
                                                false,
                                                GUILayout.Height(55));
    else
    {
      _building.roofMaterial = null;
      roofTexture = null;
    }
    EditorGUILayout.EndHorizontal();
    // TODO find zhe material here

    // roof base texture
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Roof base texture");
    inputRoofBase = EditorGUILayout.Toggle(inputRoofBase);
    if (inputRoofBase)
      roofBaseTexture = EditorGUILayout.ObjectField(roofBaseTexture,
                                                    typeof(Texture),
                                                    false,
                                                    GUILayout.Height(55));
    else
    {
      _building.roofBaseMaterial = null;
      roofBaseTexture = null;
    }
    EditorGUILayout.EndHorizontal();
    // TODO find zhe material here
  }

  private string GetFullType (myRType t)
  {
    return "Thesis." + t.ToString() + "Roof,Assembly-CSharp-firstpass";
  }
}