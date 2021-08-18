using UnityEngine;
using UnityEngine.UI;

public class AFPC_OxygenManager : MonoBehaviour {
	public int maxOxygen = 100;
	public int minOxygen = 0;
	public bool decreaseHealthOnLowOxygen = true;
	public int amountToDecreaseHealthOnminOxygen = 5;
	public int currentOxygen = 100;
	public bool hideOxygenBar = true; // if enabled then hide oxygen bar when not in use means when player is not running, hide the bar
	public Vector3 hidePosition = new Vector3(-350f, 108f, 0f);
	public Image oxygenMaster;
	public bool useoxygenRestoreSound = true;
	public bool useTextToShowOxygen = true;
	public bool useBarToShowOxygen = true;
	public float oxygenRestoreVolume = 0.9f; // the volume of oxygenRestoreSound
	public AudioClip oxygenRestoreSound; // sound to play when player reaches 0 oxygen and the player is on the water surface
	public Image oxygenBar;
	public Text oxygenText;
	public float timeToSmoothFillAmount = 1f;
	public float timeToHideOxygenBar = 0.5f;
	[Tooltip("In Seconds")]
	public float oxygenDecreaseTime = 1f;	// Time taken to decrease Oxygen by oxygenToDecrease in seconds
	public int oxygenToDecrease = 1;
	public int oxygenToIncrease = 4;
	public float oxygenIncreaseTime = 2f;


	[HideInInspector]
	public bool useOxygenManager = true;
	private AFPC_PlayerMovement afpcPlayer; // The Player
	private float fillAmountV;	// used in smoothdamp
	private Vector3 startingPos; // The starting position of oxygen master
	private Vector3 oxygenMasterPositionV;
	private AudioSource _audioSrc; // the audioSource attached to this gameobject
	private AFPC_HealthManager healthManager;

	private AFPC_PlayerMovement.PlayerType prevPlayerType;
	private bool i = false;
	// Use this for initialization
	void Start () 
	{
		afpcPlayer = GetComponent<AFPC_PlayerMovement> ();

		if (oxygenMaster != null) {
			oxygenMaster.gameObject.SetActive (true);
			startingPos = oxygenMaster.rectTransform.anchoredPosition;
			if (hideOxygenBar)
				oxygenMaster.rectTransform.anchoredPosition = hidePosition;
		}

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			_audioSrc = GetComponent<AudioSource> ();
			healthManager = GetComponent<AFPC_HealthManager> ();
			if (_audioSrc == null) {	
				gameObject.AddComponent<AudioSource> ();
				_audioSrc = GetComponent<AudioSource> ();
			}
				
			if (afpcPlayer.IsSwimming)
				useOxygenManager = true;
			else
				useOxygenManager = false; // Only enable oxygen system when player is swimming 
			
			currentOxygen = Mathf.Clamp (currentOxygen, minOxygen, maxOxygen); //To Make sure that the user can't enter a non ranging value of oxygen
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if(oxygenMaster != null)
				oxygenMaster.gameObject.SetActive (false);
		}	
	}

	public void DecreaseOxygen()
	{
		// if (afpcPlayer.IsSwimming && afpcPlayer.DecreaseOxygen && currentOxygen > minOxygen && useOxygenManager) 
		// {
		// 	currentOxygen -= oxygenToDecrease;
		// }
		// if (afpcPlayer.IsSwimming && afpcPlayer.DecreaseOxygen && currentOxygen == minOxygen && useOxygenManager && decreaseHealthOnLowOxygen) 
		// {
		// 	healthManager.DecreaseHealth (amountToDecreaseHealthOnminOxygen);
		// 	i = false;
		// }
	}

	public void IncreaseOxygen()
	{
		// if (afpcPlayer.IsSwimming && !afpcPlayer.DecreaseOxygen && currentOxygen >= minOxygen && currentOxygen <= maxOxygen && useOxygenManager) 
		// {
		// 	currentOxygen += oxygenToIncrease;
		// }
	}

	public void IncreaseOxygenByAmount(int amountToIncrease)
	{
		// if (afpcPlayer.IsSwimming && currentOxygen >= minOxygen && currentOxygen <= maxOxygen && useOxygenManager) 
		// {
		// 	currentOxygen += amountToIncrease;
		// }
	}

	void ChangePlayerType()
	{
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			_audioSrc = GetComponent<AudioSource> ();
			healthManager = GetComponent<AFPC_HealthManager> ();
			if (_audioSrc == null) {	
				gameObject.AddComponent<AudioSource> ();
				_audioSrc = GetComponent<AudioSource> ();
			}
			if (oxygenMaster != null)
				oxygenMaster.gameObject.SetActive (true);	
			if (afpcPlayer.IsSwimming)
				useOxygenManager = true;
			else
				useOxygenManager = false; // Only enable oxygen system when player is swimming 

			currentOxygen = Mathf.Clamp (currentOxygen, minOxygen, maxOxygen); //To Make sure that the user can't enter a non ranging value of oxygen
		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if(oxygenMaster != null)
				oxygenMaster.gameObject.SetActive (false);
		}	
	}
	// Update is called once per frame
	void Update ()
	{
		
		if (prevPlayerType != afpcPlayer.playerType)
			ChangePlayerType ();

		prevPlayerType = afpcPlayer.playerType;

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
		{
			if (afpcPlayer.IsSwimming)
				useOxygenManager = true;
			else
				useOxygenManager = false; // Only enable oxygen system when player is swimming 
		
			currentOxygen = Mathf.Clamp (currentOxygen, minOxygen, maxOxygen);

			if (!hideOxygenBar && useOxygenManager) {
				oxygenMaster.rectTransform.anchoredPosition = startingPos;
			}

			if(oxygenMaster != null)
			{
				if (useOxygenManager && afpcPlayer.IsSwimming && hideOxygenBar) {
					oxygenMaster.rectTransform.anchoredPosition = Vector3.SmoothDamp (oxygenMaster.rectTransform.anchoredPosition, startingPos, ref oxygenMasterPositionV, timeToHideOxygenBar);	
				} else if (!useOxygenManager && !afpcPlayer.IsSwimming && hideOxygenBar) {
					oxygenMaster.rectTransform.anchoredPosition = Vector3.SmoothDamp (oxygenMaster.rectTransform.anchoredPosition, hidePosition, ref oxygenMasterPositionV, timeToHideOxygenBar);
				}
			}

			if (oxygenBar != null && useBarToShowOxygen) {
				float targetFillAmount = oxygenBar.GetComponent<AFPC_UIBar> ().ConvertValuesToFillAmountValue (currentOxygen, minOxygen, maxOxygen, 0f, 1f);
				oxygenBar.fillAmount = Mathf.SmoothDamp (oxygenBar.fillAmount, targetFillAmount, ref fillAmountV, timeToSmoothFillAmount);
			}

			if (oxygenText != null && useTextToShowOxygen)
				oxygenText.text = currentOxygen.ToString ();

			if (!afpcPlayer.infiniteRunning && currentOxygen == minOxygen && useoxygenRestoreSound && oxygenRestoreSound != null && useOxygenManager && !i) {
				_audioSrc.PlayOneShot (oxygenRestoreSound, oxygenRestoreVolume);
				i = true;
			}

		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator) {
			if(oxygenMaster != null && oxygenMaster.IsActive())
				oxygenMaster.gameObject.SetActive (false);
		}
			
	}
}
