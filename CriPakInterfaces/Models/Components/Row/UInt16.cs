using CriPakInterfaces.IComponents;
using System.ComponentModel;

namespace CriPakInterfaces.Models.Components
{
    public class UInt16 : Value<ushort>, IUint16
    {
        public UInt16(ushort value) : base(value) { }

        [DefaultValue(0xFFFF)]
        public ushort Value => GetValue();
    }
}
