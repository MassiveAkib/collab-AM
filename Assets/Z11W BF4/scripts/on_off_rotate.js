function Update () {
 
 
    if(Input.GetKeyDown("r"))
 
    {
 
    
        GetComponent("moverscript").enabled = !GetComponent("moverscript").enabled;
    }
    
}