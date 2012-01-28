using UnityEngine;
using System.Collections.Generic;

public class BuildingMesh
{
	// fields
#pragma warning disable 0414
	private float _height;
	private float _floor_height;
	private int _floor_number;
	private readonly BuildingType _type;
	private List<Vector3> _boundaries = new List<Vector3>();
	private List<Face> _faces = new List<Face>();
#pragma warning restore 0414

	
	// properties
	public float Height
	{
		get { return _height; }
	}
	
	
	public float FloorHeight
	{
		get { return _floor_height; }
		set 
		{
			_floor_height = value;
			if (_floor_number > 0)
				_height = _floor_height * _floor_number;
		}
	}
	
	
	public int FloorNumber
	{
		get { return _floor_number; }
		set
		{
			_floor_number = value;
			if (_floor_height > 0f)
				_height = _floor_height * _floor_number;
		}
	}
	
	
	public BuildingType Type
	{
		get { return _type; }
	}	
	
	
	// constructors
	public BuildingMesh (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, BuildingType type)
	{
		_boundaries.Add(p1);
		_boundaries.Add(p2);
		_boundaries.Add(p3);
		_boundaries.Add(p4);
		_type = type;
		
		//TODO set height to 0 when neoclassical is implemented
		_height = 6f;
		_floor_height = 0f;
		_floor_number = 0;
	}
	
	
	// methods
	public void ConstructFaces()
	{
		_faces.Add(new Face(this, _boundaries[0], _boundaries[1]));
		_faces.Add(new Face(this, _boundaries[1], _boundaries[2]));
		_faces.Add(new Face(this, _boundaries[2], _boundaries[3]));
		_faces.Add(new Face(this, _boundaries[3], _boundaries[0]));
	}
	
	
	public void Draw(Material mat)
	{
		mat.SetPass(0);
	
		// draw roof
		GL.PushMatrix();
		GL.Begin(GL.QUADS);
		foreach (Vector3 v in _boundaries)
			GL.Vertex(v + (new Vector3(0, _height, 0)));
		GL.End();
		GL.PopMatrix();	
		
		// draw faces
		foreach (Face face in _faces)
			face.Draw();
	}
}
