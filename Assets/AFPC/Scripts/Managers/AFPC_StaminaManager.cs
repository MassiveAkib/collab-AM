using UnityEngine;
using UnityEngine.UI;

public class AFPC_StaminaManager : MonoBehaviour {

	public int maxStamina = 100;
	public int minStamina = 0;
	public int currentStamina = 100;
	public bool hideStaminaBar = true; // if enabled then hide stamina bar when not in use means when player is not running, hide the bar
	public Vector3 hidePosition = new Vector3(-350f, 500f, 0f);
	public Image staminaMaster;
	public bool useGaspSound = true;
	public bool useTextToShowStamina = true;
	public bool useBarToShowStamina = true;
	public float gaspVolume = 0.9f; // the volume of gaspSound
	public AudioClip gaspSound; // sound to play when player reaches 0 stamina
	public Image staminaBar;
	public Text staminaText;
	public float timeToSmoothFillAmount = 1f;
	public float timeToHideStaminaBar = 0.5f;
	[Tooltip("In Seconds")]
	public float staminaDecreaseTime = 1f;	// Time taken to decrease Stamina by staminaToDecrease in seconds
	public int staminaToDecrease = 1;
	public int staminaToIncrease = 4;
	public float staminaIncreaseTime = 2f;


	[HideInInspector]
	public bool useStaminaManager = true;
	private AFPC_PlayerMovement afpcPlayer; // The Player
	private float fillAmountV;	// used in smoothdamp
	private Vector3 startingPos; // The starting position of stamina master
	private Vector3 staminaMasterPositionV;
	private AudioSource _audioSrc; // the audioSource attached to this gameobject

	private bool i = false;
	private AFPC_PlayerMovement.PlayerType prevPlayerType;
	// Use this for initialization
	void Start () 
	{
		afpcPlayer = GetComponent<AFPC_PlayerMovement> ();

		if (staminaMaster != null) {
			startingPos = staminaMaster.rectTransform.anchoredPosition;
			if (hideStaminaBar)
				staminaMaster.rectTransform.anchoredPosition = hidePosition;
			staminaMaster.gameObject.SetActive (true);
		}

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			if (!GetComponent<AudioSource> () && useGaspSound) {	
				gameObject.transform.GetChild (0).gameObject.AddComponent<AudioSource> ();
				_audioSrc = gameObject.transform.GetChild (0).gameObject.GetComponent<AudioSource> ();
			} else if (useGaspSound && GetComponent<AudioSource> ()) {
				_audioSrc = GetComponent<AudioSource> ();
			}

			if (afpcPlayer.infiniteRunning)
				useStaminaManager = false;
			else
				useStaminaManager = true; // Only enable stamina system when infinite running is disabled 
			currentStamina = Mathf.Clamp (currentStamina, minStamina, maxStamina); //To Make sure that the user can't enter a non ranging value of stamina
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if (staminaMaster != null)
				staminaMaster.gameObject.SetActive (false);
		}
	}

	void ChangePlayerType()
	{
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			if (staminaMaster != null)
				staminaMaster.gameObject.SetActive (true);
			if (!GetComponent<AudioSource> () && useGaspSound) {	
				gameObject.AddComponent<AudioSource> ();
				_audioSrc = GetComponent<AudioSource> ();
			} else if (useGaspSound && GetComponent<AudioSource> ()) {
				_audioSrc = GetComponent<AudioSource> ();
			}
				
			if (afpcPlayer.infiniteRunning)
				useStaminaManager = false;
			else
				useStaminaManager = true; // Only enable stamina system when infinite running is disabled 
			currentStamina = Mathf.Clamp (currentStamina, minStamina, maxStamina); //To Make sure that the user can't enter a non ranging value of stamina
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if (staminaMaster != null)
				staminaMaster.gameObject.SetActive (false);
		}
	}

	public void DecreaseStamina ()
	{
		// if (afpcPlayer.IsRunning && !afpcPlayer.infiniteRunning && currentStamina > minStamina && useStaminaManager) 
		// {
		// 	currentStamina -= staminaToDecrease;
		// }
	}

	public void IncreaseStamina()
	{
		// if (!afpcPlayer.IsRunning && !afpcPlayer.infiniteRunning && currentStamina >= minStamina && currentStamina <= maxStamina && useStaminaManager) 
		// {
		// 	currentStamina += staminaToIncrease;
		// 	i = false;
		// }
	}

	public void IncreaseStaminaByAmount(int amountToIncrease)
	{
		// if (!afpcPlayer.infiniteRunning && currentStamina >= minStamina && currentStamina <= maxStamina && useStaminaManager)
		// 	currentStamina += amountToIncrease;
	}

	// Update is called once per frame
	void Update ()
	{
		if (prevPlayerType != afpcPlayer.playerType)
			ChangePlayerType ();

		prevPlayerType = afpcPlayer.playerType;

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) 
		{
			if (afpcPlayer.infiniteRunning)
				useStaminaManager = false;
			else
				useStaminaManager = true; // Only enable stamina system when infinite running is disabled 

			currentStamina = Mathf.Clamp (currentStamina, minStamina, maxStamina);

			if (!hideStaminaBar && useStaminaManager) {
				staminaMaster.rectTransform.anchoredPosition = startingPos;
			}

			if (staminaMaster != null) 
			{
				if (useStaminaManager && afpcPlayer.IsRunning && hideStaminaBar) {
					staminaMaster.rectTransform.anchoredPosition = Vector3.SmoothDamp (staminaMaster.rectTransform.anchoredPosition, startingPos, ref staminaMasterPositionV, timeToHideStaminaBar);	
				} else if (useStaminaManager && !afpcPlayer.IsRunning && hideStaminaBar) {
					staminaMaster.rectTransform.anchoredPosition = Vector3.SmoothDamp (staminaMaster.rectTransform.anchoredPosition, hidePosition, ref staminaMasterPositionV, timeToHideStaminaBar);
				}
			}

			if (staminaBar != null && useBarToShowStamina) {
				float targetFillAmount = staminaBar.GetComponent<AFPC_UIBar> ().ConvertValuesToFillAmountValue (currentStamina, minStamina, maxStamina, 0f, 1f);
				staminaBar.fillAmount = Mathf.SmoothDamp (staminaBar.fillAmount, targetFillAmount, ref fillAmountV, timeToSmoothFillAmount);
			}

			if (staminaText != null && useTextToShowStamina)
				staminaText.text = currentStamina.ToString ();

			if (!afpcPlayer.infiniteRunning && currentStamina <= minStamina && useGaspSound && gaspSound != null && useStaminaManager && !i) {
				_audioSrc.PlayOneShot (gaspSound, gaspVolume);
				i = true;
			}

		}else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if (staminaMaster != null && staminaMaster.IsActive())
				staminaMaster.gameObject.SetActive (false);
		}
			
	}
}