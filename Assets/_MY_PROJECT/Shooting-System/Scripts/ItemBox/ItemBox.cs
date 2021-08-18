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
        if (boxAnimator == null) Debug.LogError("Animator�� �����ϴ�.");

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null) Debug.LogError("BoxCollider�� �����ϴ�.");

        if (box == null) Debug.LogError("box�� �����ϴ�.");
        else if (!box.activeSelf) Debug.LogError("box�� active�� �����־�� �մϴ�.");

        if (destroyBox == null) Debug.LogError("destroyBox�� �����ϴ�.");
        else if (destroyBox.activeSelf) Debug.LogError("destroyBox�� active�� �����־�� �մϴ�.");

        if (portion == null) Debug.LogError("portion�� �����ϴ�.");
        else if (portion.activeSelf) Debug.LogError("portion�� active�� �����־�� �մϴ�.");

        if (effect == null) Debug.LogError("effect�� �����ϴ�.");
        else if (!effect.activeSelf) Debug.LogError("effect�� active�� �����־�� �մϴ�.");

        boxRenderer = box.GetComponent<Renderer>();
        if (boxRenderer == null) Debug.LogError("box�ȿ� Renderer�� �����ϴ�.");

        makeMaze = FindObjectOfType<MakeMaze>();
        if (makeMaze == null) Debug.LogError("MakeMaze�� �����ϴ�.");

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
