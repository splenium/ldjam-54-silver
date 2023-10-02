using Godot;

public enum EnumEFaceState
{
	OH,
	HAPPY,
	THUG,
	SHOCKED,
}

public partial class RaKoonAvatar : Node3D
{
	private Quaternion _originalRotation;

	public Color LightColor;

	[Export]
	public int FaceSelection;

	[Export]
	public float MoveAnimationSpeed;

	[Export]
	public float MoveRotationAnimationSpeed;
	[Export]
	public float MoveRotationAnimationAmplitude;


	[Export]
	public CsgMesh3D HeadMesh;
	public ShaderMaterial HeadMaterial => (ShaderMaterial)HeadMesh.Material;
	[Export]
	public CsgMesh3D BodyMesh;
	public ShaderMaterial BodyMaterial => (ShaderMaterial)BodyMesh.Material;
	[Export]
	public CsgMesh3D TritonBodyMesh;
	public ShaderMaterial TritonBodyMaterial => (ShaderMaterial)TritonBodyMesh.Material;

	[Export]
	public Texture2D OHBackground;
	[Export]
	public Texture2D OHOutline;
	[Export]
	public Texture2D OHHighlight;
	[Export]
	public Texture2D HAPPYBackground;
	[Export]
	public Texture2D HAPPYOutline;
	[Export]
	public Texture2D HAPPYHighlight;
	[Export]
	public Texture2D THUGBackground;
	[Export]
	public Texture2D THUGOutline;
	[Export]
	public Texture2D THUGHighlight;
	[Export]
	public Texture2D SHOCKEDBackground;
	[Export]
	public Texture2D SHOCKEDOutline;
	[Export]
	public Texture2D SHOCKEDHighlight;
	[Export]
	public bool IsMoving;
	[Export]
	public bool IsLeft;
	[Export]
	public bool IsGhost;
	[Export]
	public bool IsTriton;
	public void SetFace(EnumEFaceState state)
	{
		switch (state)
		{
			case EnumEFaceState.OH:
				HeadMaterial.SetShaderParameter("_headBGTex", OHBackground);
				HeadMaterial.SetShaderParameter("_headOutlineTex", OHOutline);
				HeadMaterial.SetShaderParameter("_headHighLightTex", OHHighlight);
				break;
			case EnumEFaceState.HAPPY:
				HeadMaterial.SetShaderParameter("_headBGTex", HAPPYBackground);
				HeadMaterial.SetShaderParameter("_headOutlineTex", HAPPYOutline);
				HeadMaterial.SetShaderParameter("_headHighLightTex", HAPPYHighlight);
				break;
			case EnumEFaceState.THUG:
				HeadMaterial.SetShaderParameter("_headBGTex", THUGBackground);
				HeadMaterial.SetShaderParameter("_headOutlineTex", THUGOutline);
				HeadMaterial.SetShaderParameter("_headHighLightTex", THUGHighlight);
				break;
			case EnumEFaceState.SHOCKED:
				HeadMaterial.SetShaderParameter("_headBGTex", SHOCKEDBackground);
				HeadMaterial.SetShaderParameter("_headOutlineTex", SHOCKEDOutline);
				HeadMaterial.SetShaderParameter("_headHighLightTex", SHOCKEDHighlight);
				break;
			default:
				break;
		}

	}

	public override void _Ready()
	{
		_totalTime = 0;
		//_originalRotation
	}
	float _totalTime = 0;

	public override void _Process(double delta)
	{
		float scaleY = Mathf.Lerp(1.0f, 0.99f, Mathf.Sin(_totalTime * MoveAnimationSpeed));
		this.Scale = new Vector3((IsLeft ? 1.0f : -1.0f), scaleY, this.Scale.Z);

		SetFace((EnumEFaceState)(FaceSelection % 4));

		this.Rotation = Vector3.Forward * Mathf.Sin(_totalTime * MoveRotationAnimationSpeed) * MoveRotationAnimationAmplitude * (IsMoving ? 1.0f : 0.0f);
		_totalTime += (float)delta;
		float ghostness = IsGhost ? 1.0f : 0.0f;

		HeadMaterial.SetShaderParameter("_lightColor", LightColor);
		HeadMaterial.SetShaderParameter("_ghostness", ghostness);

		if (IsTriton)
		{
			BodyMesh.Visible = false;
			TritonBodyMesh.Visible = true;
			TritonBodyMaterial.SetShaderParameter("_lightColor", LightColor);
			TritonBodyMaterial.SetShaderParameter("_ghostness", ghostness);
		}
		else
		{
			BodyMesh.Visible = true;
			TritonBodyMesh.Visible = false;
			BodyMaterial.SetShaderParameter("_lightColor", LightColor);
			BodyMaterial.SetShaderParameter("_ghostness", ghostness);
		}
		

	}
}
