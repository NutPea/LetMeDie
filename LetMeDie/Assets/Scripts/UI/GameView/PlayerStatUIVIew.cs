using TMPro;
using UnityEngine;

public class PlayerStatUIVIew : MonoBehaviour
{
    private PlayerData playerData;

    [Header("Movement")]
    //[SerializeField] private TextMeshProUGUI movementSpeedText;
    // [SerializeField] private TextMeshProUGUI jumpForceText;
    [SerializeField] private TextMeshProUGUI extramovementSpeedText;

    [Header("Combat")]
  //  [SerializeField] private TextMeshProUGUI staminaText;
  //  [SerializeField] private TextMeshProUGUI staminaRegenerationText;
    [SerializeField] private TextMeshProUGUI healthText;
  //  [SerializeField] private TextMeshProUGUI extraHealthText;
    [SerializeField] private TextMeshProUGUI healthRegRateText;
    [SerializeField] private TextMeshProUGUI weaponBaseDamagePercentageText;
    [SerializeField] private TextMeshProUGUI spellBaseDamagePercentageText;
    [SerializeField] private TextMeshProUGUI knockBackPercentageText;
    [SerializeField] private TextMeshProUGUI spellManaReductionText;
    [SerializeField] private TextMeshProUGUI spellManaRegenerationText;
    [SerializeField] private TextMeshProUGUI lifeStealPercentageText;
    [SerializeField] private TextMeshProUGUI weaponChargeTimeText;
    [SerializeField] private TextMeshProUGUI extraAttackSpeedText;
    [SerializeField] private TextMeshProUGUI weaponExtraChargeDamageText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI extraCritDamageText;
    [SerializeField] private TextMeshProUGUI luckText;
    [SerializeField] private TextMeshProUGUI extraAttackSizeText;
    [SerializeField] private TextMeshProUGUI evasionText;
    [SerializeField] private TextMeshProUGUI extraAmountOfProjectilesText;
 //   [SerializeField] private TextMeshProUGUI extraAmountOfProjectilesPercentText;
  //  [SerializeField] private TextMeshProUGUI forceProjectileSpreadText;
  //  [SerializeField] private TextMeshProUGUI extraManaKillAmountText;
  //  [SerializeField] private TextMeshProUGUI amountOfExtraCastsText;


    private void OnEnable()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        if(playerData == null)
        {
            if(SGameManager.Instance == null)
            {
                return;
            }
            playerData = SGameManager.Instance.PlayerBody.GetComponent<PlayerStatHandler>().PlayerData;
        }

        // Movement
       // SetStat(movementSpeedText, "Movement Speed", playerData.MovementSpeed);
      //  SetStat(jumpForceText, "Jump Force", playerData.JumpForce);

        // Combat
      //  SetStat(staminaText, "Stamina", playerData.Stamina);
   //     SetStat(staminaRegenerationText, "Stamina Regeneration", playerData.StaminaRegeneration);
        SetStat(healthText, "Health", playerData.Health);
        SetStat(extramovementSpeedText, "Extra MovementSpeed", playerData.ExtraMovementSpeedPercent);
        //   SetStat(extraHealthText, "Extra Health", playerData.ExtraHealth);
        SetStat(healthRegRateText, "Health Regeneration Rate", playerData.HealthRegRate);
        SetStat(weaponBaseDamagePercentageText, "Weapon Damage", playerData.WeaponBaseDamagePercentage);
        SetStat(spellBaseDamagePercentageText, "Spell Damage", playerData.SpellBaseDamagePercentage);
        SetStat(knockBackPercentageText, "Knockback", playerData.KnockBackPercentage);
        SetStat(spellManaReductionText, "Spell Mana Reduction", playerData.SpellManaReduction);
        SetStat(spellManaRegenerationText, "Spell Mana Regeneration", playerData.SpellManaRegeneration);
        SetStat(lifeStealPercentageText, "Life Steal", playerData.LifeStealPercentage);
        SetStat(weaponChargeTimeText, "Weapon Charge Time", playerData.WeaponChargeTime);
        SetStat(extraAttackSpeedText, "Attack Speed", playerData.ExtraChargeSpeedPercentage);
        SetStat(weaponExtraChargeDamageText, "Charge Damage", playerData.WeaponExtraChargeDamage);
        SetStat(critChanceText, "Crit Chance", playerData.CritChance);
        SetStat(extraCritDamageText, "Crit Damage", playerData.ExtraCritDamage);
        SetStat(luckText, "Luck", playerData.Luck);
        SetStat(extraAttackSizeText, "Attack Size", playerData.ExtraAttackSize);
        SetStat(evasionText, "Evasion", playerData.Evasion);
        SetStat(extraAmountOfProjectilesText, "Extra Projectiles", playerData.ExtraAmountOfProjectiles);
      //  SetStat(extraAmountOfProjectilesPercentText, "Projectile Amount %", playerData.ExtraAmountOfProjectilesPercent);
       // SetStat(forceProjectileSpreadText, "Force Projectile Spread", playerData.ForceProjectileSpread);
      //  SetStat(extraManaKillAmountText, "Mana on Kill", playerData.ExtraManaKillAmount);
        //SetStat(amountOfExtraCastsText, "Extra Casts", playerData.AmountOfExtraCasts);
    }

    private void SetStat(TextMeshProUGUI text, string statName, object value)
    {
        if (text == null)
            return;

        text.text = $"{statName} : {FormatValue(value)}";
    }

    private string FormatValue(object value)
    {
        if (value is float floatValue)
            return (floatValue * 100).ToString("F1") + " %";

        return value.ToString();
    }
}
