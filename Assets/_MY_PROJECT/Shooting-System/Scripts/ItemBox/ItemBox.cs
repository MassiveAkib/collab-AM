using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, IDamageable
{
    private float hp = 100f;
    private Color color;

    private Animator boxAnimator = null;
    private BoxCollider boxCollider = null;
    private Renderer boxRenderer = null;

    public GameObject box = null;
    public GameObject destroyBox = null;
    public GameObject portion = null;
    public GameObject effect = null;

    private MakeMaze makeMaze = null;

    private void Awake()
    {
        boxAnimator = GetComponent<Animator>();
        if (boxAnimator == null) Debug.LogError("Animator가 없습니다.");

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null) Debug.LogError("BoxCollider가 없습니다.");

        if (box == null) Debug.LogError("box가 없습니다.");
        else if (!box.activeSelf) Debug.LogError("box의 active는 켜져있어야 합니다.");

        if (destroyBox == null) Debug.LogError("destroyBox가 없습니다.");
        else if (destroyBox.activeSelf) Debug.LogError("destroyBox의 active는 꺼져있어야 합니다.");

        if (portion == null) Debug.LogError("portion이 없습니다.");
        else if (portion.activeSelf) Debug.LogError("portion의 active는 꺼져있어야 합니다.");

        if (effect == null) Debug.LogError("effect가 없습니다.");
        else if (!effect.activeSelf) Debug.LogError("effect의 active는 켜져있어야 합니다.");

        boxRenderer = box.GetComponent<Renderer>();
        if (boxRenderer == null) Debug.LogError("box안에 Renderer가 없습니다.");

        makeMaze = FindObjectOfType<MakeMaze>();
        if (makeMaze == null) Debug.LogError("MakeMaze가 없습니다.");

        int randomNum = Random.Range(0, makeMaze.enablePosition.Count);
        transform.position = makeMaze.enablePosition[randomNum];
        makeMaze.enablePosition.RemoveAt(randomNum);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        hp = GameStateManager.Instance.Stage * 120;
    }

    public void OnDamage(float damage)
    {
        hp -= damage;

        color = Color.red;
        ChangeColor();
        color = Color.white;
        Invoke("ChangeColor", 0.1f);

        if (hp < 0)
        {
            box.SetActive(false);
            destroyBox.SetActive(true);
            portion.SetActive(true);
            effect.SetActive(false);
            boxCollider.enabled = false;
            boxAnimator.Play("Bang");
        }
    }

    private void ChangeColor()
    {
        boxRenderer.material.SetColor("_Color", color);
    }
}
