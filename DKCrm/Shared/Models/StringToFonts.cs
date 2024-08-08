using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class StringToFonts
    {
        public StringToFonts(FontTypes fontType, string value)
        {
            FontType = fontType;
            Value = value;
        }

        public StringToFonts()
        {
        }

        public FontTypes FontType { get; set; }
        public string Value { get; set; } = null!;
    }
}
