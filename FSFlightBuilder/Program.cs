using FSFlightBuilder.Components;
using System;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Common.DataPath =
                $"{Path.GetDirectoryName(Application.ExecutablePath).ToLower().Replace(@"\bin\debug", string.Empty).Replace(@"\bin\release", string.Empty)}\\Data";
            Common.ConfigureLogger();

            try
            {
                Application.ThreadException += Application_ThreadException;

                // Add handler to handle the exception raised by additional threads
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!IsAdministrator())
                {
                    MessageBox.Show(@"FS Flight Builder must be run with administrator privileges.  This can be accomplished by right-clicking on the desktop shortcut, select properties, select the Compatibility tab, select the 'Run this program as an administrator' checkbox.", "Administrator Privileges Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                }

                try
                {
                    Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                    Application.Run(new Main());
                }
                catch (OperationCanceledException)
                {
                }
            }
            catch (Exception err)
            {
                var message = Common.BuildExceptionMessage(err);
                Common.logger.Fatal("An unrecoverable error occurred: {0}", message);
                Common.SendMail($"An unrecoverable error occurred in FS Flight Builder. {message}");
                MessageBox.Show(@"An unrecoverable error occurred in the application.  The developer has been notified.  We apologize for the inconvenience.");
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            NLog.LogManager.Shutdown();
        }

        static void Application_ThreadException
            (object sender, ThreadExceptionEventArgs e)
        {
            ShowExceptionDetails(e.Exception);
        }

        static void CurrentDomain_UnhandledException
            (object sender, UnhandledExceptionEventArgs e)
        {// All exceptions thrown by additional threads are handled in this method

            ShowExceptionDetails(e.ExceptionObject as Exception);

            // Suspend the current thread for now to stop the exception from throwing.
            Thread.CurrentThread.Interrupt();
        }

        static void ShowExceptionDetails(Exception ex)
        {
            var message = Common.BuildExceptionMessage(ex);
            Common.logger.Error("FSFlightBuilder exception: {0}", message);
            Common.SendMail(message.ToString());
        }

        static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
