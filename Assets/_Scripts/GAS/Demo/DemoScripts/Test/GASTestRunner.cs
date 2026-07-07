using System.Collections;
using UnityEngine;

namespace GAS.Demo.Test
{
    /// <summary>
    /// GAS 自动测试运行器
    /// </summary>
    [DefaultExecutionOrder(1000)]
    public class GASTestRunner : MonoBehaviour
    {
        /// <summary>
        /// 是否在 Start 时自动运行
        /// </summary>
        [SerializeField] private bool autoRunOnStart = true;

        /// <summary>
        /// 是否测试完成后退出 PlayMode
        /// </summary>
        [SerializeField] private bool quitPlayModeAfterFinish;

        /// <summary>
        /// 启动时自动测试
        /// </summary>
        private void Start()
        {
            if (!autoRunOnStart)
            {
                return;
            }

            StartCoroutine(RunTestsRoutine());
        }

        /// <summary>
        /// 手动运行测试
        /// </summary>
        [ContextMenu("Run GAS Tests")]
        public void RunTests()
        {
            StartCoroutine(RunTestsRoutine());
        }

        /// <summary>
        /// 测试协程
        /// </summary>
        private IEnumerator RunTestsRoutine()
        {
            yield return null;

            GASTestReport report = new GASTestReport();
            Debug.Log("[GASTestRunner] 开始运行 GAS 系统测试");

            try
            {
                GASTestSuites.RunAll(report);
            }
            catch (System.Exception exception)
            {
                report.RecordFail("TestRunner_未捕获异常", exception.Message);
                Debug.LogException(exception);
            }

            report.PrintSummary();

            if (quitPlayModeAfterFinish)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}
