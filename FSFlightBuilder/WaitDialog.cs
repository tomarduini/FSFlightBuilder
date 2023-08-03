using System;
using System.Threading;
using System.Windows.Forms;

namespace FSFlightbuilder
{
    public class WaitDialog : IDisposable
    {
        public WaitDialog(Action codeToRun, string message, Form dlg)
        {
            ManualResetEvent dialogLoadedFlag = new ManualResetEvent(false);

            // open the dialog on a new thread so that the dialog window gets
            // drawn. otherwise our long running code will run and the dialog
            // window never renders.
            (new Thread(() =>
            {
                Form waitDialog = new Form()
                {
                    Name = "WaitForm",
                    Text = message, //"Please Wait...",
                    ControlBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    Width = 240,
                    Height = 50,
                    Enabled = true
                };

                ProgressBar ScrollingBar = new ProgressBar()
                {
                    Style = ProgressBarStyle.Marquee,
                    Parent = waitDialog,
                    Dock = DockStyle.Fill,
                    Enabled = true
                };

                waitDialog.Load += new EventHandler((x, y) =>
                {
                    dialogLoadedFlag.Set();
                });

                waitDialog.Shown += new EventHandler((x, y) =>
                {
                    (new Thread(() =>
                    {
                        codeToRun();

                        dlg.Invoke((MethodInvoker)(() => waitDialog.Close()));
                    })).Start();
                });

                dlg.Invoke((MethodInvoker)(() => waitDialog.ShowDialog(dlg)));
            })).Start();

            while (dialogLoadedFlag.WaitOne(100, true) == false)
                Application.DoEvents(); // note: this will block the main thread once the wait dialog shows
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
