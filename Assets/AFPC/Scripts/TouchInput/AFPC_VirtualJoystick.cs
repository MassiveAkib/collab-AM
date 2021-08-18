using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class AFPC_VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	[Range(0f, 2f)]
	public float handleMaxDistance = 1f;
	public float timeToFadeIn = 0.5f, timeToResetHandlePos = 0.1f;

	public RectTransform background, handle;

	protected Vector2 inputVector = new Vector2(0f, 0f); 
	public Vector2 InputVector{get{ return inputVector;}}

	private bool resetHandlePos = false, doFadeIn = false;

	private Image backgroundImage, handleImage;
	private Color initBackgroundColor, initHandleColor;

	private Vector3 handleV; // Used in SmoothDamp
	private float colorV1;

	protected void Start()
	{
		if (background != null && background.GetComponent<Image> ()) 
		{
			backgroundImage = background.GetComponent<Image> ();
			initBackgroundColor = backgroundImage.color;
		}
		if (handle != null && handle.GetComponent<Image> ()) 
		{
			handleImage = handle.GetComponent<Image> ();
			initHandleColor = handleImage.color;
		}
	}

	public virtual void OnDrag(PointerEventData pointerEventData)
	{
		
	}

	public virtual void OnPointerUp(PointerEventData pointerEventData)
	{
		resetHandlePos = true;
		doFadeIn = true;
	}

	public virtual void OnPointerDown(PointerEventData pointerEventData)
	{
		resetHandlePos = false;
		MakeVisible ();
		doFadeIn = false;
	}

	private void Update()
	{
		if (handle != null && resetHandlePos)
			handle.anchoredPosition = Vector3.SmoothDamp (handle.anchoredPosition, Vector3.zero, ref handleV, timeToResetHandlePos);
		if (backgroundImage != null && handleImage != null && doFadeIn)
			FadingIn ();
	}

	private void MakeVisible()
	{
		if (background != null && handle != null) 
		{
			background.gameObject.SetActive (true);
			initBackgroundColor.a = 1f;
			initHandleColor.a = 1f;
			backgroundImage.color = initBackgroundColor;
			handleImage.color = initHandleColor;
		} else {
			Debug.LogError ("No Reference set to background or handle in " + name + "joystick");
		}
	}

	private void FadingIn()
	{
		backgroundImage.color = initBackgroundColor;
		handleImage.color = initHandleColor;

		//Lerping Background Color
		Color targetColor = new Color (initBackgroundColor.r, initBackgroundColor.g, initBackgroundColor.b, 0f);
		initBackgroundColor.a = Mathf.SmoothDamp (initBackgroundColor.a, targetColor.a, ref colorV1, timeToFadeIn);
		backgroundImage.color = initBackgroundColor;

		//Lerping Handle Color
		Color targetColor1 = new Color (initHandleColor.r, initHandleColor.g, initHandleColor.b, 0f);
		initHandleColor.a = Mathf.SmoothDamp (initHandleColor.a, targetColor1.a, ref colorV1, timeToFadeIn);
		handleImage.color = initHandleColor;

		if(backgroundImage.color.a <= 0.05f && handleImage.color.a <= 0.05f)
			backgroundImage.gameObject.SetActive (false);
	}
}