using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCheck : MonoBehaviour
{
    private PlayerMove playerMove = null;

    [SerializeField] private int attackDamage;

    private float enterTime = 0f;
    [SerializeField] private float attackTime = 0.5f;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove를 찾을 수 없습니다.");
        }

        attackDamage = attackDamage * GameStateManager.Instance.Stage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            playerMove.Damage(attackDamage);
            enterTime = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            if (enterTime + attackTime < Time.time)
            {
                playerMove.Damage(attackDamage);
                enterTime = Time.time;
            }
        }
    }
}
