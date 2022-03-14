﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        string DisplayName { get; set; }
        string ValidationName { get; set; }
        string SelectionName { get; set; }
        IPacket Packet { get; set; }
        long PacketLength {get;set;}
    }
}