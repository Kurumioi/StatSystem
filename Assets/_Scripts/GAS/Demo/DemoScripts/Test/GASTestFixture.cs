using System.Collections.Generic;
using GAS.AbilitySystem;
using GAS.Component;
using GAS.Core;
using GAS.StateSystem;
using UnityEngine;

namespace GAS.Demo.Test
{
    /// <summary>
    /// 测试对象包装
    /// </summary>
    public class GASTestSubject
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public GameObject Root { get; private set; }

        /// <summary>
        /// 能力组件
        /// </summary>
        public AbilitySystemComponent AbilitySystem { get; private set; }

        /// <summary>
        /// GE 管理器
        /// </summary>
        public GEManager GEManager { get; private set; }

        /// <summary>
        /// 属性控制器
        /// </summary>
        public StatController StatController { get; private set; }

        /// <summary>
        /// 运行时资源列表
        /// </summary>
        private readonly List<Object> runtimeAssetList = new();

        /// <summary>
        /// 创建测试对象
        /// </summary>
        public static GASTestSubject Create(string objectName, params StatData[] statDataArray)
        {
            GASTestSubject subject = new GASTestSubject();
            subject.Root = new GameObject(objectName);
            subject.StatController = subject.Root.AddComponent<StatController>();
            subject.GEManager = subject.Root.AddComponent<GEManager>();
            subject.AbilitySystem = subject.Root.AddComponent<AbilitySystemComponent>();

            List<StatData> statDataList = new List<StatData>(statDataArray);
            foreach (StatData statData in statDataArray)
            {
                subject.runtimeAssetList.Add(statData);
            }

            GASTestReflectionHelper.SetComponentField(subject.StatController, "statDataList", statDataList);
            GASTestReflectionHelper.SetComponentField(subject.AbilitySystem, "geManager", subject.GEManager);
            GASTestReflectionHelper.SetComponentField(subject.AbilitySystem, "statController", subject.StatController);
            GASTestReflectionHelper.SetComponentField(subject.AbilitySystem, "initialAbilities", new List<GameplayAbility>());

            subject.StatController.Init();
            subject.GEManager.SetStatController(subject.StatController);
            subject.GEManager.SetOwner(subject.AbilitySystem);
            return subject;
        }

        /// <summary>
        /// 设置初始标签
        /// </summary>
        public void SetInitialTags(params string[] tagNames)
        {
            foreach (string tagName in tagNames)
            {
                AbilitySystem.AddGameplayTag(tagName);
            }
        }

        /// <summary>
        /// 注册运行时资源
        /// </summary>
        public void TrackRuntimeAsset(Object asset)
        {
            runtimeAssetList.Add(asset);
        }

        /// <summary>
        /// 销毁测试对象
        /// </summary>
        public void Destroy()
        {
            if (Root != null)
            {
                Object.Destroy(Root);
            }

            foreach (Object asset in runtimeAssetList)
            {
                if (asset != null)
                {
                    Object.Destroy(asset);
                }
            }

            runtimeAssetList.Clear();
        }
    }
}
