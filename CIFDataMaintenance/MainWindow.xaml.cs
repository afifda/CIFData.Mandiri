using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIFDataMaintenance.BusinessLogic;
using System.Data;
using CIFDataMaintenance.Entity;

namespace CIFDataMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDlg = new OpenFileDialog();

            if (fileDlg.ShowDialog() == true)
            {
                new FileInfo(fileDlg.FileName);
                using (Stream s = fileDlg.OpenFile())
                {
                    attachTxtBox.Text = fileDlg.FileName;
                }
            }

        }

        private void migrateBtn_Click(object sender, RoutedEventArgs e)
        {
            string logPath = Environment.CurrentDirectory + "\\logs\\MirationLog_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";
            string file = attachTxtBox.Text;
            string ext = System.IO.Path.GetExtension(file);
            BaseLogic logic = new BaseLogic();
            DataSet cmdExcel = CallEntityBulkCopy(logPath, file, ext, logic);
            MessageBox.Show("Success");
        }

        private static DataSet CallEntityBulkCopy(string logPath, string file, string ext, BaseLogic logic)
        {
            string stagingTable = "MonitoringResultTemp";
            DataSet cmdExcel = null;
            cmdExcel = logic.BulkCopyToStagingTable<CIFDataQualityEntity>(file, ext, stagingTable, logPath);
            return cmdExcel;
        }

        private void CusInsMenu_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
