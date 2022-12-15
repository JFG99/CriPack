using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CriPakInterfaces
{
    public interface ISection
    {
        int Id { get; set; }
        string Name { get; set; }   
        IMeta MetaData { get; set; }
        IHeader HeaderData { get; set; }
        IContent ContentData { get; set; }

    }
}
