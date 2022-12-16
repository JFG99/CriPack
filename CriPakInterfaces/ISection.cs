using CriPakInterfaces.Models;

namespace CriPakInterfaces
{
    public interface ISection 
    {
        int Id { get; set; }
        string Name { get; set; }   
        SectionMeta MetaData { get; set; }
        SectionHeader HeaderData { get; set; }
        IContent ContentData { get; set; }

    }
}
