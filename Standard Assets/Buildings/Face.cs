using UnityEngine;
using System.Collections.Generic;

public class Face
{
	// fields
	
	private readonly BuildingMesh _parent;
	private Vector3 _normal;
	private float _width;
	private bool _is_free;
	private List<Vector3> _boundaries = new List<Vector3>();
#pragma warning disable 0414
	private List<FaceComponent> _face_components = new List<FaceComponent>();
#pragma warning restore 0414
	
	
	// properties

	/// <summary>
	/// Gets the parent mesh.
	/// </summary>
	/// <value>
	/// The parent mesh.
	/// </value>
	public BuildingMesh Parent
	{
		get { return _parent; }
	}
	
	/// <summary>
	/// Gets the normal of this face.
	/// </summary>
	/// <value>
	/// The normal.
	/// </value>
	public Vector3 Normal
	{
		get { return _normal; }
	}
	
	/// <summary>
	/// Gets the width of this face.
	/// </summary>
	/// <value>
	/// The width.
	/// </value>
	public float Width
	{
		get { return _width; }
	}
	
	
	// constructors
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Face"/> class,
	/// from the given points in clockwise order.
	/// </summary>
	/// <param name='parent'>
	/// The parent mesh of this face.
	/// </param>
	/// <param name='dr'>
	/// Down-right point of the face.
	/// </param>
	/// <param name='dl'>
	/// Down-left point of the face.
	/// </param>
	public Face (BuildingMesh parent, Vector3 dr, Vector3 dl)
	{
		_parent = parent;
		
		_boundaries.Add(dr);
		_boundaries.Add(dl);
		_boundaries.Add(new Vector3(dl.x, dl.y + _parent.Height, dl.z));
		_boundaries.Add(new Vector3(dr.x, dr.y + _parent.Height, dr.z));
		
		this.CalculateNormal();
		this.CalculateWidth();
	}
	
	
	// methods
	
	/// <summary>
	/// Calculates the normal.
	/// </summary>
	private void CalculateNormal ()
	{
		Vector3 edge1 = new Vector3(_boundaries[1].x - _boundaries[0].x,
									_boundaries[1].y - _boundaries[0].y,
									_boundaries[1].z - _boundaries[0].z);
		
		Vector3 edge2 = new Vector3(_boundaries[3].x - _boundaries[0].x,
									_boundaries[3].y - _boundaries[0].y,
									_boundaries[3].z - _boundaries[0].z);
		
		_normal = Vector3.Cross(edge1, edge2);
		_normal.Normalize();
	}
	
	/// <summary>
	/// Calculates the width.
	/// </summary>
	private void CalculateWidth ()
	{
		Vector3 edge = new Vector3(_boundaries[0].x - _boundaries[1].x,
								   0f,
								   _boundaries[0].z - _boundaries[1].z);
		_width = edge.magnitude;
	}
	
	/// <summary>
	/// Draw the face.
	/// </summary>
	public void Draw ()
	{
		GL.PushMatrix();
		GL.Begin(GL.QUADS);
		foreach (Vector3 v in _boundaries)
			GL.Vertex(v);
		GL.End();
		GL.PopMatrix();
	}
}
