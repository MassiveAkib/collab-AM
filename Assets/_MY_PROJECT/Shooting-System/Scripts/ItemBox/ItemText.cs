using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemText : MonoBehaviour
{
    private Text text = null;

    private void Awake()
    {
        text = GetComponent<Text>();

        if (text == null)
        {
            Debug.LogError("text가 없습니다.");
        }
    }

    public void ShowText(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;

        transform.DOLocalMoveY(50f, 0.5f).SetRelative().OnComplete(() =>
        {
            this.text.DOColor(new Color(color.r, color.g, color.b, 0f), 0.5f).OnComplete(() => {
                gameObject.SetActive(false);
                transform.DOLocalMoveY(-50f, 0f).SetRelative();
            });
        });
    }
}
