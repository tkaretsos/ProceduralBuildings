using UnityEngine;
using UnityEditor;
using Thesis;
using System.Collections.Generic;
using System;

using Object = UnityEngine.Object;

public class Builder : EditorWindow {

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

  private Building _building = new Building();
  private Object texture;

  private Vector2 _p1 = Vector2.zero;
  private Vector2 _p2 = Vector2.zero;
  private Vector2 _p3 = Vector2.zero;
  private Vector2 _p4 = Vector2.zero;
  private float _width = 0f, _depth = 0f, _height = 0f;
  private int _floors = 0;

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
    BuildingDimensions();

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
  
  void BuildingDimensions ()
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
}