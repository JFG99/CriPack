using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System.Collections.Generic;

namespace CriPakInterfaces
{
    public class Section : ISection
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public SectionMeta MetaData { get; set; }
        public SectionHeader HeaderData { get; set; }
        public IContent ContentData { get; set; }
    }
}
