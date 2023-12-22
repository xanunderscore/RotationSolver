﻿using RotationSolver.Basic.Configuration;

namespace RotationSolver.Basic.Rotations;

public abstract partial class CustomRotation
{
    private static DateTime _nextTimeToHeal = DateTime.MinValue;
    private IAction GCD(IEnumerable<BattleChara> hostilesCastingAOE, IEnumerable<BattleChara> hostilesCastingST)
    {
        IAction act = DataCenter.CommandNextAction;

        BaseAction.SkipDisable = true;
        if (act is IBaseAction a && a != null && a.IsRealGCD && a.CanUse(out _, CanUseOption.MustUse | CanUseOption.EmptyOrSkipCombo)) return act;
        BaseAction.SkipDisable = false;

        if (IsLimitBreak && UseLimitBreak(out act)) return act;

        if (EmergencyGCD(out act)) return act;

        if (RaiseSpell(out act, false)) return act;

        if (IsMoveForward && MoveForwardGCD(out act))
        {
            if (act is IBaseAction b && ObjectHelper.DistanceToPlayer(b.Target) > 5) return act;
        }

        //General Heal
        if ((DataCenter.HPNotFull || ClassJob.GetJobRole() != JobRole.Healer)
            && (InCombat || Service.Config.GetValue(PluginConfigBool.HealOutOfCombat)))
        {
            if (IsHealArea)
            {
                if (HealAreaGCD(out act)) return act;
            }
            if (CanHealAreaSpell)
            {
                BaseAction.AutoHealCheck = true;
                if (HealAreaGCD(out act)) return act;
                BaseAction.AutoHealCheck = false;
            }
            if (IsHealSingle)
            {
                if (HealSingleGCD(out act)) return act;
            }
            if (CanHealSingleSpell)
            {
                BaseAction.AutoHealCheck = true;
                if (HealSingleGCD(out act)) return act;
                BaseAction.AutoHealCheck = false;
            }
        }
        if (IsDefenseArea && DefenseAreaGCD(out act, Array.Empty<BattleChara>())) return act;
        if (IsDefenseSingle && DefenseSingleGCD(out act, Array.Empty<BattleChara>())) return act;

        //Auto Defense
        if (DataCenter.SetAutoStatus(AutoStatus.DefenseArea, hostilesCastingAOE.Any()) && DefenseAreaGCD(out act, hostilesCastingAOE)) return act;
        if (DataCenter.SetAutoStatus(AutoStatus.DefenseSingle, hostilesCastingST.Any()) && DefenseSingleGCD(out act, hostilesCastingST)) return act;

        //Esuna
        if (DataCenter.SetAutoStatus(AutoStatus.Esuna, (IsEsunaStanceNorth
            || !HasHostilesInRange || Service.Config.GetValue(PluginConfigBool.EsunaAll) || (DataCenter.Territory?.IsPvpZone ?? false))
            && WeakenPeople.Any() || DyingPeople.Any()))
        {
            if (ClassJob.GetJobRole() == JobRole.Healer && EsunaAction(out act, CanUseOption.MustUse)) return act;
        }

        if (GeneralGCD(out var action)) return action;

        if (Service.Config.GetValue(PluginConfigBool.HealWhenNothingTodo) && InCombat)
        {
            // Please don't tell me someone's fps is less than 1!!
            if (DateTime.Now - _nextTimeToHeal > TimeSpan.FromSeconds(1))
            {
                var min = Service.Config.GetValue(PluginConfigFloat.HealWhenNothingTodoMin);
                var max = Service.Config.GetValue(PluginConfigFloat.HealWhenNothingTodoMax);
                _nextTimeToHeal = DateTime.Now + TimeSpan.FromSeconds(new Random().NextDouble() * (max - min) + min);
            }
            else if (_nextTimeToHeal < DateTime.Now)
            {
                _nextTimeToHeal = DateTime.Now;

                if (PartyMembersMinHP < Service.Config.GetValue(PluginConfigFloat.HealWhenNothingTodoBelow))
                {
                    if (DataCenter.PartyMembersDifferHP < Service.Config.GetValue(PluginConfigFloat.HealthDifference) && HealAreaGCD(out act)) return act;
                    if (HealSingleGCD(out act)) return act;
                }
            }
        }

        if (Service.Config.GetValue(PluginConfigBool.RaisePlayerByCasting) && RaiseSpell(out act, true)) return act;

        return null;
    }

    private bool UseLimitBreak(out IAction act)
    {
        var role = ClassJob.GetJobRole();
        act = null;

        return LimitBreakLevel switch
        {
            1 => role switch
            {
                JobRole.Tank => ShieldWall.CanUse(out act, CanUseOption.MustUse),
                JobRole.Healer => HealingWind.CanUse(out act, CanUseOption.MustUse),
                JobRole.Melee => Braver.CanUse(out act, CanUseOption.MustUse),
                JobRole.RangedPhysical => BigShot.CanUse(out act, CanUseOption.MustUse),
                JobRole.RangedMagical => Skyshard.CanUse(out act, CanUseOption.MustUse),
                _ => false,
            },
            2 => role switch
            {
                JobRole.Tank => Stronghold.CanUse(out act, CanUseOption.MustUse),
                JobRole.Healer => BreathOfTheEarth.CanUse(out act, CanUseOption.MustUse),
                JobRole.Melee => Bladedance.CanUse(out act, CanUseOption.MustUse),
                JobRole.RangedPhysical => Desperado.CanUse(out act, CanUseOption.MustUse),
                JobRole.RangedMagical => Starstorm.CanUse(out act, CanUseOption.MustUse),
                _ => false,
            },
            3 => LimitBreak?.CanUse(out act, CanUseOption.MustUse) ?? false,
            _ => false,
        };
    }

    private bool RaiseSpell(out IAction act, bool mustUse)
    {
        act = null;
        if (IsRaiseShirk && DataCenter.DeathPeopleAll.Any())
        {
            if (RaiseAction(out act)) return true;
        }

        if ((Service.Config.GetValue(PluginConfigBool.RaiseAll) ? DataCenter.DeathPeopleAll.Any() : DataCenter.DeathPeopleParty.Any())
            && RaiseAction(out act, CanUseOption.IgnoreCastCheck))
        {
            if (HasSwift)
            {
                return DataCenter.SetAutoStatus(AutoStatus.Raise, true);
            }
            else if (mustUse)
            {
                var action = act;
                if (Swiftcast.CanUse(out act))
                {
                    return DataCenter.SetAutoStatus(AutoStatus.Raise, true);
                }
                else if (!IsMoving)
                {
                    act = action;
                    return DataCenter.SetAutoStatus(AutoStatus.Raise, true);
                }
            }
            else if (Service.Config.GetValue(PluginConfigBool.RaisePlayerBySwift) && !Swiftcast.IsCoolingDown
                && NextAbilityToNextGCD > DataCenter.MinAnimationLock + Ping)
            {
                return DataCenter.SetAutoStatus(AutoStatus.Raise, true);
            }
        }
        return DataCenter.SetAutoStatus(AutoStatus.Raise, false);
    }

    private bool RaiseAction(out IAction act, CanUseOption option = CanUseOption.None)
    {
        if (VariantRaise.CanUse(out act, option)) return true;
        if (VariantRaise2.CanUse(out act, option)) return true;
        if (Player.CurrentMp > Service.Config.GetValue(PluginConfigInt.LessMPNoRaise) && (Raise?.CanUse(out act, option) ?? false)) return true;

        return false;
    }

    private static bool EsunaAction(out IAction act, CanUseOption option = CanUseOption.None)
    {
        if (Esuna.CanUse(out act, option)) return true;

        return false;
    }

    /// <summary>
    /// The emergency gcd with highest priority.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected virtual bool EmergencyGCD(out IAction act)
    {
        #region Bozja
        if (LostSpellforge.CanUse(out act)) return true;
        if (LostSteelsting.CanUse(out act)) return true;
        if (LostRampage.CanUse(out act)) return true;
        if (LostBurst.CanUse(out act)) return true;

        if (LostBravery.CanUse(out act)) return true;
        if (LostBubble.CanUse(out act)) return true;
        if (LostShell2.CanUse(out act)) return true;
        if (LostShell.CanUse(out act)) return true;
        if (LostProtect2.CanUse(out act)) return true;
        if (LostProtect.CanUse(out act)) return true;

        //Add your own logic here.
        //if (LostFlarestar.CanUse(out act)) return true;
        //if (LostSeraphStrike.CanUse(out act)) return true;

        #endregion

        #region PvP
        if (PvP_Guard.CanUse(out act)
            && (Player.GetHealthRatio() <= Service.Config.GetValue(PluginConfigFloat.HealthForGuard)
            || IsRaiseShirk)) return true;

        if (PvP_StandardIssueElixir.CanUse(out act)) return true;
        #endregion
        act = null; return false;
    }

    /// <summary>
    /// Moving forward GCD.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [RotationDesc(DescType.MoveForwardGCD)]
    protected virtual bool MoveForwardGCD(out IAction act)
    {
        act = null; return false;
    }

    /// <summary>
    /// Heal single GCD.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [RotationDesc(DescType.HealSingleGCD)]
    protected virtual bool HealSingleGCD(out IAction act)
    {
        if (VariantCure.CanUse(out act)) return true;
        if (VariantCure2.CanUse(out act)) return true;
        return false;
    }

    /// <summary>
    /// Heal area GCD.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [RotationDesc(DescType.HealAreaGCD)]
    protected virtual bool HealAreaGCD(out IAction act)
    {
        act = null; return false;
    }

    /// <summary>
    /// Defense single gcd.
    /// </summary>
    /// <param name="act"></param>
    /// <param name="hostiles">The attacking hostiles.</param>
    /// <returns></returns>
    [RotationDesc(DescType.DefenseSingleGCD)]
    protected virtual bool DefenseSingleGCD(out IAction act, IEnumerable<BattleChara> hostiles)
    {
        if (LostStoneskin.CanUse(out act)) return true;

        act = null; return false;
    }

    /// <summary>
    /// Defense single gcd.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [Obsolete("Use DefenseSingleGCD(act, hostiles)")]
    [RotationDesc(DescType.DefenseSingleGCD)]
    protected virtual bool DefenseSingleGCD(out IAction act)
    {
        return DefenseSingleGCD(out act, Array.Empty<BattleChara>());
    }


    /// <summary>
    /// Defense area gcd.
    /// </summary>
    /// <param name="act"></param>
    /// <param name="hostiles">The attacking hostiles.</param>
    /// <returns></returns>
    [RotationDesc(DescType.DefenseAreaGCD)]
    protected virtual bool DefenseAreaGCD(out IAction act, IEnumerable<BattleChara> hostiles)
    {
        if (LostStoneskin2.CanUse(out act)) return true;

        act = null; return false;
    }

    /// <summary>
    /// Defense area gcd.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [Obsolete("Use DefenseAreaGCD(act, hostiles)")]
    [RotationDesc(DescType.DefenseAreaGCD)]
    protected virtual bool DefenseAreaGCD(out IAction act)
    {
        return DefenseAreaGCD(out act, Array.Empty<BattleChara>());
    }

    /// <summary>
    /// General GCD.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected virtual bool GeneralGCD(out IAction act)
    {
        act = null; return false;
    }
}
