
function Update () {
 
 
    if(Input.GetKeyDown("r"))
 
    {
 
        GetComponent("move").enabled = !GetComponent("move").enabled;
//        GetComponent("Speedometer").enabled = !GetComponent("Speedometer").enabled;
    }
     if(Input.GetKeyDown("r")) {
		 	if(audio.mute)
				audio.mute = false;
			else
				audio.mute = true;
		}
}