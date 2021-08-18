using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FXnRXn.PlaneHijack;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float fireDelay = 0.5f;
    public float continuousFireDelay = 0.2f;

    public GameObject firePosition;
    public float range = 50f;
    public float damage = 10f;

    private Rigidbody myRigidbody;

    private float lastFireTime = 0f;

    public bool autoGun = false;

    public float lookSensitivity;
    public float cameraRotationLimit;

    private float currentCameraRotationX = 0f;
    
    [Header("Camera :")]
    public Camera theCamera;
    public Transform _pivotTrans;

    [Header("Audio clips")]
    public AudioClip fireSound;
    public AudioClip switchGunSound;
    public AudioClip reloadSound1;
    public AudioClip reloadSound2;

    public GameObject damagePanel;

    public int maxHp = 100;
    private int hp;

    public Text hpText;
    public Image hpBar;

    public ParticleSystem particle;
    public GameObject gun;
    private StringBuilder sb = new StringBuilder(11);

    public int def = 0;
    private bool isCollision = false;

    private bool isGameOver = false;
    private bool isAttackable = true;

    public GameObject soundObject = null;

    public int currentBulletCount = 30;
    private int maxBulletCount = 30;

    public float reloadDelay = 3.0f;

    private bool isReload = false;

    private GunAnimation gunAnimation = null;
    private GameStateManager gameStateManager = null;

    public int Hp 
    {
        get { return hp; }
        set 
        {
            if (value > maxHp)
            {
                hp = maxHp;
            }
            else
            {
                hp = value;
            }

            if (hp < 0) hp = 0;

            sb.Remove(0, sb.Length);
            sb.Append("HP: ");
            sb.Append(hp);
            sb.Append('/');
            sb.Append(maxHp);
            hpText.text = sb.ToString();
            hpBar.fillAmount = (float)hp / maxHp;

            if (hp == 0)
            {
                GameOver();
            }
        }
    }

    private void Awake()
    {
        gameStateManager = GameStateManager.Instance;

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager");
        }

        gunAnimation = FindObjectOfType<GunAnimation>();

        if (gunAnimation == null)
        {
            Debug.LogError("gunAnimationÀÌ");
        }

        myRigidbody = GetComponent<Rigidbody>();

        damage = gameStateManager.playerDamage;
        Hp = gameStateManager.playerHP;
        currentBulletCount = gameStateManager.bulletCount;
        def = gameStateManager.playerDef;
        autoGun = gameStateManager.autoGun;
        //GameManager.Instance.GunModeUIChange(autoGun ? 1 : 0);

        GameManager.Instance.BulletCountUI(currentBulletCount);
        GameManager.Instance.AttackTextUI((int)damage);
        GameManager.Instance.DefTextUI(def);

        if (soundObject == null)
        {
            Debug.LogError("soundObject°");
        }
        else
        {
            PoolManager.CreatePool<Sound>(soundObject, transform, 10);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (!GameManager.Instance.isPlay) return;

        CameraRotation();
        CharacterRotation();

        if (transform.position.y <= -50f)
        {
            GameOver();
        }

        if (!isAttackable || isReload) return;

        // if (Input.GetButton("Fire1") && autoGun == true)
        // {
        //     Fire(damage, continuousFireDelay);
        // }
        // else if (Input.GetButtonDown("Fire1"))
        // {
        //     Fire(damage * 2f, fireDelay);
        // }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            autoGun = !autoGun;
            GameManager.Instance.GunModeUIChange(autoGun ? 1 : 0);

            isAttackable = false;
            PoolManager.GetItem<Sound>().soundPlay(switchGunSound, 1f, 2f);
            Invoke("switchGunModeChange", 0.5f);
        }

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Reload();
        // }
    }

    public void ReloadButtonPressed()
    {
        Reload();
    }


    public void FireButtonPressed()
    {
        if (autoGun == true)
        {
            Fire(damage, continuousFireDelay);
        }
        else
        {
            Fire(damage * 2f, fireDelay);
        }
    }

    private void switchGunModeChange()
    {
        isAttackable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WALL"))
        {
            isCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
    }

    private void Move()
    {
        float x = InputPlaneHijack.singleton.horizontal;//Input.GetAxisRaw("Horizontal")
        float z = InputPlaneHijack.singleton.vertical;//Input.GetAxisRaw("Vertical")

        Vector3 moveHorizontal = transform.right * x;
        Vector3 moveVertical = transform.forward * z;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        if (isCollision)
        {
            return;
        }

        if (velocity == Vector3.zero) gunAnimation.MoveEnd();
        else gunAnimation.MoveStart();

        myRigidbody.MovePosition(transform.position + velocity);
    }

    public void CameraRotation(float xRotation = 0)
    {
        // xRotation = xRotation == 0 ? InputPlaneHijack.singleton.mouseY: xRotation;//Input.GetAxis("Mouse Y") 
        // float cameraRotationX = xRotation * lookSensitivity;

        if (isCollision)
        {
            return;
        }
        
        //currentCameraRotationX -= cameraRotationX;
        //currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        //New
        InputPlaneHijack.singleton.lookAngle += InputPlaneHijack.singleton.mouseX * (Time.deltaTime * lookSensitivity);
        Vector3 camEulers = Vector3.zero;
        camEulers.y = InputPlaneHijack.singleton.lookAngle;
        transform.eulerAngles = camEulers;
            
            
        InputPlaneHijack.singleton.tiltAngle -= InputPlaneHijack.singleton.mouseY * (Time.deltaTime * lookSensitivity);
        InputPlaneHijack.singleton.tiltAngle = Mathf.Clamp(InputPlaneHijack.singleton.tiltAngle, -45f, 45f);
            
        Vector3 tiltEuler = Vector3.zero;
        tiltEuler.x = InputPlaneHijack.singleton.tiltAngle;
        _pivotTrans.localEulerAngles = tiltEuler;
    }

    private void CharacterRotation()
    {
        // float yRotation = InputPlaneHijack.singleton.mouseY;//Input.GetAxis("Mouse X")
        // Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        // transform.Rotate(transform.rotation.normalized * characterRotationY);
    }

    private void Fire(float damage, float delay)
    {
        if (currentBulletCount <= 0) return;

        if (Time.time - lastFireTime > delay)
        {
            PoolManager.GetItem<Sound>().soundPlay(fireSound, 0.5f, 2f);
            
            lastFireTime = Time.time;

            RaycastHit hit;

            int randomNumber = Random.Range(0, 10);
            damage = damage + randomNumber - 5;

            if (Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out hit, range))
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(damage);

                    Color color;

                    if (randomNumber <= 2)
                    {
                        color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        color = new Color(1f, 1f * ((float)(10 - randomNumber) / 10), 0f, 1f);
                    }
                    
                    float sizeValue = 0.5f + (0.05f * randomNumber);
                    Vector3 size = new Vector3(sizeValue, sizeValue, sizeValue);
                    PoolManager.GetItem<DamageText>().ShowText(damage.ToString(), hit.point, transform.position, size, color);
                }
            }

            particle.Play();
            currentBulletCount--;
            GameManager.Instance.BulletCountUI(currentBulletCount);

            if (autoGun)
            {
                gunAnimation.ContinuousFireStart();
            }
            else
            {
                gunAnimation.FireStart();
            }
        }
    }

    private void Reload()
    {
        if (isReload) return;

        currentBulletCount = maxBulletCount;
        isReload = true;

        PoolManager.GetItem<Sound>().soundPlay(reloadSound1, 1f, 1f);

        Invoke("ReloadSound2Play", 1.3f);
        Invoke("EndReload", reloadDelay);
        gunAnimation.ReloadStart();
    }

    private void ReloadSound2Play()
    {
        PoolManager.GetItem<Sound>().soundPlay(reloadSound2, 1f, 1f);
    }

    private void EndReload()
    {
        isReload = false;

        GameManager.Instance.BulletCountUI(currentBulletCount);
    }

    public void Damage(int damage)
    {
        damagePanel.SetActive(true);
        Invoke("EndDamage", 0.2f);

        if (damage - def < 1)
        {
            Hp -= 1;
        }
        else
        {
            Hp -= (damage - def);
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        gunAnimation.AnimationEnd();
        GameManager.Instance.GameOver();
        gun.SetActive(false);
    }

    private void EndDamage()
    {
        damagePanel.SetActive(false);
    }
}
