using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TeeScore.Services
{
    public class ErrorReportingService
    {
        public static void ReportError(object sender, Exception e, [CallerMemberName] string caller = null)
        {
            Debug.WriteLine($"*** BEGIN ERROR [{sender.GetType().Name}.{caller}]: {e.Message}");
            Debug.WriteLine($"*** - {e.StackTrace}");

            var inner = e.InnerException;
            var level = 1;
            while (inner != null)
            {
                Debug.WriteLine($"*** [{level}] {inner.Message}");
                level++;
                inner = inner.InnerException;
            }
        }
    }
}
