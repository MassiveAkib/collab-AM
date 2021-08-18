////////////////
private var z : float;
private var y : float =0;
var turn = 0.0;
var tur = 0.0;
var maxtur = 0.0;
var mintur = 0.0;
var speed = 0.0;
var maxspeed = 20.0;
var minspeed = -0.25;
var acceleration = 0.0;
var acceleration2 = 0.0;
var mass = 5;
var angularDrag = 0.5;
static var gameover=0;
var crashforce = 0;
////////////////
var Audio : GameObject;
var Audio_A : GameObject;
var Audio_Crash : GameObject;
var Audio_Warning : GameObject;
var CS : GameObject;
var rotor : GameObject;
var rotor2 : GameObject;
var on_off_rotate : GameObject;
var mainCamera: Camera;
var ArialCamera : Camera;
function Start (){
	
	if(rigidbody)
		Destroy(rigidbody);
	
	gameObject.AddComponent(Rigidbody);
	rigidbody.mass = mass;
	rigidbody.angularDrag = angularDrag;
	rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.centerOfMass = Vector3 (0, -2, 0);    
}

function Update () {
   
    speed += Input.GetAxis("Vertical") * acceleration * Time.deltaTime;
   if( speed > maxspeed ){
      speed = maxspeed;
   } else if ( speed < minspeed ){
      speed = minspeed;
   }
    transform.Translate(0, 0, speed * Time.deltaTime);
    
    y = Input.GetAxis("Throttle") * tur * Time.deltaTime;
    transform.Translate(0, y, 0 * Time.deltaTime);
    tur += Input.GetAxis("Throttle") * acceleration2 * Time.deltaTime;
   if( tur > maxtur ){
      tur = maxtur;
   } else if ( tur < mintur ){
      tur = mintur;
   }
   
     z = Input.GetAxis("Horizontal") * Time.deltaTime* -turn;
    transform.Rotate(0, z, 0);
    				
	audio.pitch = 0.5f + Mathf.Abs(speed) *0.01f;
    rigidbody.velocity = Vector3(0,y,0);
    
    transform.Rotate (0,z,0);

     airplaneangley= transform.eulerAngles.y; 

	if ((gameover==2) && (Input.GetKey ("enter"))||(gameover==2) &&(Input.GetKey ("return")))	{
	gameover=0;
	rigidbody.useGravity = false;
	transform.position = Vector3(0, 1.67, 0);
	transform.eulerAngles = Vector3(0,0,0);
	}

	if (gameover==1)	{
	rigidbody.AddRelativeForce (0, 0, crashforce);
	gameover=2;
	}
	if (Input.GetKey("f2")){
		speed=0;
		gameover=0;
		rigidbody.useGravity = false;
		Application.LoadLevel(0);
		}
 
} 

function OnCollisionEnter(collision : Collision) {
	if (groundtrigger.triggered==0) {
	groundtrigger.triggered=1;
	CS.GetComponent("moverscript").enabled = false;
	CS.GetComponent("on_off_rotate").enabled = false;
    rotor.GetComponent("rotor").enabled = false;
    rotor2.GetComponent("rotor").enabled = false;
    mainCamera.enabled = false;;
    
	crashforce= speed*10000;
	acceleration=0;
	speed=0;
	turn=0;
	tur=0;
	gameover=1;
	rigidbody.useGravity = true;
    ArialCamera.enabled = true;
	Audio.audio.Pause();
	Audio_A.audio.Pause();
	Audio_Crash.audio.Play();
	Audio_Warning.audio.Play();
	if (Input.GetKeyDown("r")){
	Audio_Warning.audio.Play();
	
	}
	}
}

function OnGUI (){
   GUI.color = Color.black;  	
   GUI.Box (Rect (0,Screen.height - 30,150,50),"Скорость  " +(Mathf.Round(speed)));
   GUI.Box (Rect (160,Screen.height - 30,150,80),"Y  " +(Mathf.Round(transform.localPosition.y)));
   GUI.Box (Rect (320,Screen.height - 30,150,80),"X  " +(Mathf.Round(transform.localPosition.x)));
   GUI.Box (Rect (480,Screen.height - 30,150,80),"Z  " +(Mathf.Round(transform.localPosition.z)));

   
  
}
