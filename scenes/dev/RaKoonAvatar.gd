extends Node3D

class_name RaKoonAvatar

var LightColor: Color

@export var FaceSelection: int

@export var MoveAnimationSpeed: float

@export var MoveRotationAnimationSpeed: float

@export var MoveRotationAnimationAmplitude: float


@export var HeadMesh: CSGMesh3D
@export var BodyMesh: CSGMesh3D
@export var TritonBodyMesh: CSGMesh3D

@export var OHBackground: Texture2D
@export var OHOutline: Texture2D
@export var OHHighlight: Texture2D
@export var HAPPYBackground: Texture2D
@export var HAPPYOutline: Texture2D
@export var HAPPYHighlight: Texture2D
@export var THUGBackground: Texture2D
@export var THUGOutline: Texture2D
@export var THUGHighlight: Texture2D
@export var SHOCKEDBackground: Texture2D
@export var SHOCKEDOutline: Texture2D
@export var SHOCKEDHighlight: Texture2D
@export var IsMoving: bool
@export var IsLeft: bool
@export var IsGhost: bool
@export var IsTriton: bool

func SetFace(state: Enums.EFaceState) -> void:
    match state:
        Enums.EFaceState.OH:
            HeadMesh.material.set_shader_parameter("_headBGTex", OHBackground)
            HeadMesh.material.set_shader_parameter("_headOutlineTex", OHOutline)
            HeadMesh.material.set_shader_parameter("_headHighLightTex", OHHighlight)
        Enums.EFaceState.HAPPY:
            HeadMesh.material.set_shader_parameter("_headBGTex", HAPPYBackground)
            HeadMesh.material.set_shader_parameter("_headOutlineTex", HAPPYOutline)
            HeadMesh.material.set_shader_parameter("_headHighLightTex", HAPPYHighlight)
        Enums.EFaceState.THUG:
            HeadMesh.material.set_shader_parameter("_headBGTex", THUGBackground)
            HeadMesh.material.set_shader_parameter("_headOutlineTex", THUGOutline)
            HeadMesh.material.set_shader_parameter("_headHighLightTex", THUGHighlight)
        Enums.EFaceState.SHOCKED:
            HeadMesh.material.set_shader_parameter("_headBGTex", SHOCKEDBackground)
            HeadMesh.material.set_shader_parameter("_headOutlineTex", SHOCKEDOutline)
            HeadMesh.material.set_shader_parameter("_headHighLightTex", SHOCKEDHighlight)
        _:
            pass

func _ready() -> void:
    _totalTime = 0

var _totalTime: float = 0

func _process(delta: float) -> void:
    var scaleY: float = lerp(1.0, 0.99, sin(_totalTime * MoveAnimationSpeed))
    scale = Vector3(1.0 if IsLeft else -1.0, scaleY, scale.z)

    SetFace((FaceSelection % 4) as Enums.EFaceState)

    rotation = Vector3(0, 0, sin(_totalTime * MoveRotationAnimationSpeed) * MoveRotationAnimationAmplitude * (1.0 if IsMoving else 0.0))
    _totalTime += delta
    var ghostness: float = 1.0 if IsGhost else 0.0

    HeadMesh.material.set_shader_parameter("_lightColor", LightColor)
    HeadMesh.material.set_shader_parameter("_ghostness", ghostness)

    if IsTriton:
        BodyMesh.visible = false
        TritonBodyMesh.visible = true
        TritonBodyMesh.material.set_shader_parameter("_lightColor", LightColor)
        TritonBodyMesh.material.set_shader_parameter("_ghostness", ghostness)
    else:
        BodyMesh.visible = true
        TritonBodyMesh.visible = false
        BodyMesh.material.set_shader_parameter("_lightColor", LightColor)
        BodyMesh.material.set_shader_parameter("_ghostness", ghostness)
    pass
