using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : FoxObject
{
    public WeaponType weaponType;
    public GameObject vfx;
    public WeaponData weaponData;
    public BulletData bulletData;
    public ParticleSystem muzzle;
    public Transform[] bulletSpawnPoints;

    public bool fireIsOn;
    float fireCoolDown;
    Transform directionPoint;
    public LayerMask layerMask;

    Animator anim;
    [HideInInspector]
    public BulletData preservedBulletData;
    [HideInInspector]
    public WeaponData preservedWeaponData;

    Action OnBulletHitAction;
    bool bossFightStarted;

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

        bulletData = preservedBulletData.ExtractData();
        weaponData = preservedWeaponData.ExtractData();
    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        preservedBulletData = bulletData.ExtractData();
        preservedWeaponData = weaponData.ExtractData();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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

        fireCoolDown -= Time.deltaTime;
        if(fireCoolDown <= 0 && fireIsOn)
        {
            fireCoolDown = 1f / (weaponData.fireRate * weaponData.fireRateMinimizer);

            if (weaponType.Equals(WeaponType.GUN))
            {
                Fire();
            }
            else
                MeleeAttack();
        }
        else if(!fireIsOn)
        {
            fireCoolDown = 0;
        }

    }

    void FixedUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    void LateUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameOver()
    {
        base.OnGameOver();

        fireIsOn = false;
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void FireOn(LayerMask layerMask, Animator anim = null, Transform directionPoint = null, Action OnBulletHitAction = null)
    {
        this.anim = anim;
        this.layerMask = layerMask;
        this.directionPoint = directionPoint;
        this.OnBulletHitAction = OnBulletHitAction;
        fireIsOn = true;
    }

    public void FireOFF()
    {
        fireIsOn = false;
    }

    public void Fire()
    {
        if (muzzle)
        {
            if (bossFightStarted)
                muzzle.transform.localScale = Vector3.one * 2;
            muzzle.Play();
        }

        GameObject bulletObj;
        for (int i = 0; i < bulletSpawnPoints.Length; i++)
        {
            bulletObj = FoxTools.poolManager.Instantiate(
                weaponData.bulletPrefab,
                bulletSpawnPoints[i].position,
                directionPoint == null? bulletSpawnPoints[i].rotation :
                                        Quaternion.LookRotation(directionPoint.position - bulletSpawnPoints[i].position, Vector3.up)
                );
            bulletObj.layer = layerMask;
            bulletObj.GetComponent<Bullet>().Initialize(bulletData, weaponType, OnBulletHitAction);
        }
    }

    void MeleeAttack() {

        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }


    #endregion ALL SELF DECLARE FUNCTIONS

}
