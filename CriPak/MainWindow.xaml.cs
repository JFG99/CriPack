using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using Ookii.Dialogs.Wpf;
using System.Windows.Threading;
using CriPakInterfaces.Models;
using CriPakRepository.Helpers;
using System.Threading.Tasks;

namespace CriPakComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public CriPak criPak = new CriPak();
        private readonly Orchestrator _home;
        public MainWindow(Orchestrator home)
        {
            _home = home;
            InitializeComponent();
            SetBasicPrefs();
        }
        private void SetBasicPrefs()
        {
            menu_savefiles.IsEnabled = false;
            menu_patch.IsEnabled = false;
            progressbar0.Maximum = 100;
        }
        private void menu_openfile_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Loading cpk");
            string fName;
            string baseName;
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            openFileDialog.InitialDirectory = "";
            openFileDialog.Filter = "Criware CPK|*.cpk";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog().Value)
            {
                fName = openFileDialog.FileName;
                baseName = System.IO.Path.GetFileName(fName);
                status_cpkname.Content = baseName;
                beginLoadCPK(fName);
                button_extract.IsEnabled = true;
                button_importassets.IsEnabled = true;

            }
        }
        private void beginLoadCPK(string inFile)
        {
            criPak.FilePath = inFile;

            _ = Task.Run(() => 
                Dispatcher.Invoke(() => 
                {
                    criPak = _home.Read(criPak);
                    datagrid_cpk.ItemsSource = criPak.DisplayList;
                    status_cpkmsg.Content = string.Format($"{datagrid_cpk.Items.Count} file(s) registered.");
                    menu_patch.IsEnabled = true;
                    menu_savefiles.IsEnabled = true;
                })
            );
        }
        private void Patch_Click(object sender, RoutedEventArgs e)
        {
            CpkPatcher patcherWindow = new CpkPatcher(_home, criPak, Top, Left);
            patcherWindow.ShowDialog();
        }
        private void updateDatagrid(bool value = false)
        {
            datagrid_cpk.IsEnabled = value;
            button_extract.IsEnabled = value;
            button_importassets.IsEnabled = value;
        }
        private void beginExtractCPK(string foutDir)
        {
            criPak.OutputDirectory = foutDir;
            _home.Extract(criPak);
            Dispatcher.Invoke(() => progressbar0.Value = 100f);
            Dispatcher.Invoke(() => updateDatagrid(true));
            MessageBox.Show("Extraction Complete.");
           
        }
        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog saveFilesDialog = new VistaFolderBrowserDialog();
            saveFilesDialog.SelectedPath = criPak.BasePath + "/";
            if (saveFilesDialog.ShowDialog().Value)
            {
                Debug.Print(saveFilesDialog.SelectedPath + "/" + criPak.Name + "_unpacked");
                Task.Run(() => beginExtractCPK(saveFilesDialog.SelectedPath));
            }
        }        
        private void menu_aboutgui_Click(object sender, RoutedEventArgs e)
        {
            //WindowAboutGUI aboutwindow = new WindowAboutGUI(this.Top, this.Left);
            //aboutwindow.ShowDialog();
        }
        private void dgmenu1_Cilck(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this.datagrid_cpk);
            HitTestResult htr = VisualTreeHelper.HitTest(this.datagrid_cpk, p);
            TextBlock o = htr.VisualHit as TextBlock;
            if (o != null)
            {
                DataGridRow dgr = VisualTreeHelper.GetParent(o) as DataGridRow;

                dgr.Focus();
                dgr.IsSelected = true;
            }
        }
        private void dgitem1_Click(object sender, RoutedEventArgs e)
        {

            var t = this.datagrid_cpk.SelectedItem as PackagedFile;
            if (t != null)
            {
                if (t.CompressedFileSize > 0 && t.FileType == "FILE")
                {
                    VistaSaveFileDialog saveFilesDialog = new VistaSaveFileDialog();
                    saveFilesDialog.InitialDirectory = criPak.BasePath;
                    saveFilesDialog.FileName = criPak.BasePath + "/" + t.LocalName;
                    if (saveFilesDialog.ShowDialog().Value)
                    {
                        byte[] chunk = ExtractItem(t);

                        File.WriteAllBytes(saveFilesDialog.FileName, chunk);
                        MessageBox.Show(String.Format("Decompress to :{0}", saveFilesDialog.FileName));
                    }

                }
            }

        }
        private void dgitem2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Currently not supported");
        }
        private byte[] ExtractItem(PackagedFile entries)
        {
            BinaryReader oldFile = new BinaryReader(File.OpenRead(criPak.FilePath));
            oldFile.BaseStream.Seek((long)entries.FileOffset, SeekOrigin.Begin);

            string isComp = Encoding.ASCII.GetString(oldFile.ReadBytes(8));
            oldFile.BaseStream.Seek((long)entries.FileOffset, SeekOrigin.Begin);

            byte[] chunk = oldFile.ReadBytes(Int32.Parse(entries.FileSize.ToString()));

            if (isComp == "CRILAYLA")
            {
                int size;
                if (entries.ExtractedFileSize == 0)
                {
                    size = entries.CompressedFileSize;
                }
                else
                {
                    size = entries.ExtractedFileSize;
                }

                if (size != 0)
                {
                    chunk = chunk.DecompressLegacyCRI();
                }
            }
            oldFile.Close();
            return chunk;
        }
        private void menu_makeCSV_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("currently not supported");
        }
        private void comboBox_encodings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cur = comboBox_encodings.SelectedIndex;
            Encoding current_codepage;
            switch (cur)
            {
                case 0:
                    current_codepage = Encoding.GetEncoding(65001);
                    break;
                case 1:
                    current_codepage = Encoding.GetEncoding(932);
                    break;
                default:
                    current_codepage = Encoding.GetEncoding(65001);
                    break;

            }
            //if (current_codepage != package.encoding)
            //{
            //    myPackage.encoding = current_codepage;
            //    if (myPackage.fileName != null)
            //    {

            //        beginLoadCPK(myPackage.fileName);
            //    }

            //}



        }
    }
}
