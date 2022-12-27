using System.Windows;
using System.IO;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
using CriPakInterfaces.Models;
using System.Threading.Tasks;

namespace CriPakComplete
{
    /// <summary>
    /// CpkPatcher.xaml 的交互逻辑
    ///// </summary>
    public partial class CpkPatcher : Window
    {
        public CriPak package { get; set; }
        private delegate void textblockDelegate(string text);
        private delegate void progressbarDelegate(float no);
        private readonly Orchestrator _home;

        public CpkPatcher(Orchestrator home, CriPak mainPackage, double x, double y)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = x;      
            Left = y;
            package = mainPackage;
            _home = home;
        }

        private void SelectPatchPath_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog saveFilesDialog = new VistaFolderBrowserDialog();
            saveFilesDialog.SelectedPath = package.BasePath + "/";
            if (saveFilesDialog.ShowDialog().Value)
            {
                Debug.Print(saveFilesDialog.SelectedPath);
                textbox_patchDir.Text = saveFilesDialog.SelectedPath;
            }
        }

        private void SelectNewPath_Click(object sender, RoutedEventArgs e)
        {
            VistaSaveFileDialog saveDialog = new VistaSaveFileDialog();
            saveDialog.InitialDirectory = package.BasePath;
            saveDialog.RestoreDirectory = true;
            saveDialog.Filter = "CPK File（*.cpk）|*.cpk";
            if (saveDialog.ShowDialog() == true)
            {
                string saveFileName = saveDialog.FileName;
                textbox_cpkDir.Text = saveFileName;
            }
        }

        private void updateTextblock(string text)
        {
            textblock0.Text += $"Updating ... {text}\n";
            Debug.WriteLine(text);
            scrollview0.ScrollToEnd();
        }

        private void PatchCPK_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textbox_patchDir.Text))
            {
                _ = Task.Run(() =>
                   Dispatcher.Invoke(() =>
                   {
                       _home.Patch(package, textbox_patchDir.Text, textbox_cpkDir.Text, checkbox_donotcompress.IsChecked ?? false);
                   })
                );
                return;
            }
            MessageBox.Show("Error, cpkdata or patchdata not found.");
        }

        private void WriteChunkData(BinaryWriter cpk,/* CriFile entry,*/ byte[] chunkData, int originalDataLength, string msg)
        {
           // Dispatcher.Invoke(() => updateTextblock(msg));
           // Debug.WriteLine(msg);
           // entry.CompressedFileSize = chunkData.Length;
           // entry.ExtractedFileSize = originalDataLength == 0 ? chunkData.Length : originalDataLength;
           //// package.UpdateCriFile(entry);
           // cpk.Write(chunkData);
           // Dispatcher.Invoke(() => updateTextblock(msg));
           // Debug.WriteLine(msg);

        }
        public void WriteCPK(BinaryWriter cpk)
        {
            //WritePacket(cpk, "CPK ", 0, package.CpkPacket);

            //cpk.BaseStream.Seek(0x800 - 6, SeekOrigin.Begin);
            //cpk.Write(Encoding.ASCII.GetBytes("(c)CRI"));
            //if ((package.TocOffset > 0x800) && package.TocOffset < 0x8000)
            //{
            //    //Part of cpk starts TOC from 0x2000, so
            //    //Need to calculate cpk padding
            //    //HUH?
            //    cpk.Write(new byte[package.TocOffset - 0x800]);
            //}
        }

        //public void WriteTOC(BinaryWriter cpk)
        //{
        //    WritePacket(cpk, "TOC ", package.TocOffset, package.TocPacket);
        //}

        //public void WriteITOC(BinaryWriter cpk)
        //{
        //    WritePacket(cpk, "ITOC", package.ItocOffset, package.ItocPacket);
        //}

        //public void WriteETOC(BinaryWriter cpk, ulong currentEtocOffset)
        //{
        //    WritePacket(cpk, "ETOC", currentEtocOffset, package.EtocPacket);
        //}

        //public void WriteGTOC(BinaryWriter cpk)
        //{
        //    WritePacket(cpk, "GTOC", package.GtocOffset, package.GtocPacket);
        //}

        //public void WritePacket(BinaryWriter cpk, string ID, ulong position, byte[] packet)
        //{
        //    if (position != 0xffffffffffffffff)
        //    {
        //        cpk.BaseStream.Seek((long)position, SeekOrigin.Begin);
        //        byte[] encrypted;
        //        if (package.IsUtfEncrypted == true)
        //        {
        //            encrypted = packet.DecryptUTF(); // Yes it says decrypt...
        //        }
        //        else
        //        {
        //            encrypted = packet;
        //        }

        //        cpk.Write(Encoding.ASCII.GetBytes(ID));
        //        cpk.Write((Int32)0xff);
        //        cpk.Write((UInt64)encrypted.Length);
        //        cpk.Write(encrypted);
        //    }
        //}

        public void UI_SetTextBlock(string msg)
        {
            Dispatcher.Invoke(new textblockDelegate(updateTextblock), new object[] { msg });
        }        
    }
}
