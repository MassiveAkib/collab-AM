using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AFPC_UIBar : MonoBehaviour {

	public bool changeColorOnChangingFillAmountValues = true;
	public Color minValueColor = Color.red;
	public Color maxValueColor = new Color (85f, 171f, 85f, 255f);

	private Color currentColor;
	private Image target;

	// Use this for initialization
	void Start () 
	{
		target = GetComponent<Image> ();	
	}

	void Update()
	{
		if (changeColorOnChangingFillAmountValues)
		{
			currentColor = Color.Lerp (minValueColor, maxValueColor, target.fillAmount);
			target.color = currentColor;
		}
	}

	public void DecreaseBarAmount(float amountToDecrease, float inMin, float inMax, float outMin, float outMax)
	{
		if (target.type == Image.Type.Filled) 
		{
			amountToDecrease = ConvertValuesToFillAmountValue (amountToDecrease, inMin, inMax, outMin, outMax);
			target.fillAmount -= amountToDecrease;
		} else 
		{
			Debug.LogError ("Fill Amount of " + target.name + " can't be changed because the Image Type is not Filled!");
		}
	}

	public void IncreaseBarAmount(float amountToIncrease, float inMin, float inMax, float outMin, float outMax)
	{
		if (target.type == Image.Type.Filled)
		{
			amountToIncrease = ConvertValuesToFillAmountValue (amountToIncrease, inMin, inMax, outMin, outMax);
			target.fillAmount += amountToIncrease;
		} else 
		{
			Debug.LogError ("Fill Amount of " + target.name + " can't be changed because the Image Type is not Filled!");
		}
	}

	public float ConvertValuesToFillAmountValue(float value, float inMin, float inMax, float outMin, float outMax)
	{
		if (target.type == Image.Type.Filled) 
		{
			/*This Method converts the health value or any other value in the range of 0 to 1 i.e. the range of image fill amount value
		 * value = the current value, inMin - the minimum input value i.e. the minimum health or something for e.g. minimum health is 0 
		 * and maximum health is 100 and minimum fillAmount value is 0 and maximum fillamount value is 1, and the current health be 75,
		 * then, value - 75f, inMin - 0f, inMax - 100f, outMin - 0f, outMax - 1f, the resulting value of fill Amount will be 0.75f. */
			return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;	
		} else
		{
			Debug.LogError ("Fill Amount of " + target.name + " can't be changed because the Image Type is not Filled!");
			return 0f;
		}
	}

}
