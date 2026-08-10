using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Sword", order = 1)]
public class SwordData : WeaponData
{

    [Header("Hitbox")]
    [SerializeField] private float range = 2f;
    public float Range => range;
    [SerializeField] private float attackWidth = 2f;
    public float AttackWidth => attackWidth;

    private Dictionary<Vector3, bool> hitPositions = new();
    private bool hasHit;

    private float lastChargeAmount;
    [Header("HitFeedback")]
    [SerializeField] private float minStopTime = 0.05f;
    [SerializeField] private float maxStopTime = 0.15f;

    [SerializeField] private float minTimeScale = 0.2f;
    [SerializeField] private float maxTimeScale = 0f;

    [Header("AttackTrail")]
    [SerializeField] private Color trailColor = Color.white;
    public Color TrailColor => trailColor;

    [Header("Debug")]
    [SerializeField] private bool showAttackHitbox;
    public bool ShowAttackHitbox => showAttackHitbox;



    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack(camera, chargeAmount);
        lastChargeAmount = chargeAmount;
        hitPositions.Clear();
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, range, attackLayer)) {
            hasHit = true;
            bool isEnemyHit = false;
            Vector3 hitPosition = hit.point - camera.forward * 0.2f;
            if (hit.transform.TryGetComponent<HealthManager>(out HealthManager healthManager))
            {
                if (playerWeaponController.PlayerCombatController.IsBlocking)
                {
                    healthManager.InflictDamage(0, knockBackStregth * 2, TeamFlag.Player, camera);
                }
                else
                {
                    healthManager.InflictDamage(CalculateDamage(chargeAmount,playerData.Strength), Mathf.Lerp(minKnockBackStregth,knockBackStregth,chargeAmount), TeamFlag.Player, camera);
                }
                isEnemyHit = true;
            }
            hitPositions.Add(hitPosition, isEnemyHit);
        }


        RaycastHit[] hits = Physics.BoxCastAll(
            camera.position,
             GetHalfBoxExtend(camera),
            camera.forward/2,
            camera.rotation,
            range,
            attackLayer
        );


        foreach (RaycastHit otherhit in hits)
        {
            if(otherhit.transform == null)
            {
                continue;
            }
            if(hasHit)
            {
                if (otherhit.transform.name == hit.transform.name)
                {
                    return;
                }
            }
            bool isEnemyHit = false;
            Vector3 hitPosition = otherhit.point - camera.forward * 0.2f;
            if (otherhit.transform.TryGetComponent<HealthManager>(out HealthManager healthManager))
            {
                if (playerWeaponController.PlayerCombatController.IsBlocking)
                {
                    healthManager.InflictDamage(0, knockBackStregth * 10, TeamFlag.Player, camera);
                }
                else
                {
                    healthManager.InflictDamage(CalculateDamage(chargeAmount, playerData.Strength), chargeAmount * knockBackStregth, TeamFlag.Player, camera);
                }
                isEnemyHit = true;
            }
            if (!hitPositions.ContainsKey(hitPosition))
            {
                hitPositions.Add(hitPosition, isEnemyHit);
            }
        }

        if (hits.Length > 0) {
            hasHit = true;
        }

    }


    public void PlayHitVFX()
    {
        if (!hasHit) {
            return;
        }

        bool hasPlayedSound = false;


        foreach (KeyValuePair<Vector3,bool> position in hitPositions) {
            GameObject hitVFX;
            if (position.Value){
                hitVFX = SVFXLibary.Instance.GetVFX(SVFXLibary.VFXNames.StandartHit);
            }
            else{
                hitVFX = SVFXLibary.Instance.GetVFX(SVFXLibary.VFXNames.GroundHit);
            }
            if (!hasPlayedSound)
            {
                if (position.Value) {
                    SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Sword_Hit_Enemy);
                }
                else{
                    SoundManager.instance.PlayLibarySound(SoundLibary.SFX.Sword_Hit_Ground);
                }
                hasPlayedSound = true;
            }
            GameObject hit = Instantiate(hitVFX, position.Key, Quaternion.identity);
            Destroy(hit,1f);
        }
        playerWeaponController.HitStop(Mathf.Lerp(minTimeScale, maxTimeScale, lastChargeAmount), Mathf.Lerp(minStopTime, maxStopTime, lastChargeAmount));

        hasHit = false;
    }

    public Vector3 GetHalfBoxExtend(Transform camera)
    {
        return new Vector3(attackWidth / 2, 0.1f, range/2);
    }

    protected int CalculateDamage(float chargeAmount,int strength)
    {
        return PlayerData.CalculateMeleeChargeDamage(minDamageAmount, maxDamageAmount, chargeAmount, strength);
    }


}
