﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using RotationSolver.Basic.Traits;

namespace RotationSolver.Basic.Rotations.Basic;

/// <summary>
/// The base class of MNK.
/// </summary>
public abstract class MNK_Base : CustomRotation
{
    /// <summary>
    /// 
    /// </summary>
    public override MedicineType MedicineType => MedicineType.Strength;

    /// <summary>
    /// 
    /// </summary>
    public sealed override Job[] Jobs => new[] { Job.MNK, Job.PGL };

    #region Job Gauge
    static MNKGauge JobGauge => Svc.Gauges.Get<MNKGauge>();

    /// <summary>
    /// 
    /// </summary>
    protected static BeastChakra[] BeastChakras => JobGauge.BeastChakra;

    /// <summary>
    /// 
    /// </summary>
    public static byte Chakra => JobGauge.Chakra;

    /// <summary>
    /// 
    /// </summary>
    public static bool HasSolar => JobGauge.Nadi.HasFlag(Nadi.SOLAR);

    /// <summary>
    /// 
    /// </summary>
    public static bool HasLunar => JobGauge.Nadi.HasFlag(Nadi.LUNAR);
    #endregion

    #region Attack Single
    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction BootShine { get; } = new BaseAction(ActionID.BootShine);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction DragonKick { get; } = new BaseAction(ActionID.DragonKick)
    {
        StatusProvide = new[] { StatusID.LeadenFist },
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction TwinSnakes { get; } = new BaseAction(ActionID.TwinSnakes, ActionOption.Dot);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction TrueStrike { get; } = new BaseAction(ActionID.TrueStrike);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction Demolish { get; } = new BaseAction(ActionID.Demolish, ActionOption.Dot)
    {
        TargetStatus = new StatusID[] { StatusID.Demolish },
        GetDotGcdCount = () => 3,
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction SnapPunch { get; } = new BaseAction(ActionID.SnapPunch);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction SteelPeak { get; } = new BaseAction(ActionID.SteelPeak, ActionOption.UseResources)
    {
        ActionCheck = (b, m) => InCombat && Chakra == 5,
    };
    #endregion

    #region Attack Area
    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction ArmOfTheDestroyer { get; } = new BaseAction(ActionID.ArmOfTheDestroyer);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction FourPointFury { get; } = new BaseAction(ActionID.FourPointFury);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction RockBreaker { get; } = new BaseAction(ActionID.RockBreaker);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction HowlingFist { get; } = new BaseAction(ActionID.HowlingFist)
    {
        ActionCheck = SteelPeak.ActionCheck,
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction SixSidedStar { get; } = new BaseAction(ActionID.SixSidedStar);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction ElixirField { get; } = new BaseAction(ActionID.ElixirField);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction FlintStrike { get; } = new BaseAction(ActionID.FlintStrike);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction RisingPhoenix { get; } = new BaseAction(ActionID.RisingPhoenix);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction CelestialRevolution { get; } = new BaseAction(ActionID.CelestialRevolution);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction TornadoKick { get; } = new BaseAction(ActionID.TornadoKick);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PhantomRush { get; } = new BaseAction(ActionID.PhantomRush);
    #endregion

    #region Support
    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction Mantra { get; } = new BaseAction(ActionID.Mantra, ActionOption.Heal)
    {
        ActionCheck = (b, m) => IsLongerThan(10),
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction RiddleOfEarth { get; } = new BaseAction(ActionID.RiddleOfEarth, ActionOption.Defense)
    {
        StatusProvide = new[] { StatusID.RiddleOfEarth },
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction RiddleOfWind { get; } = new BaseAction(ActionID.RiddleOfWind)
    {
        ActionCheck = (b, m) => IsLongerThan(10),
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PerfectBalance { get; } = new BaseAction(ActionID.PerfectBalance)
    {
        ActionCheck = (b, m) => InCombat && IsLongerThan(5),
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction Meditation { get; } = new BaseAction(ActionID.Meditation);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction FormShift { get; } = new BaseAction(ActionID.FormShift, ActionOption.Buff)
    {
        StatusProvide = new[] { StatusID.FormlessFist, StatusID.PerfectBalance },
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction Brotherhood { get; } = new BaseAction(ActionID.Brotherhood, ActionOption.Buff)
    {
        ActionCheck = (b, m) => IsLongerThan(10)
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction RiddleOfFire { get; } = new BaseAction(ActionID.RiddleOfFire)
    {
        ActionCheck = (b, m) => IsLongerThan(10)
    };
    #endregion

    #region Traits
    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait DeepMeditation { get; } = new BaseTrait(160);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait DeepMeditation2 { get; } = new BaseTrait(245);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait GreasedLightning { get; } = new BaseTrait(364);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedGreasedLightning { get; } = new BaseTrait(365);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedGreasedLightning2 { get; } = new BaseTrait(366);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedGreasedLightning3 { get; } = new BaseTrait(367);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait SteelPeakMastery { get; } = new BaseTrait(428);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait HowlingFistMastery { get; } = new BaseTrait(429);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait ArmOfTheDestroyerMastery { get; } = new BaseTrait(430);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedThunderclap { get; } = new BaseTrait(431);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedBrotherhood { get; } = new BaseTrait(432);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait EnhancedPerfectBalance { get; } = new BaseTrait(433);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait FlintStrikeMastery { get; } = new BaseTrait(512);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait TornadoKickMastery { get; } = new BaseTrait(513);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseTrait MeleeMastery { get; } = new BaseTrait(518);
    #endregion
    
    #region PvP

		/// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Phantomrushcombo { get; } = new BaseAction(ActionID.PvP_Phantomrushcombo);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Bootshine { get; } = new BaseAction(ActionID.PvP_Bootshine);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Truestrike { get; } = new BaseAction(ActionID.PvP_Truestrike);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Snappunch { get; } = new BaseAction(ActionID.PvP_Snappunch);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Dragonkick { get; } = new BaseAction(ActionID.PvP_Dragonkick);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Twinsnakes { get; } = new BaseAction(ActionID.PvP_Twinsnakes);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Demolish { get; } = new BaseAction(ActionID.PvP_Demolish);


    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Phantomrush { get; } = new BaseAction(ActionID.PvP_Phantomrush);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Sixsidedstar { get; } = new BaseAction(ActionID.PvP_Sixsidedstar);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Enlightenment { get; } = new BaseAction(ActionID.PvP_Enlightenment);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Risingphoenix { get; } = new BaseAction(ActionID.PvP_Risingphoenix,ActionOption.Buff);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Riddleofearth { get; } = new BaseAction(ActionID.PvP_Riddleofearth,ActionOption.Buff);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Thunderclap { get; } = new BaseAction(ActionID.PvP_Thunderclap);

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Earthsreply { get; } = new BaseAction(ActionID.PvP_Earthsreply,ActionOption.Buff)
    {
        StatusNeed = new StatusID[] { StatusID.PvP_EarthResonance },
    };

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction PvP_Meteodrive { get; } = new BaseAction(ActionID.PvP_Meteodrive)
    {
        FilterForHostiles = tars => tars.Where(t => t is PlayerCharacter),
        ActionCheck = (t, m) => LimitBreakLevel >= 1,
    };

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static IBaseAction Thunderclap { get; } = new BaseAction(ActionID.Thunderclap, ActionOption.EndSpecial)
    {
        ChoiceTarget = TargetFilter.FindTargetForMoving,
    };

    private protected override IBaseAction LimitBreak => FinalHeaven;

    /// <summary>
    /// LB
    /// </summary>
    public static IBaseAction FinalHeaven { get; } = new BaseAction(ActionID.FinalHeaven)
    {
        ActionCheck = (b, m) => LimitBreakLevel == 3,
    };

    /// <inheritdoc/>
    [RotationDesc(ActionID.Thunderclap)]
    protected sealed override bool MoveForwardAbility(out IAction act)
    {
        if (Thunderclap.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    [RotationDesc(ActionID.Feint)]
    protected sealed override bool DefenseAreaAbility(out IAction act)
    {
        if (Feint.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    [RotationDesc(ActionID.Mantra)]
    protected sealed override bool HealAreaAbility(out IAction act)
    {
        if (Mantra.CanUse(out act)) return true;
        return base.HealAreaAbility(out act);
    }

    /// <inheritdoc/>
    [RotationDesc(ActionID.RiddleOfEarth)]
    protected sealed override bool DefenseSingleAbility(out IAction act)
    {
        if (RiddleOfEarth.CanUse(out act, CanUseOption.EmptyOrSkipCombo)) return true;
        return base.DefenseSingleAbility(out act);
    }
}
