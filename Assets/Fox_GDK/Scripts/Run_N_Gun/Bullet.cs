using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : FoxObject
{
    public BulletData bulletData;

    float remainingTravelLength;
    TrailRenderer trail;
    Vector3 startingPosition;
    public new Collider collider;
    BulletData preservedData;
    WeaponType weaponType;

    Action OnBulletHitAction;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        preservedData = bulletData.ExtractData();
        trail = GetComponent<TrailRenderer>();
        collider = GetComponent<Collider>();
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
        if(collider)
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
        //if (gameState.Equals(GameState.GAME_INITIALIZED) && Input.GetMouseButtonDown(0))
        //{
        //    gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
        //    gameState = GameState.GAME_PLAY_STARTED;
        //}

        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    void FixedUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (weaponType.Equals(WeaponType.GUN))
        {
            transform.position += bulletData.bulletSpeed * Time.fixedDeltaTime * transform.forward;
            if ((startingPosition - transform.position).magnitude >= bulletData.travelLength)
            {
                OnBulletHitAction?.Invoke();
                FoxTools.poolManager.Destroy(gameObject);
            }
        }
        else if(weaponType.Equals(WeaponType.MELEE))
        {
            bulletData.travelLength -= bulletData.bulletSpeed;
            if (bulletData.travelLength <= 0)
            {
                OnBulletHitAction?.Invoke();
                FoxTools.poolManager.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {            
            health.Damage(bulletData.bulletDefaultDamage + bulletData.bulletAdditionalDamage);
        }
        FoxTools.poolManager.Destroy(gameObject);
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameOver()
    {
        base.OnGameOver();

        FoxTools.poolManager.Destroy(gameObject);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Initialize(BulletData bulletData, WeaponType weaponType, Action OnBulletHitAction = null)
    {
        this.bulletData = bulletData;
        this.weaponType = weaponType;
        this.OnBulletHitAction = OnBulletHitAction;
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
