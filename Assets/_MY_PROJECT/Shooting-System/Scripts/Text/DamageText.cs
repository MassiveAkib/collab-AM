using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public float moveY;
    public float moveDuration;
    public float alphaDuration;

    private TextMesh text = null;

    private void Awake()
    {
        text = FindObjectOfType<TextMesh>();

        if (text == null)
        {
            Debug.LogError("text가 없습니다.");
        }
    }

    public void ShowText(string textValue, Vector3 startPosition, Vector3 lookPosition, Vector3 size, Color textColor)
    {
        transform.position = startPosition;
        transform.localScale = size;
        transform.LookAt(lookPosition);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180f, transform.rotation.eulerAngles.z);
        text.text = textValue;
        text.color = textColor;

        transform.DOMoveY(transform.position.y + moveY, moveDuration).OnComplete(() =>
        {
            DOTween.To(() => text.color, x => text.color = x, new Color(text.color.r, text.color.g, text.color.b, 0f), alphaDuration).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
