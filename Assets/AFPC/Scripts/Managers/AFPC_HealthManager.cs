using UnityEngine;
using UnityEngine.UI;

public class AFPC_HealthManager : MonoBehaviour {
	public int health = 100;
	public int maxHealth = 100;
	public int minHealth = 0;
	public Image healthDecreaseEffect;
	public bool useHealthDecreaseAudioEffects = true;
	public bool useTextToShowHealth = true;
	public bool useBarToShowHealth = true;

	public bool hasFallDamage = true;
	public float heightInAitWithNoDamage = 10f;
	public int damagePerUnitHeightInAir = 5;

	public AudioClip healthDecreaseAudio;
	public float audioVolume = 0.87f;
	public Color healthDecreaseScreenColor = new Color (255f, 0f, 0f, 255f);
	public Text healthText;
	public Image healthBar;
	public Image healthMaster;
	public float timeToSmoothFillAmount = 0.2f;
	public GameObject kickBack; // the kickBack gameobject 
	[HideInInspector]
	public bool dieCallBool = false;

	private float fallHeight = 0f; // fall height of the player
	private float fallStartHeight = 0f; // the height at which player started falling
	private bool falling = false; // boolean to determine if we are falling or not
	private AudioSource audioSource;
	private float fillAmountV; // used in smoothdamp
	private Color startingColor;
	private float colorV, colorV1;
	private bool damaged = false;
	private Animator kickBackAnimator;
	private int healthDecreaseAnimHashId;
	private AFPC_PlayerMovement afpcPlayer;
	private AFPC_SpawnManager spawnManager;
	private Vector3 playerVelocity;

	private AFPC_PlayerMovement.PlayerType prevPLayerType;
	// Use this for initialization
	void Start () 
	{
		if (GetComponent<AFPC_SpawnManager> ())
			spawnManager = GetComponent<AFPC_SpawnManager> ();
		afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			if (kickBack != null) {
				kickBackAnimator = kickBack.GetComponent<Animator> ();
				healthDecreaseAnimHashId = Animator.StringToHash ("DoHealthDecreaseKickBack");
			}
			health = Mathf.Clamp (health, minHealth, maxHealth); // To make sure user can't enter health value which is not in range
			if (healthDecreaseEffect != null) {
				startingColor = healthDecreaseEffect.color;
				healthDecreaseEffect.gameObject.SetActive (false);
			}
			if (!GetComponent<AudioSource> () && useHealthDecreaseAudioEffects) {
				gameObject.AddComponent<AudioSource> ();
				audioSource = GetComponent<AudioSource> ();
			} else if(useHealthDecreaseAudioEffects && GetComponent<AudioSource>()){
				audioSource = GetComponent<AudioSource> ();
			}
			if (useHealthDecreaseAudioEffects) 
			{
				audioSource.volume = audioVolume;
				audioSource.playOnAwake = false;
			}
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator)
		{
			if (healthMaster != null)
				healthMaster.gameObject.SetActive (false);
		}
	}
		
	void UpdatePainFade()
	{
		if (healthDecreaseEffect != null) {
			if (damaged) {
				startingColor.a = Mathf.SmoothDamp (startingColor.a, 0f, ref colorV, 0.5f);	
				healthDecreaseEffect.color = startingColor;
				if (startingColor.a <= 0.05) {
					damaged = false;
					healthDecreaseEffect.gameObject.SetActive (false);
				}
			}

			healthDecreaseEffect.color = startingColor;
		}
	}


	void ChangePlayerType()
	{
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {

			if(healthMaster != null)
				healthMaster.gameObject.SetActive (true);
			
			if (kickBack != null) {
				kickBackAnimator = kickBack.GetComponent<Animator> ();
				healthDecreaseAnimHashId = Animator.StringToHash ("DoHealthDecreaseKickBack");
			}
			health = Mathf.Clamp (health, minHealth, maxHealth); // To make sure user can't enter health value which is not in range
			if (healthDecreaseEffect != null) {
				startingColor = healthDecreaseEffect.color;
				healthDecreaseEffect.gameObject.SetActive (false);
			}
			if (!GetComponent<AudioSource> () && useHealthDecreaseAudioEffects) {
				gameObject.AddComponent<AudioSource> ();
				audioSource = GetComponent<AudioSource> ();
			} else if(useHealthDecreaseAudioEffects && GetComponent<AudioSource>()){
				audioSource = GetComponent<AudioSource> ();
			}
			if (useHealthDecreaseAudioEffects) 
			{
				audioSource.volume = audioVolume;
				audioSource.playOnAwake = false;
			}
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator)
		{
			if (healthMaster != null)
				healthMaster.gameObject.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (prevPLayerType != afpcPlayer.playerType)
			ChangePlayerType ();
		
		prevPLayerType = afpcPlayer.playerType;

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) 
		{
			
			health = Mathf.Clamp (health, minHealth, maxHealth); // Clamp health to make sure it is always in the specified range

			if (healthBar != null && healthBar.type == Image.Type.Filled && useBarToShowHealth) {
				float targetFillAmount = healthBar.GetComponent<AFPC_UIBar> ().ConvertValuesToFillAmountValue (health, minHealth, maxHealth, 0f, 1f);
				healthBar.fillAmount = Mathf.SmoothDamp (healthBar.fillAmount, targetFillAmount, ref fillAmountV, timeToSmoothFillAmount);
			}

			if (healthText != null && useTextToShowHealth)
				healthText.text = health.ToString ();

			if (health <= minHealth && spawnManager != null && !dieCallBool) 
			{
				dieCallBool = true;
				spawnManager.Die ();
			}

			if (hasFallDamage)
				ApplyFallDamage ();
			
			UpdatePainFade ();
		}else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator)
		{
			if (healthMaster != null)
				healthMaster.gameObject.SetActive (false);
		}

	}

	void ApplyFallDamage()
	{
		if (afpcPlayer != null) 
		{
			if (!afpcPlayer.isGrounded && !afpcPlayer.CanClimb && !afpcPlayer.IsSwimming) 
			{
				if (!falling) 
				{
					falling = true;
					fallStartHeight = transform.position.y;
				}
			}
			if (afpcPlayer.CanClimb || afpcPlayer.IsSwimming)
				falling = false;
			
			if (afpcPlayer.isGrounded && !afpcPlayer.CanClimb && !afpcPlayer.IsSwimming)
			{
				if (falling) 
				{
					falling = false;
					fallHeight = fallStartHeight - heightInAitWithNoDamage;
					if (transform.position.y < fallHeight)
					{
						int damage = damagePerUnitHeightInAir * (int)fallHeight;
						if (damage < 0)
							damage *= -1;
						DecreaseHealth (damage);
					}
				}
				fallStartHeight = transform.position.y;
			}
		}	
	}

	void MakeDamageScreenVisible()
	{
		startingColor.a = Mathf.SmoothDamp (healthDecreaseEffect.color.a, 255f, ref colorV, 0.1f);
	}

	public void DecreaseHealth(int healthToDecrease)
	{
		healthToDecrease = Mathf.Clamp (healthToDecrease, minHealth, maxHealth);
		damaged = true;
		if (health > minHealth) 
		{
			health -= healthToDecrease;
			if(kickBackAnimator != null)
				kickBackAnimator.SetTrigger (healthDecreaseAnimHashId);
			if (healthDecreaseAudio != null && useHealthDecreaseAudioEffects) 
			{
				audioSource.clip = healthDecreaseAudio;
				audioSource.Play ();
			}
			healthDecreaseEffect.gameObject.SetActive (true);
			MakeDamageScreenVisible ();
		}
	}

	public void IncreaseHealth(int healthToIncrease)
	{
		if(health < (maxHealth + healthToIncrease))
			health += healthToIncrease;
	}
}