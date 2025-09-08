using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "FoxTools/Inventory/BulletData")]
public class BulletData : ScriptableObject
{
    public float bulletSpeed;
    public float travelLength;
    public float bulletDefaultDamage;
    public float bulletAdditionalDamage;
    public ParticleSystem onHitParticle;

    public float BULLET_CURRENT_DAMAGE
    {
        get { return bulletDefaultDamage + bulletAdditionalDamage; }
    }

    public BulletData ExtractData()
    {
        BulletData bulletData = new BulletData();
        bulletData.bulletSpeed = this.bulletSpeed;
        bulletData.travelLength = this.travelLength;
        bulletData.bulletDefaultDamage = this.bulletDefaultDamage;
        bulletData.bulletAdditionalDamage = this.bulletAdditionalDamage;
        bulletData.onHitParticle = this.onHitParticle;
        return bulletData;
    }
}
