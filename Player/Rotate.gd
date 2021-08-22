extends Spatial
var zoom = Vector3(0,0,0)
var panning = false
onready var vertically = get_node("Horizontally/Vertically")
onready var horizontally = get_node("Horizontally")
onready var sprite = get_node("/root/Spatial/KinematicBody") as KinematicBody
onready var posit3D = get_node("/root/Spatial/KinematicBody/Position3D") as Position3D
var min_arm_angle = -85
var max_arm_angle = -25
var changing_angle = false
var being_pressed = false
var inital_pos_x
var inital_pos_y
var controlling = false

func _process(delta):
	vertically.set_translation(vertically.get_translation().linear_interpolate(zoom, delta))
	var camera_pos = get_viewport().get_camera().global_transform.origin
	

func _ready():
	pass # Replace with function body.
	#new_arm_angle.x = clamp(new_arm_angle.x, deg2rad(min_arm_angle), deg2rad(max_arm_angle))

func _input(event):
	if event is InputEventMouseMotion:
		if being_pressed and changing_angle:
			if(controlling):
				var new_x = (-event.position.y/100)+inital_pos_y 
				var new_angle = Vector3(new_x, 0, 0)
				vertically.set_rotation(new_angle)
			else:
				var new_y = (-event.position.x/100)+inital_pos_x 
				var new_angle = Vector3(0, new_y, 0)
				horizontally.set_rotation(new_angle)
				print(horizontally.get_rotation())
	if event is InputEventMouseButton:
		being_pressed = event.is_pressed()
		if event.button_mask == BUTTON_RIGHT: #BUTTON_WHEEL_DOWN:
			changing_angle = true
			inital_pos_x = event.position.x/100 +horizontally.get_rotation().y
			inital_pos_y = event.position.y/100 +vertically.get_rotation().x
		else: changing_angle = false
	if event.is_action_pressed("ctrl"):
		controlling = true
	if event.is_action_released("ctrl"):
		controlling = false
			
