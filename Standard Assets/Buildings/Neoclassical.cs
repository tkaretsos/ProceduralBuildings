using UnityEngine;
using System;

public class Neoclassical : BuildingMesh
{
	// fields
	private const float _window_width_min = 1.5f;
	private const float _window_width_max = 1.75f;
	
	private const float _inbetween_space_min = 2f;
	private const float _inbetween_space_max = 2.25f;
	
	
	// constructors
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Neoclassical"/> class.
	/// The boundaries of the base of this building must be given in 
	/// clockwise order.
	/// </summary>
	/// <param name='p1'>
	/// A point in space.
	/// </param>
	/// <param name='p2'>
	/// A point in space.
	/// </param>
	/// <param name='p3'>
	/// A point in space.
	/// </param>
	/// <param name='p4'>
	/// A point in space.
	/// </param>
	public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	: base(p1, p2, p3, p4, BuildingType.Neoclassical)
	{
		this.FloorHeight = UnityEngine.Random.Range(4f, 5f);
//		this.FloorNumber = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});
		this.FloorNumber = 1;
	}
	
	
	// methods
	
	/// <summary>
	/// Constructs the faces.
	/// </summary>
	public override void ConstructFaces ()
	{
		base.ConstructFaces();
	}
	
	public void ConstructFaceComponents ()
	{
		if (Faces.Count == 0) throw new Exception("There are no faces to construct the components.");
		
		float component_width = UnityEngine.Random.Range(_window_width_min, _window_width_max);
		float inbetween_space = UnityEngine.Random.Range(_inbetween_space_min, _inbetween_space_max);
		
		float offset = 0f;
		Vector3 dr;
		Vector3 dl;
		
		foreach (Face face in Faces)
		{
			int components_no = GetComponentsNumber(face.Width, component_width, inbetween_space);
			
			// ground floor
			float effective_width = components_no * component_width + (components_no - 1) * inbetween_space;
			float distance_from_edge = (face.Width - effective_width) / 2;
			
			offset = distance_from_edge;
			
			for (int i = 0; i < components_no; ++i)
			{
				dr = face.Boundaries[0] - face.Right * offset;
				dl = dr - face.Right * component_width;
				offset += component_width;
				face.AddFaceComponent(new FaceComponent(face, component_width, FloorHeight * 3 / 5, dr, dl));
				offset += inbetween_space;
			}
		}
	}
	
	private int GetComponentsNumber (float face_width, float comp_width, float space)
	{
		return Mathf.CeilToInt((face_width - 2f) / (comp_width + space));
	}
}
