﻿using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class ContentHeader : Header, IContentHeader
    {
        public ContentHeader()
        {
            DisplayName = "CONTENT_OFFSET";
            SelectionName = "Content";
            ValidationName = "";
        }        
    }                                          
}
