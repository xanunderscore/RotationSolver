using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlus.Combos;

internal class SAMCombo : CustomComboJob<SAMGauge>
{
    internal override uint JobID => 34;
    private static bool _shouldUseGoken = false;
    private static bool _shouldUseSetsugekka = false;
    private static bool _shouldUseOgiNamikiri = false;
    private static byte SenCount => (byte)((JobGauge.HasGetsu ? 1 : 0) + (JobGauge.HasSetsu ? 1 : 0) + (JobGauge.HasKa ? 1 : 0));
    internal struct Actions
    {
        public static readonly BaseAction
            //刃风
            Hakaze = new BaseAction(7477),

            //阵风
            Jinpu = new BaseAction(7478),

            //心眼
            ThirdEye = new BaseAction(7498),

            //燕飞
            Enpi = new BaseAction(7486),

            //士风
            Shifu = new BaseAction(7479),

            //风雅
            Fuga = new BaseAction(7483),

            //月光
            Gekko = new BaseAction(7481)
            {
                EnermyLocation = EnemyLocation.Back,
            },

            //彼岸花
            Higanbana = new BaseAction(7489)
            {
                TargetStatus = new ushort[] {ObjectStatus.Higanbana},
                OtherCheck = b => SenCount > 0,
            },

            //天下五剑
            TenkaGoken = new BaseAction(7488)
            {
                OtherCheck = b => SenCount > 1,
                AfterUse = () => _shouldUseGoken = true,
            },

            //纷乱雪月花
            MidareSetsugekka = new BaseAction(7487)
            {
                OtherCheck = b => SenCount > 2,
                AfterUse = () => _shouldUseSetsugekka = true,
            },

            //满月
            Mangetsu = new BaseAction(7484),

            //花车
            Kasha = new BaseAction(7482)
            {
                EnermyLocation = EnemyLocation.Side,
            },

            //樱花
            Oka = new BaseAction(7485),

            //明镜止水
            MeikyoShisui = new BaseAction(7499),

            //雪风
            Yukikaze = new BaseAction(7480),

            //必杀剑·回天
            HissatsuKaiten = new BaseAction(7494),

            //必杀剑·晓天
            HissatsuGyoten = new BaseAction(7492),

            //必杀剑·震天
            HissatsuShinten = new BaseAction(7490),

            //必杀剑·九天
            HissatsuKyuten = new BaseAction(7491),

            //意气冲天
            Ikishoten = new BaseAction(16482),

            //必杀剑·红莲
            HissatsuGuren = new BaseAction(7496),

            //必杀剑·闪影
            HissatsuSenei = new BaseAction(16481),

            //燕回返
            Tsubamegaeshi = new BaseAction(16483),

            //回返五剑
            KaeshiGoken = new BaseAction(16485)
            {
                OtherCheck = b => _shouldUseGoken,
                AfterUse = () => _shouldUseGoken = false,
            },

            //回返雪月花
            KaeshiSetsugekka = new BaseAction(16486)
            {
                OtherCheck = b => _shouldUseSetsugekka,
                AfterUse = () => _shouldUseSetsugekka = false,
            },

            //照破
            Shoha = new BaseAction(16487),

            //无明照破
            Shoha2 = new BaseAction(25779),

            //奥义斩浪
            OgiNamikiri = new BaseAction(25781)
            {
                BuffsNeed = new ushort[] { ObjectStatus.OgiNamikiriReady },
                AfterUse = () => _shouldUseOgiNamikiri = true,
            },

            //回返斩浪
            KaeshiNamikiri = new BaseAction(25782)
            {
                OtherCheck = b => _shouldUseOgiNamikiri,
                AfterUse = () => _shouldUseOgiNamikiri = false,
            };
    }

    private protected override bool GeneralGCD(uint lastComboActionID, out BaseAction act)
    {
        if (Actions.OgiNamikiri.ShouldUseAction(out act, mustUse:true)) return true;
        if (Actions.TenkaGoken.ShouldUseAction(out act)) return true;
        if (Actions.Higanbana.ShouldUseAction(out act)) return true;
        if (Actions.MidareSetsugekka.ShouldUseAction(out act)) return true;


        //123
        bool haveMeikyoShisui = BaseAction.HaveStatusSelfFromSelf(ObjectStatus.MeikyoShisui);
        //如果是单体，且明镜止水的冷却时间小于3秒。
        if (!JobGauge.HasSetsu && !Actions.Fuga.ShouldUseAction(out _) && Actions.MeikyoShisui.RecastTimeRemain < 3)
        {
            if (Actions.Yukikaze.ShouldUseAction(out act, lastComboActionID)) return true;
        }
        if (!JobGauge.HasGetsu)
        {
            if (Actions.Mangetsu.ShouldUseAction(out act, lastComboActionID)) return true;
            if (Actions.Gekko.ShouldUseAction(out act, lastComboActionID, mustUse: haveMeikyoShisui)) return true;
            if (Actions.Jinpu.ShouldUseAction(out act, lastComboActionID)) return true;
        }
        if (!JobGauge.HasKa)
        {
            if (Actions.Oka.ShouldUseAction(out act, lastComboActionID)) return true;
            if (Actions.Kasha.ShouldUseAction(out act, lastComboActionID, mustUse: haveMeikyoShisui)) return true;
            if (Actions.Shifu.ShouldUseAction(out act, lastComboActionID)) return true;
        }
        if (!JobGauge.HasSetsu)
        {
            if (Actions.Yukikaze.ShouldUseAction(out act, lastComboActionID)) return true;
        }
        if (Actions.Fuga.ShouldUseAction(out act, lastComboActionID)) return true;
        if (Actions.Hakaze.ShouldUseAction(out act, lastComboActionID)) return true;



        if (IconReplacer.Move && MoveAbility(1, out act)) return true;
        if (!haveMeikyoShisui && Actions.Enpi.ShouldUseAction(out act)) return true;

        return false;
    }

    private protected override bool MoveAbility(byte abilityRemain, out BaseAction act)
    {
        if (JobGauge.Kenki >= 30 && Actions.HissatsuGyoten.ShouldUseAction(out act)) return true;
        act = null;
        return false;
    }

    private protected override bool ForAttachAbility(byte abilityRemain, out BaseAction act)
    {
        if (Actions.KaeshiNamikiri.ShouldUseAction(out act)) return true;
        if (Actions.KaeshiNamikiri.ShouldUseAction(out _, mustUse:true, Empty: true))
        {
            if (Actions.KaeshiGoken.ShouldUseAction(out act)) return true;
            if (Actions.KaeshiSetsugekka.ShouldUseAction(out act)) return true;
        }
        else
        {
            _shouldUseGoken = _shouldUseSetsugekka = false;
        }

        if (JobGauge.MeditationStacks == 3)
        {
            if (Actions.Shoha2.ShouldUseAction(out act)) return true;
            if (Actions.Shoha.ShouldUseAction(out act)) return true;
        }

        if (JobGauge.Kenki >= 45)
        {
            if (Actions.HissatsuGuren.ShouldUseAction(out act)) return true;
            if (Actions.HissatsuKyuten.ShouldUseAction(out act)) return true;

            if (Actions.HissatsuSenei.ShouldUseAction(out act)) return true;
            if (Actions.HissatsuShinten.ShouldUseAction(out act)) return true;
        }
        else
        {
            if (Actions.Ikishoten.ShouldUseAction(out act)) return true;
        }
        act = null;
        return false;
    }

    private protected override bool EmergercyAbility(byte abilityRemain, BaseAction nextGCD, out BaseAction act)
    {
        if(nextGCD.ActionID == Actions.Hakaze.ActionID && JobGauge.HasSetsu && (!JobGauge.HasGetsu || !JobGauge.HasKa))
        {
            if (Actions.MeikyoShisui.ShouldUseAction(out act)) return true;
        }
        if(nextGCD.ActionID == Actions.TenkaGoken.ActionID || nextGCD.ActionID == Actions.Higanbana.ActionID || nextGCD.ActionID == Actions.MidareSetsugekka.ActionID)
        {
            if (JobGauge.Kenki >= 20 && Actions.HissatsuKaiten.ShouldUseAction(out act)) return true;
        }

        act = null;
        return false;
    }

    private protected override bool DefenceSingleAbility(byte abilityRemain, out BaseAction act)
    {
        if (Actions.ThirdEye.ShouldUseAction(out act)) return true;
        return false;
    }
}
