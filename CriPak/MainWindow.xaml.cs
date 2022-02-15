using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using Ookii.Dialogs.Wpf;
using System.Threading;
using System.Windows.Threading;
using CriPakInterfaces.Models.Components;

namespace CriPakComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public myPackage package { get; set; }

        public MainWindow()
        {
            package = new myPackage();
            InitializeComponent();
            SetBasicPrefs();
        }

        private void SetBasicPrefs()
        {
            menu_savefiles.IsEnabled = false;
            menu_importAssets.IsEnabled = false;
            progressbar0.Maximum = 100;
            package.basePath = @"C:/";
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

        private void beginLoadCPK(string fName)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    new Action(() =>
                    {
                        cpkwrapper cpk = new cpkwrapper(fName);
                        status_cpkmsg.Content = string.Format("{0} file(s) registered.", cpk.nums);
                        datagrid_cpk.ItemsSource = cpk.tablePkgFiles;

                        menu_importAssets.IsEnabled = true;
                        menu_savefiles.IsEnabled = true;
                        package.basePath = System.IO.Path.GetDirectoryName(fName);
                        package.baseName = System.IO.Path.GetFileName(fName);
                        package.fileName = fName;
                    })
               );
            });
        }


        private void menu_importAssets_Click(object sender, RoutedEventArgs e)
        {
            //CpkPatcher patcherWindow = new CpkPatcher(this.Top, this.Left);
            //patcherWindow.ShowDialog();
        }


        private delegate void progressbarDelegate(float no);

        private delegate void datagridDelegate(bool value);

        private void updateDatagrid(bool value)
        {
            datagrid_cpk.IsEnabled = value;
            button_extract.IsEnabled = value;
            button_importassets.IsEnabled = value;
        }

        private void updateprogressbar(float no)
        {
            progressbar0.Value = no;
        }

        private void menu_savefiles_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog saveFilesDialog = new VistaFolderBrowserDialog();
            saveFilesDialog.SelectedPath = package.basePath;
            if (saveFilesDialog.ShowDialog().Value)
            {
                Debug.Print(saveFilesDialog.SelectedPath + "/" + package.baseName + "_unpacked");
                ThreadPool.QueueUserWorkItem(new WaitCallback(beginExtractCPK), saveFilesDialog.SelectedPath);

            }

        }

        private void beginExtractCPK(object foutDir)
        {
            string outDir;
            outDir = (string)(foutDir + "/" + package.baseName + "_unpacked");
            if (package.cpk != null)
            {
                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }
                BinaryReader oldFile = new BinaryReader(File.OpenRead(myPackage.cpk_name));
                List<CriFile> entries = null;

                //    entries = myPackage.cpk.FileTable.Where(x => x.FileType == "FILE").ToList();

                //    if (entries.Count == 0)
                //    {
                //        Debug.Print("err while extracting.");
                //        oldFile.Close();
                //        return;
                //    }

                //    int i = 0;
                //    int id;
                //    string currentName;
                //    bool bFileRepeated = entries.CheckListRedundant();
                //    this.Dispatcher.Invoke(new datagridDelegate(updateDatagrid), new object[] { (bool)false });

                //    while (i < entries.Count)
                //    {
                //        this.Dispatcher.Invoke(new progressbarDelegate(updateprogressbar), new object[] { (float)i / (float)entries.Count * 100f });//异步委托

                //        if (!String.IsNullOrEmpty((string)entries[i].DirName))
                //        {
                //            Directory.CreateDirectory(outDir + "/" + entries[i].DirName.ToString());
                //        }

                //        id = Convert.ToInt32(entries[i].ID);
                //        if (id > 0 && bFileRepeated)
                //        {
                //            currentName = (((entries[i].DirName != null) ?
                //                            entries[i].DirName + "/" : "") + string.Format("[{0}]", id.ToString()) + entries[i].FileName);
                //            currentName = currentName.TrimStart('/');
                //        }
                //        else
                //        {
                //            currentName = ((entries[i].DirName != null) ? entries[i].DirName + "/" : "") + entries[i].FileName;
                //            currentName = currentName.TrimStart('/');
                //        }

                //        oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);

                //        string isComp = Encoding.ASCII.GetString(oldFile.ReadBytes(8));
                //        oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);

                //        byte[] chunk = oldFile.ReadBytes(Int32.Parse(entries[i].FileSize.ToString()));

                //        if (isComp == "CRILAYLA")
                //        {
                //            int size = Int32.Parse((entries[i].ExtractedFileSize?? entries[i].FileSize).ToString());

                //            if (size != 0)
                //            {
                //                chunk = myPackage.cpk.DecompressLegacyCRI(chunk, size);
                //            }
                //        }

                //        Debug.WriteLine(" FileName :{0}\n    FileOffset:{1:x8}    ExtractSize:{2:x8}   ChunkSize:{3:x8} {4}",
                //                                                    entries[i].FileName.ToString(),
                //                                                    (long)entries[i].FileOffset,
                //                                                    entries[i].ExtractSize,
                //                                                    entries[i].FileSize,
                //                                                    ((float)i / (float)entries.Count) * 100f);
                //        string dstpath = outDir + "/" + currentName;
                //        dstpath = dstpath.GetSafePath();
                //        string dstdir = System.IO.Path.GetDirectoryName(dstpath);
                //        if (!Directory.Exists(dstdir))
                //        {
                //            Directory.CreateDirectory(dstdir);
                //        }
                //        File.WriteAllBytes(dstpath, chunk);
                //        i += 1;
                //    }
                //    oldFile.Close();
                //    this.Dispatcher.Invoke(new progressbarDelegate(updateprogressbar), new object[] { 100f });
                //    this.Dispatcher.Invoke(new datagridDelegate(updateDatagrid), new object[] { (bool)true });
                //    MessageBox.Show("Extraction Complete.");

                //}

            }

        private void button_extract_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog saveFilesDialog = new VistaFolderBrowserDialog();
            saveFilesDialog.SelectedPath = package.basePath + "/";
            if (saveFilesDialog.ShowDialog().Value)
            {
                Debug.Print(saveFilesDialog.SelectedPath + "/" + package.baseName + "_unpacked");
                ThreadPool.QueueUserWorkItem(new WaitCallback(beginExtractCPK), saveFilesDialog.SelectedPath);

            }
        }

        private void button_importassets_Click(object sender, RoutedEventArgs e)
        {
            //CpkPatcher patcherWindow = new CpkPatcher(this.Top, this.Left);
            //patcherWindow.ShowDialog();
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
                    saveFilesDialog.InitialDirectory = package.basePath;
                    saveFilesDialog.FileName = package.basePath + "/" + t.LocalName;
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
                    //chunk = myPackage.cpk.DecompressLegacyCRI(chunk, size);
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
