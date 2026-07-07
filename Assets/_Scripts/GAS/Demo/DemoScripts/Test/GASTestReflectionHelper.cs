using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GAS.Demo.Test
{
    /// <summary>
    /// 测试反射工具
    /// </summary>
    public static class GASTestReflectionHelper
    {
        /// <summary>
        /// 设置私有字段
        /// </summary>
        public static void SetField(object target, string fieldName, object value)
        {
            Type type = target.GetType();
            while (type != null)
            {
                FieldInfo field = type.GetField(
                    fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);

                if (field is not null)
                {
                    field.SetValue(target, value);
                    return;
                }

                type = type.BaseType;
            }

            throw new MissingFieldException(target.GetType().Name, fieldName);
        }

        /// <summary>
        /// 设置组件私有字段
        /// </summary>
        public static void SetComponentField<T>(T component, string fieldName, object value) 
        where T : UnityEngine.Component
        {
            SetField(component, fieldName, value);
        }

        /// <summary>
        /// 设置 ScriptableObject 名称
        /// </summary>
        public static void SetAssetName(ScriptableObject asset, string assetName)
        {
            asset.name = assetName;
        }

        /// <summary>
        /// 创建列表
        /// </summary>
        public static List<T> CreateList<T>(params T[] items)
        {
            return new List<T>(items);
        }
    }
}
