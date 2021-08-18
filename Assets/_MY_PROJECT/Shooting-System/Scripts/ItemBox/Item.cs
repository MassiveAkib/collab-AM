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
        if (playerMove == null) Debug.LogError("PlayerMove�� �����ϴ�.");

        if (audioSource == null) Debug.LogError("audioSource�� �����ϴ�.");

        if (sound[0] == null) Debug.LogError("sound�� 1�� �̻� �־�� �մϴ�.");
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
                    PoolManager.GetItem<ItemText>().ShowText("���ݷ� ��", Color.green);
                    break;
                case ItemEnum.DEFUP:
                    playerMove.def += defUpValue;
                    GameManager.Instance.DefTextUI(playerMove.def);
                    PoolManager.GetItem<ItemText>().ShowText("���� ��", Color.cyan);
                    break;
                case ItemEnum.HPUP:
                    playerMove.Hp += hpUpValue;
                    PoolManager.GetItem<ItemText>().ShowText("ü�� ȸ��", Color.red);
                    break;
            }

            audioSource.clip = sound[Random.Range(0, sound.Length)];
            audioSource.Play();

            gameObject.SetActive(false);
        }
    }
}
