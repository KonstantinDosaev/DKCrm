using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Constants
{
    public static class PathsToDirectories
    {
        public const string Documents = @"Documents";
        public static readonly string ProductsDocuments = Path.Combine("Documents","Products");
        public static readonly string ProductsImages = Path.Combine("Images","Products");
        public static readonly string OrdersImages = Path.Combine("Images","Orders");
        public static readonly string OrdersDocuments = Path.Combine("Documents","Orders");
        public const string Fonts = @"Fonts";
        public const string Stamps = @"Stamps";
        public const string Preview = @"Preview";
    }
}
