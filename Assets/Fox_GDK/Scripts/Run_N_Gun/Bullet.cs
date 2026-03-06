using System;
using UnityEngine;

public class Bullet : FoxObject
{
    public BulletData bulletData;
    public GameObject vfx;
    public GameObject bulletTrail;
    public new Collider collider;

    TrailRenderer trail;
    BulletData preservedData;
    Fox_WeaponType weaponType;
    LayerMask collisionMask;
    Action OnBulletHitAction;
    AudioSource audioSource;
    Vector3 startingPosition;

    float remainingTravelLength;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        preservedData = bulletData.ExtractData();
        trail = GetComponent<TrailRenderer>();
        collider = GetComponent<Collider>();
        if (preservedData.onHitSoundClip)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.clip = preservedData.onHitSoundClip;
        }

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        bulletData = preservedData.ExtractData();
        vfx.SetActive(true);
        bulletTrail.SetActive(true);
        if (collider)
            collider.enabled = true;
        startingPosition = transform.position;

        if (trail != null)
        {
            trail.Clear();
            trail.enabled = true;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (trail != null)
        {
            trail.Clear();
            trail.enabled = false;
        }
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    void FixedUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (weaponType.Equals(Fox_WeaponType.GUN))
        {
            transform.position += bulletData.bulletSpeed * Time.fixedDeltaTime * transform.forward;
            if ((startingPosition - transform.position).magnitude >= bulletData.travelLength)
            {
                foxTools.poolManager.Destroy(gameObject);
            }
        }
        else if (weaponType.Equals(Fox_WeaponType.MELEE))
        {
            bulletData.travelLength -= bulletData.bulletSpeed;
            if (bulletData.travelLength <= 0)
            {
                foxTools.poolManager.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (!collisionMask.FOXE_IsInLayerMask(other.gameObject))
            return;

        collider.enabled = false;
        //EnemyController enemyController = other.GetComponent<EnemyController>();
        //if (enemyController != null)
        //{
        //    enemyController.health.OnLooseHealth?.Invoke(bulletData.bulletDefaultDamage + bulletData.bulletAdditionalDamage, this);

        //    if (bulletData.onHitParticle)
        //        foxTools.poolManager.Instantiate(bulletData.onHitParticle.gameObject,
        //            transform.position, Quaternion.identity,
        //            null,
        //            ConstantManager.ONE_HALF_TIME);
        //    if (bulletData.onHitSoundClip)
        //    {
        //        audioSource.Play();
        //    }

        //}
        vfx.SetActive(false);
        bulletTrail.SetActive(false);
        foxTools.poolManager.Destroy(gameObject);
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameOver()
    {
        base.OnGameOver();

        foxTools.poolManager.Destroy(gameObject);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Initialize(BulletData bulletData, Fox_WeaponType weaponType, LayerMask collisionMask, Action OnBulletHitAction = null)
    {
        this.bulletData = bulletData;
        this.weaponType = weaponType;
        this.collisionMask = collisionMask;
        this.OnBulletHitAction = OnBulletHitAction;
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
