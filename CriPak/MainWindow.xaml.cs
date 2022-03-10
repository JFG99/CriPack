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
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models;
using CriPakRepository.Parsers;
using CriPakRepository.Helpers;
using System.Threading.Tasks;
using CriPakRepository.Readers;
namespace CriPakComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CriPak package = new CriPak();
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
            menu_importAssets.IsEnabled = false;
            progressbar0.Maximum = 100;
            package.BasePath = @"C:/";
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
            package.CpkName = inFile;
            package.BasePath = Path.GetDirectoryName(inFile);
            package.BaseName = Path.GetFileName(inFile);
            package.Encoding = Encoding.GetEncoding(65001);
            _ = Task.Run(() => 
                Dispatcher.Invoke(() => 
                {
                    var parser = new CpkParser();
                    _home.Read(inFile);
                    //_cpkReader.Read(package);
                    parser.Parse(package);
                    status_cpkmsg.Content = string.Format("{0} file(s) registered.", package.CriFileList.Count());
                    datagrid_cpk.ItemsSource = package.DisplayList;
                    menu_importAssets.IsEnabled = true;
                    menu_savefiles.IsEnabled = true;
                })
            );
        }
        private void ImportAssets_Click(object sender, RoutedEventArgs e)
        {
            //CpkPatcher patcherWindow = new CpkPatcher(package, Top, Left);
            //patcherWindow.ShowDialog();
        }
        private void updateDatagrid(bool value = false)
        {
            datagrid_cpk.IsEnabled = value;
            button_extract.IsEnabled = value;
            button_importassets.IsEnabled = value;
        }
        private void beginExtractCPK(object foutDir)
        {
            if (package != null)
            {
                var outDir = $"{foutDir}/{package.BaseName}_unpacked";
                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }
                BinaryReader oldFile = new BinaryReader(File.OpenRead(package.CpkName));
                
                var entries = package.CriFileList.Where(x => x.FileType == "FILE").ToList();

                if (entries.Count == 0)
                {
                    Debug.Print("err while extracting.");
                    oldFile.Close();
                    return;
                }
                Dispatcher.Invoke(() => updateDatagrid()); 
                var i = 0;
                foreach (var entry in entries) 
                {                    
                    Dispatcher.Invoke(() => progressbar0.Value = i++ / (float)entries.Count * 100f );
                    oldFile.BaseStream.Seek((long)entry.FileOffset, SeekOrigin.Begin);
                    string isComp = Encoding.ASCII.GetString(oldFile.ReadBytes(8));
                    oldFile.BaseStream.Seek((long)entry.FileOffset, SeekOrigin.Begin);
                    byte[] chunk = oldFile.ReadBytes(entry.CompressedFileSize);
                    if (isComp == "CRILAYLA")
                    {
                        chunk = chunk.DecompressLegacyCRI();
                    }

                    if (!string.IsNullOrEmpty((string)entry.DirName))
                    {
                        Directory.CreateDirectory(outDir + "/" + entry.DirName.ToString());
                    }
                    var currentName = $"{((entry.DirName != null) ? entry.DirName + "/" : "")}{entry.FileName}".TrimStart('/');                    
                    Debug.WriteLine($" FileName :{entry.FileName}\n FileOffset:{entry.FileOffset}\n ExtractSize:{entry.ExtractedFileSize}\n ChunkSize:{entry.FileSize}\n {(i / (float)entries.Count) * 100f}");
                    string dstpath = outDir + "/" + currentName;
                    string dstdir = System.IO.Path.GetDirectoryName(dstpath);
                    if (!Directory.Exists(dstdir))
                    {
                        Directory.CreateDirectory(dstdir);
                    }

                    File.WriteAllBytes(dstpath, chunk);
                    i += 1;
                }
                oldFile.Close();
                Dispatcher.Invoke(() => progressbar0.Value = 100f);
                Dispatcher.Invoke(() => updateDatagrid(true));
                MessageBox.Show("Extraction Complete.");
            }
        }
        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog saveFilesDialog = new VistaFolderBrowserDialog();
            saveFilesDialog.SelectedPath = package.BasePath + "/";
            if (saveFilesDialog.ShowDialog().Value)
            {
                Debug.Print(saveFilesDialog.SelectedPath + "/" + package.BaseName + "_unpacked");
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
                    saveFilesDialog.InitialDirectory = package.BasePath;
                    saveFilesDialog.FileName = package.BasePath + "/" + t.LocalName;
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
            BinaryReader oldFile = new BinaryReader(File.OpenRead(package.CpkName));
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
