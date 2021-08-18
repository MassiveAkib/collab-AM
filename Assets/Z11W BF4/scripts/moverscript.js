
function Update(){
    var v = Input.GetAxis("Vertical");
    var h = Input.GetAxis("Horizontal");
    transform.localEulerAngles.z = h*15;
    transform.localEulerAngles.x = v*5; 
}