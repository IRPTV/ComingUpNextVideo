using MahApps.Metro.Controls;
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

namespace ComingUpVideoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string file1 = string.Empty;
        string file2 = string.Empty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(file1.Length==0)
            {
                var res = MessageBox.Show("Please select File1", "Warning", MessageBoxButton.OK);
                return;
            }
            if (file2.Length == 0)
            {
                var res = MessageBox.Show("Please select File2", "Warning", MessageBoxButton.OK);
                return;
            }
            String exePath = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            string d = DateTime.Now.ToString("yyyyMMdd-hhmmss");
            string dir = System.IO.Path.GetDirectoryName(exePath)+"\\tmp\\"+d;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.Copy(file1, dir + "\\1.mp4");
            File.Copy(file2, dir + "\\2.mp4");


            StreamWriter str = new StreamWriter(dir+"\\CUNVDATA.xml");
            str.WriteLine("Program1 = [\"" +txtTitle1.Text.Trim()+ "\"]");
            str.WriteLine("Program2 = [\"" + txtTitle2.Text.Trim() + "\"]");
            str.WriteLine("Time1 = [\"" + txtTime1.Text.Trim() + "\"]");
            str.WriteLine("Time2 = [\"" + txtTime2.Text.Trim() + "\"]");
            str.Close();

            MessageBox.Show("Request has been sent", "Upload", MessageBoxButton.OK);
            CopyFolder(dir, "\\\\192.168.20.48\\input$\\ComingUpVideo\\"+d);

        }

        private void btnBrowse1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ShowDialog();
            file1 = openFileDialog.FileName;
        }

        private void btnBrowse2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ShowDialog();
            file2 = openFileDialog.FileName;
        }
        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
    }
}
