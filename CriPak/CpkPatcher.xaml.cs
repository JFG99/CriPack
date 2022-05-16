using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
using System.Threading;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakInterfaces.Models;
using System.Threading.Tasks;
using System.Text;

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
                    Dispatcher.Invoke(() => PatchCPK())
                );
                return;
            }
            MessageBox.Show("Error, cpkdata or patchdata not found.");
        }

        private void PatchCPK()
        {
            _home.Patch(package, textbox_patchDir.Text, textbox_cpkDir.Text, checkbox_donotcompress.IsChecked ?? false);
           
            
            var oldFile = new BinaryReader(File.OpenRead(package.FilePath));
            //var newCPK = new BinaryWriter(File.OpenWrite(cpkDir));
            //var entries = package.CriFileList.OrderBy(x => x.FileOffset).ToList();
            //var badEntries = package.CriFileList.Where(x => x.FileOffsetType == null);
            //var i = 0; 
            //var msg = "";
            //foreach (var entry in entries)
            //{                
            //    Dispatcher.Invoke(() => progressbar1.Value = i++ / (float)package.CriFileList.Count * 100f);

            //    if (entry.FileType != "CONTENT")
            //    {
            //        if (entry.FileType == "FILE")
            //        {
            //            // *****I'm too lazy to figure out how to update the ContentOffset position so this works :)
            //            // No it doesnt.
            //            if ((ulong)newCPK.BaseStream.Position < package.ContentOffset)
            //            {
            //                ulong padLength = package.ContentOffset - (ulong)newCPK.BaseStream.Position;
            //                for (ulong z = 0; z < padLength; z++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }                   
            //        Debug.WriteLine($"Got File: {entry.FileName}");

            //        //If Not in dictionary, copy original data
            //        if (!fileList.ContainsKey(entry.FileName))
            //        {
            //            oldFile.BaseStream.Seek((long)entry.FileOffset, SeekOrigin.Begin);

            //            entry.FileOffset = (ulong)newCPK.BaseStream.Position;

            //            if (entry.FileName.ToString() == "ETOC_HDR")
            //            {
            //                package.EtocOffset = entry.FileOffset;
            //                Debug.WriteLine($"Fix ETOC_OFFSET to {package.EtocOffset}");
            //            }

            //            package.UpdateCriFile(entry);
            //            byte[] chunk = oldFile.ReadBytes(entry.CompressedFileSize);
            //            newCPK.Write(chunk);

            //            if ((newCPK.BaseStream.Position % 0x800) > 0 && i <= package.CriFileList.Count)
            //            {
            //                long cur_pos = newCPK.BaseStream.Position;
            //                for (int j = 0; j < (0x800 - (cur_pos % 0x800)); j++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //Got patch file name
            //            msg = $"Patching: {entry.FileName}";
            //            Dispatcher.Invoke(() => updateTextblock(msg));
                        

            //            var newFileBytes = File.ReadAllBytes(fileList[entry.FileName]);
            //            entry.FileOffset = (ulong)newCPK.BaseStream.Position;
            //            msg = $"Storing data: {newFileBytes.Length}\r\n";
            //            var originalSize = 0;
            //            if (isForceCompress && (entry.CompressedFileSize < entry.ExtractedFileSize) && entry.FileType == "FILE")
            //            {
            //                msg = $"Compressing and storing data:{newFileBytes.Length}\r\n";
            //                originalSize = newFileBytes.Length;
            //                newFileBytes = newFileBytes.CompressCRILAYLA();                          
            //            }
            //            WriteChunkData(newCPK, entry, newFileBytes, originalSize, msg);

            //            if ((newCPK.BaseStream.Position % 0x800) > 0 && i < entries.Count - 1)
            //            {
            //                long cur_pos = newCPK.BaseStream.Position;
            //                for (int j = 0; j < (0x800 - (cur_pos % 0x800)); j++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // Content is special.... just update the position
            //        package.UpdateCriFile(entry);
            //    }
            //}

            //WriteCPK(newCPK);
            //Dispatcher.Invoke(() => updateTextblock("Writing TOC...."));

            //WriteITOC(newCPK);
            //WriteTOC(newCPK);
            //WriteETOC(newCPK, package.EtocOffset);
            //WriteGTOC(newCPK);

            //newCPK.Close();
            //oldFile.Close();
            //Dispatcher.Invoke(() => updateTextblock($"Saving CPK to {cpkDir}...."));
            //MessageBox.Show("CPK Patched.");
            //Dispatcher.Invoke(() => progressbar1.Value = 0f);
        }

        private void WriteChunkData(BinaryWriter cpk, CriFile entry, byte[] chunkData, int originalDataLength, string msg)
        {
            Dispatcher.Invoke(() => updateTextblock(msg));
            Debug.WriteLine(msg);
            entry.CompressedFileSize = chunkData.Length;
            entry.ExtractedFileSize = originalDataLength == 0 ? chunkData.Length : originalDataLength;
           // package.UpdateCriFile(entry);
            cpk.Write(chunkData);
            Dispatcher.Invoke(() => updateTextblock(msg));
            Debug.WriteLine(msg);

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
