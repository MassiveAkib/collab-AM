using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private PlayerMove playerMove = null;
    public AudioSource audioSource = null;

    [SerializeField] private AudioClip[] sound;

    enum ItemEnum
    {
        ATKUP, DEFUP, HPUP
    }

    [SerializeField] private ItemEnum itemCategory;

    public int atkUpValue = 10;
    public int defUpValue = 3;
    public int hpUpValue = 20;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        if (playerMove == null) Debug.LogError("PlayerMove가 없습니다.");

        if (audioSource == null) Debug.LogError("audioSource가 없습니다.");

        if (sound[0] == null) Debug.LogError("sound는 1개 이상 있어야 합니다.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAYER")
        {
            switch (itemCategory)
            {
                case ItemEnum.ATKUP:
                    playerMove.damage += atkUpValue;
                    GameManager.Instance.AttackTextUI((int)playerMove.damage);
                    PoolManager.GetItem<ItemText>().ShowText("공격력 업", Color.green);
                    break;
                case ItemEnum.DEFUP:
                    playerMove.def += defUpValue;
                    GameManager.Instance.DefTextUI(playerMove.def);
                    PoolManager.GetItem<ItemText>().ShowText("방어력 업", Color.cyan);
                    break;
                case ItemEnum.HPUP:
                    playerMove.Hp += hpUpValue;
                    PoolManager.GetItem<ItemText>().ShowText("체력 회복", Color.red);
                    break;
            }

            audioSource.clip = sound[Random.Range(0, sound.Length)];
            audioSource.Play();

            gameObject.SetActive(false);
        }
    }
}
