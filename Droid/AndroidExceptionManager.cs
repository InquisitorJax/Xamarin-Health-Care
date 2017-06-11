using Android.App;
using Android.Runtime;
using Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SampleApplication.Droid
{
    public class AndroidExceptionManager : IPlatformExceptionManager
    {
        private const string ErrorFileName = "CrashReport.log";

        public AndroidExceptionManager()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;
        }

        public Task ReportApplicationCrash()
        {
            //TODO: report crash to analytics service
#if DEBUG
            ShowCrashReportDebug();
            return Task.FromResult(default(int));
#endif
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = new Exception("CurrentDomainOnUnhandledException", e.ExceptionObject as Exception);
            LogUnhandledException(exception);
        }

        private void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            e.Handled = true;
            var exception = new Exception("CurrentDomainOnUnhandledException", e.Exception);
            LogUnhandledException(exception);
        }

        private void LogUnhandledException(Exception exception)
        {
            try
            {
                var libraryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, ErrorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}", DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        private void ShowCrashReportDebug()
        {
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, ErrorFileName);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(MainActivity.AppContext)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = new Exception("TaskSchedulerOnUnobservedTaskException", e.Exception);
            LogUnhandledException(exception);
        }
    }
}