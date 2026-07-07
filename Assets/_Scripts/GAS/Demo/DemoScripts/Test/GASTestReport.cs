using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GAS.Demo.Test
{
    /// <summary>
    /// 单条测试结果
    /// </summary>
    public class GASTestCaseResult
    {
        /// <summary>
        /// 测试名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Passed;

        /// <summary>
        /// 失败信息
        /// </summary>
        public string Message;
    }

    /// <summary>
    /// GAS 测试报告
    /// </summary>
    public class GASTestReport
    {
        /// <summary>
        /// 测试结果列表
        /// </summary>
        private readonly List<GASTestCaseResult> resultList = new();

        /// <summary>
        /// 通过数量
        /// </summary>
        public int PassCount { get; private set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; private set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount => PassCount + FailCount;

        /// <summary>
        /// 记录通过
        /// </summary>
        public void RecordPass(string name)
        {
            PassCount++;
            resultList.Add(new GASTestCaseResult
            {
                Name = name,
                Passed = true,
                Message = string.Empty
            });
        }

        /// <summary>
        /// 记录失败
        /// </summary>
        public void RecordFail(string name, string message)
        {
            FailCount++;
            resultList.Add(new GASTestCaseResult
            {
                Name = name,
                Passed = false,
                Message = message
            });
        }

        /// <summary>
        /// 输出汇总
        /// </summary>
        public void PrintSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== GAS 系统测试报告 ==========");

            foreach (GASTestCaseResult result in resultList)
            {
                if (result.Passed)
                {
                    sb.AppendLine($"[通过] {result.Name}");
                }
                else
                {
                    sb.AppendLine($"[失败] {result.Name} -> {result.Message}");
                }
            }

            sb.AppendLine("--------------------------------------");
            sb.AppendLine($"总计 {TotalCount} 项  通过 {PassCount}  失败 {FailCount}");
            sb.AppendLine("======================================");

            Debug.Log(sb.ToString());
        }
    }
}
