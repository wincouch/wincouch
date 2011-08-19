using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinCouch
{
    public partial class Form1 : Form
    {
        #region Private Fileds
        Skybound.Gecko.GeckoWebBrowser geckoBrowser;
        Process couchdb;
        
        #endregion

        #region ctor
        public Form1()
        {
            // GeckoFX initialization
            Skybound.Gecko.Xpcom.Initialize(Application.StartupPath + @"\xulrunner");             
            
            InitializeComponent();

            // start CouchDB
            runToolStripButton_Click(runToolStripButton, null);
            geckoBrowser = new Skybound.Gecko.GeckoWebBrowser();
            geckoBrowser.Parent = this.splitContainer1.Panel1;
            geckoBrowser.Dock = DockStyle.Fill;
            geckoBrowser.Navigated += new Skybound.Gecko.GeckoNavigatedEventHandler(geckoBrowser_Navigated);
        } 
        #endregion       

        #region Log Window Output
        delegate void LogAppendTextCallback(string text);

        private void Log(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                LogAppendTextCallback d = new LogAppendTextCallback(Log);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                Debug.WriteLine(richTextBox1.Lines.Length);
                // bad user experiance with RichTextBox with over 500 lines 
                if (richTextBox1.Lines.Length > 500)
                    richTextBox1.Clear();

                this.richTextBox1.AppendText(text);
                this.richTextBox1.AppendText("\n");

                // navigate only when ready!
                if (!browseToolStripButton.Enabled && richTextBox1.Text.Contains("Time to relax."))
                {
                    geckoBrowser.Navigate("localhost:5984/_utils");
                    browseToolStripButton.Enabled = true;
                }
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            Log(args.Data);
        }

        #endregion

        #region Start Stop CouchDB
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GenerateConsoleCtrlEvent(ConsoleCtrlEvent sigevent, int dwProcessGroupId);

        public enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            if (!(sender as ToolStripButton).Checked)
            {
                if (couchdb != null)
                {
                    GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, couchdb.SessionId);
                    couchdb.OutputDataReceived -= OutputDataReceived;
                    if (!couchdb.HasExited)
                        couchdb.Kill();

                    browseToolStripButton.Enabled = false;
                    (sender as ToolStripButton).Text = "Start";
                    (sender as ToolStripButton).ToolTipText = "Start";
                    (sender as ToolStripButton).Image = global::WinCouch.Properties.Resources.db_start;

                }
            }
            else
            {
                if (couchdb != null)
                {
                    couchdb.OutputDataReceived -= OutputDataReceived;
                    if (!couchdb.HasExited)
                        couchdb.Kill();
                }

                richTextBox1.Clear();

                couchdb = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardInput = true;
                startInfo.UseShellExecute = false;
                startInfo.Arguments = "-sasl errlog_type error -s couch";
                startInfo.WorkingDirectory = @".\couchdb\bin";
                startInfo.FileName = @".\couchdb\bin\erl.exe";

                couchdb.StartInfo = startInfo;
                couchdb.OutputDataReceived += OutputDataReceived;
                couchdb.Start();
                couchdb.BeginOutputReadLine();

                (sender as ToolStripButton).Text = "Stop";
                (sender as ToolStripButton).ToolTipText = "Stop";
                (sender as ToolStripButton).Checked = true;
                (sender as ToolStripButton).Image = global::WinCouch.Properties.Resources.db_stop;
            }
        }
        #endregion

        #region Tool Bar Handlers

        void geckoBrowser_Navigated(object sender, Skybound.Gecko.GeckoNavigatedEventArgs e)
        {
            browseToolStripButton.Checked = false;
        }

        private void browseToolStripButton_Click(object sender, EventArgs e)
        {
            geckoBrowser.Navigate("localhost:5984/_utils");
            browseToolStripButton.Checked = true;
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = !statusBarToolStripMenuItem.Checked;
        }

        #region Show In Taskbar

        private void taskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.notifyIcon1.Visible = true;
            this.Visible = false;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = false;
            this.Visible = true;
            geckoBrowser.Navigate("localhost:5984/_utils");
        } 
        #endregion

        private void infoToolStripButton_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }
        #endregion
    }
}
