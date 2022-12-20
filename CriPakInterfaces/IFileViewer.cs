using CriPakInterfaces.Models.Components.Enums;

namespace CriPakInterfaces
{
    public interface IFileViewer
    {
        ulong ArchiveLength { get; set; }
        uint ExtractedLength { get; set; }
        string FileName { get; set; }
        int Id { get; set; }
        long Offset { get; set; }
        float Percentage { get; }
        string Size { get; }
        ItemType Type { get; set; }
        string TypeString { get; }
    }
}