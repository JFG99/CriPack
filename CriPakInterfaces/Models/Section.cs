using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CriPakInterfaces
{
    public class Section : ISection
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public IMeta MetaData { get; set; }
        public IHeader HeaderData { get; set; }
        public IContent ContentData { get; set; }

    }
}
