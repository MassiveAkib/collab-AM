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
        if (playerMove == null) Debug.LogError("PlayerMove°¡ ¾ø½À´Ï´Ù.");

        if (audioSource == null) Debug.LogError("audioSource°¡ ¾ø½À´Ï´Ù.");

        if (sound[0] == null) Debug.LogError("sound´Â 1°³ ÀÌ»ó ÀÖ¾î¾ß ÇÕ´Ï´Ù.");
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
                    PoolManager.GetItem<ItemText>().ShowText("°ø°Ý·Â ¾÷", Color.green);
                    break;
                case ItemEnum.DEFUP:
                    playerMove.def += defUpValue;
                    GameManager.Instance.DefTextUI(playerMove.def);
                    PoolManager.GetItem<ItemText>().ShowText("¹æ¾î·Â ¾÷", Color.cyan);
                    break;
                case ItemEnum.HPUP:
                    playerMove.Hp += hpUpValue;
                    PoolManager.GetItem<ItemText>().ShowText("Ã¼·Â È¸º¹", Color.red);
                    break;
            }

            audioSource.clip = sound[Random.Range(0, sound.Length)];
            audioSource.Play();

            gameObject.SetActive(false);
        }
    }
}
