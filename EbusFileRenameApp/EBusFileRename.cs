using EbusFileRenameApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EbusFileRenameApp
{
    public partial class EbusFileRename : Form
    {
        public EbusFileRename()
        {
            InitializeComponent();
        }
        private BackgroundWorker worker;
        public static object thisLock = new object();
        private ReaderWriterLockSlim fileLock = new ReaderWriterLockSlim();
        private Helper helper = new Helper();

        private void EbusFileRename_Load(object sender, EventArgs e)
        {
            try
            {
                worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(StartProcess);
                InitializeRefreshTimer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StartProcess(object sender, EventArgs e)
        {
            while (true)
            {
                #region logTrigger
                List<string> files = helper.DirSearch(Constants.SourceFilePath);
                if (files.Any())
                {
                    files.ForEach(x =>
                    {
                        string fileName = Path.GetFileName(x);
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(x);
                        if (fileName.StartsWith("00_") || (fileNameWithoutExt.Length == 6 && int.TryParse(fileNameWithoutExt, out int number)))
                        {
                            string result = string.Empty;
                            int seeder = 0;
                        FileExist:
                            seeder++;
                            result = Path.ChangeExtension(x, ".0" + seeder.ToString());
                            File.Move(x, result);
                            if (helper.Exists(result, Constants.ExtRenameFilePath))
                            {
                                goto FileExist;
                            }

                            helper.MoveFile(result, Constants.ExtRenameFilePath);
                        }
                        else if (fileName.StartsWith("status"))
                        {
                            if (Constants.DeleteStatusFiles)
                            {
                                File.Delete(x);
                            }
                            else
                            {
                                helper.MoveFile(x, Constants.StatusFilePath);
                            }
                        }
                        else
                        {
                            helper.MoveFile(x, Constants.OthersFilePath);
                        }
                    });

                }
                #endregion
            }
        }

        public void InitializeRefreshTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += TriggerStartProcess;
            timer.Start();
        }

        private void TriggerStartProcess(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }
    }
}
