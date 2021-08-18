#pragma strict

var turn : float = 0.0;
var maxspeed = 20.0;
var minspeed = -0.25;
var acceleration = 0.0;


function Update () {
   
    turn += Input.GetAxis("Throttle") * acceleration * Time.deltaTime;
   if( turn > maxspeed ){
      turn = maxspeed;
   } else if ( turn < minspeed ){
      turn = minspeed;
   }
    transform.Rotate( 0, turn ,0 * Time.deltaTime);
    audio.pitch = 0.5f + Mathf.Abs(turn) *0.08f;

}