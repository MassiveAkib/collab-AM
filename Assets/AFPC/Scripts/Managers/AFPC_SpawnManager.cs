using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AFPC_SpawnManager : MonoBehaviour {

	public bool hasDroppablesOnDie = false;
	public bool spawnAtDiePoint = false;
	public Image fadePanel;
	public float timeToFadeIn = 1f, timeToFadeOut = 1f;
	public GameObject[] Droppables;
	public Transform[] spawnPoints;
	public float dieDropForce = 5f;
	[Tooltip("In seconds")]
	public float timeToRespawn = 5f;
	[Tooltip("In Seconds")]
	public float timeToApplyForce = 2f;

	private bool hasDied = false;
	public bool HasDied{get{ return hasDied;}}
	private AFPC_PlayerMovement afpcPlayer;
	private Rigidbody rgbd;
	private AFPC_HealthManager healthManager;
	private AFPC_StaminaManager staminaManager;
	private AFPC_OxygenManager oxygenManager;
	private CapsuleCollider playerCapsule;
	private Color initialFadePanelColor;
	private bool respawnInvokeFlag = false;
	private RigidbodyConstraints initialConstraints;
	private bool fadedOut = false;
 	// Use this for initialization
	void Start () {
		if (GetComponent<AFPC_HealthManager> ())
			healthManager = GetComponent<AFPC_HealthManager> ();
		if (GetComponent<CapsuleCollider> ())
			playerCapsule = GetComponent<CapsuleCollider> ();
		if(GetComponent<AFPC_PlayerMovement>())
			afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
		if (GetComponent<Rigidbody> ()) {
			rgbd = GetComponent<Rigidbody> ();
			initialConstraints = rgbd.constraints;
		}
		if (GetComponent<AFPC_StaminaManager> ())
			staminaManager = GetComponent<AFPC_StaminaManager> ();
		if (GetComponent<AFPC_OxygenManager> ())
			oxygenManager = GetComponent<AFPC_OxygenManager> ();
		
		if (fadePanel != null)
		{
			initialFadePanelColor = fadePanel.color;
			initialFadePanelColor.a = 0f;
			fadePanel.color = initialFadePanelColor;
			fadePanel.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!respawnInvokeFlag && hasDied && fadedOut) 
		{
			respawnInvokeFlag = true;
			StartCoroutine (Respawn ());
		}
	}

	Vector3 CalcSpawnPos(Vector3 pos)
	{
		Vector3 value = new Vector3();
		float distance = (afpcPlayer.transform.localScale.y * playerCapsule.height / 2f) + afpcPlayer.groundCheckDistance;
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (pos, Vector3.down, out hit, distance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			value = hit.point + new Vector3(0f, afpcPlayer.transform.localScale.y * playerCapsule.height / 2f, 0f);
		} else {
			value = pos;	
		}


		return value;
	}

	IEnumerator Respawn()
	{
		if (hasDied && fadedOut)
		{
			yield return new WaitForSeconds (timeToRespawn);
			int n = Random.Range (0, spawnPoints.Length - 1);
			if (spawnAtDiePoint) {
				afpcPlayer.transform.rotation = Quaternion.identity;
				rgbd.constraints = initialConstraints;
				hasDied = false;
				rgbd.useGravity = true;
				if (healthManager != null) {
					healthManager.dieCallBool = false;
					healthManager.health = healthManager.maxHealth;
				}
				if (staminaManager != null)
					staminaManager.currentStamina = staminaManager.maxStamina;
				if (oxygenManager != null)
					oxygenManager.currentOxygen = oxygenManager.maxOxygen;
			} 
			else if (!spawnAtDiePoint && spawnPoints [n] != null) {
				afpcPlayer.transform.rotation = Quaternion.identity;
				afpcPlayer.transform.position = CalcSpawnPos (spawnPoints [n].position);
				rgbd.constraints = initialConstraints;
				hasDied = false;
				rgbd.useGravity = true;
				if (healthManager != null) {
					healthManager.dieCallBool = false;
					healthManager.health = healthManager.maxHealth;
				}
				if (staminaManager != null)
					staminaManager.currentStamina = staminaManager.maxStamina;
				if (oxygenManager != null)
					oxygenManager.currentOxygen = oxygenManager.maxOxygen;
			} else {
				Debug.LogError ("Spawn Point at index " + n.ToString () + " is not defined!!");
			}
			hasDied = false;
			respawnInvokeFlag = false;
			StartCoroutine (FadeIn ());
		} else 
		{
			yield return null;
		}
	}

	void DropObjects()
	{
		foreach (GameObject droppable in Droppables)
		{
			Instantiate (droppable, transform.position, droppable.transform.rotation);
		}
	}

	public void Die()
	{
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			if (healthManager.IsInvoking ())
				healthManager.CancelInvoke ();
			if (afpcPlayer.IsInvoking ())
				afpcPlayer.CancelInvoke ();
			if (oxygenManager.IsInvoking ())
				oxygenManager.CancelInvoke ();
			if (staminaManager.IsInvoking ())
				staminaManager.CancelInvoke ();
			if (afpcPlayer.footstepsDetectionMode == AFPC_PlayerMovement.FootstepsDetectionMode.basedOnTags) {
				if (afpcPlayer.footstepManagerTag.IsInvoking ())
					afpcPlayer.footstepManagerTag.CancelInvoke ();
			} else if (afpcPlayer.footstepsDetectionMode == AFPC_PlayerMovement.FootstepsDetectionMode.basedOnTextures) {
				if (afpcPlayer.footstepManagerTexture.IsInvoking ())
					afpcPlayer.footstepManagerTexture.CancelInvoke ();
			}
			hasDied = true;
			rgbd.constraints = RigidbodyConstraints.None;
			StartCoroutine (ApplyForce ());
			rgbd.isKinematic = false;

			if (hasDroppablesOnDie && Droppables != null)
				DropObjects ();
			StartCoroutine (FadeOut ());
			respawnInvokeFlag = false;
		}
	}


	IEnumerator ApplyForce()
	{
		if (hasDied) {
			Vector3 Pos = transform.position;
			Pos.y = transform.position.y + (transform.localScale.y * playerCapsule.height) / 2f - 0.25f;
			Vector3 forcePos = new Vector3 (Random.Range (-dieDropForce, dieDropForce), 0f, Random.Range (-dieDropForce, dieDropForce));
			rgbd.AddForceAtPosition (forcePos, Pos, ForceMode.VelocityChange);
			yield return new WaitForSeconds (timeToApplyForce);
		} else {
			yield return null;
		}
	}

	IEnumerator FadeOut()
	{
		yield return new WaitForSeconds (timeToFadeOut);
		fadePanel.gameObject.SetActive (true);
		float timeElapsed = 0f;
		while (timeElapsed < timeToFadeOut) 
		{
			Color targetColor = new Color (initialFadePanelColor.r, initialFadePanelColor.g, initialFadePanelColor.b, 1f);
			fadePanel.color = Color.Lerp (fadePanel.color, targetColor, timeElapsed / timeToFadeOut);
			timeElapsed += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		fadedOut = true;
	}

	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds (timeToFadeIn);
		float timeElapsed = 0f;
		initialFadePanelColor.a = 1f;
		fadePanel.color = initialFadePanelColor;
		while (timeElapsed < timeToFadeIn) 
		{
			Color targetColor = new Color (initialFadePanelColor.r, initialFadePanelColor.g, initialFadePanelColor.b, 0f);
			fadePanel.color = Color.Lerp (fadePanel.color, targetColor, timeElapsed / timeToFadeIn);
			timeElapsed += Time.deltaTime;
			yield return new WaitForFixedUpdate ();
		}
		fadedOut = false;
		fadePanel.gameObject.SetActive (false);
	}
}
