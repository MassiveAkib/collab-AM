//Unity 2.61

//~ Groundsensor ist das Würfelprimitive an der Flugzeugunterseite. Dieses Primitive repräsentiert unsere Räder. Wenn dieses Primitive Bodenkontakt 
//~ hat wird die triggered Variable auf 1 gestellt. Hat es keinen Bodenkontakt befinden wir uns in der Luft.

//~ Groundsensor is the cube primitive at the bottom side of our plane. This primitive represents our wheels. When it has ground contact the triggered 
//~ variable is set to 1. When not this variable is set to 0, and we are in air.


static var triggered=0;

function OnTriggerEnter  (other : Collider) {
triggered=1;
}

function OnTriggerExit  (other : Collider) {
triggered=0;
}