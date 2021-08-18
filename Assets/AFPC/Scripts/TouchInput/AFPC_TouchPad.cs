using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AFPC_TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private Vector2 inputVector = Vector2.zero;
	public Vector2 InputVector{get{ return inputVector;}}

	private bool _isDragging = false;
	private RectTransform _rectTrans;

	private void Start()
	{
		if (GetComponent<RectTransform> ()) 
			_rectTrans = GetComponent<RectTransform> ();
	}

	private void Update()
	{
		if (!_isDragging)
			return;
	
		foreach (Touch touch in Input.touches) {
			if (RectTransformUtility.RectangleContainsScreenPoint (_rectTrans, touch.position)) {
				if (touch.phase == TouchPhase.Moved) {
					inputVector.x = touch.deltaPosition.x;
					inputVector.y = touch.deltaPosition.y;
					inputVector.Normalize ();
				} else if (touch.phase == TouchPhase.Stationary) {
					inputVector = Vector2.zero;
				} else if (touch.phase == TouchPhase.Ended) {
					inputVector = Vector2.zero;
				}
			} 
		}
			
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_isDragging = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		inputVector = Vector2.zero;
		_isDragging = false;
	}

}