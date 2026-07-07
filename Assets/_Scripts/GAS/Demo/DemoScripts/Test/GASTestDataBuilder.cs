using System.Collections.Generic;
using GAS.Core.GameplayEffect;
using GAS.StateSystem;
using UnityEngine;

namespace GAS.Demo.Test
{
    /// <summary>
    /// 测试数据构建器
    /// </summary>
    public static class GASTestDataBuilder
    {
        /// <summary>
        /// 创建属性数据
        /// </summary>
        public static StatData CreateStatData(
            string statId,
            E_StatType statType,
            float baseValue,
            float minValue = float.MinValue,
            float maxValue = float.MaxValue)
        {
            StatData data = ScriptableObject.CreateInstance<StatData>();
            GASTestReflectionHelper.SetAssetName(data, statId);
            GASTestReflectionHelper.SetField(data, "statType", statType);
            GASTestReflectionHelper.SetField(data, "baseValue", baseValue);
            GASTestReflectionHelper.SetField(data, "minValue", minValue);
            GASTestReflectionHelper.SetField(data, "maxValue", maxValue);
            GASTestReflectionHelper.SetField(data, "resetCurrentValueOnPlay", true);
            return data;
        }

        /// <summary>
        /// 创建 GE 数据
        /// </summary>
        public static GameplayEffectData CreateGameplayEffect(
            string effectName,
            E_EffectDuration durationPolicy,
            params StatModifierConfig[] modifiers)
        {
            return CreateGameplayEffect(
                effectName,
                durationPolicy,
                5f,
                false,
                1f,
                E_EffectStacking.None,
                1,
                System.Array.Empty<string>(),
                System.Array.Empty<string>(),
                System.Array.Empty<string>(),
                modifiers);
        }

        /// <summary>
        /// 创建 GE 数据
        /// </summary>
        public static GameplayEffectData CreateGameplayEffect(
            string effectName,
            E_EffectDuration durationPolicy,
            float duration,
            bool isPeriodic,
            float period,
            E_EffectStacking stackingPolicy,
            int stackLimit,
            string[] gameplayTags,
            string[] requiredNeedTags,
            string[] requiredBanTags,
            params StatModifierConfig[] modifiers)
        {
            GameplayEffectData data = ScriptableObject.CreateInstance<GameplayEffectData>();
            GASTestReflectionHelper.SetAssetName(data, effectName);
            GASTestReflectionHelper.SetField(data, "gameplayTags", gameplayTags ?? System.Array.Empty<string>());
            GASTestReflectionHelper.SetField(data, "requiredNeedTags", requiredNeedTags ?? System.Array.Empty<string>());
            GASTestReflectionHelper.SetField(data, "requiredBanTags", requiredBanTags ?? System.Array.Empty<string>());
            GASTestReflectionHelper.SetField(data, "durationPolicy", durationPolicy);
            GASTestReflectionHelper.SetField(data, "duration", duration);
            GASTestReflectionHelper.SetField(data, "isPeriodic", isPeriodic);
            GASTestReflectionHelper.SetField(data, "period", period);
            GASTestReflectionHelper.SetField(data, "executePeriodicEffectOnApplication", true);
            GASTestReflectionHelper.SetField(data, "stackingPolicy", stackingPolicy);
            GASTestReflectionHelper.SetField(data, "stackLimit", stackLimit);
            GASTestReflectionHelper.SetField(data, "durationRefreshPolicy", E_EffectDurationRefresh.RefreshOnSuccessfulApplication);
            GASTestReflectionHelper.SetField(data, "periodResetPolicy", E_EffectPeriodReset.ResetOnSuccessfulApplication);
            GASTestReflectionHelper.SetField(data, "expirationPolicy", E_EffectExpiration.ClearEntireStack);
            GASTestReflectionHelper.SetField(data, "statModifierConfig", new List<StatModifierConfig>(modifiers));
            return data;
        }

        /// <summary>
        /// 创建修饰符配置
        /// </summary>
        public static StatModifierConfig CreateModifierConfig(string statId, E_ModifierType type, float value)
        {
            return new StatModifierConfig
            {
                statId = statId,
                type = type,
                value = value,
                priority = 0
            };
        }
    }
}
