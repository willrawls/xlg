using System;
using System.IO;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project.Pieces
{
    public static class Piece
    {
        public const string Missing = "Missing";
        public const string LangVersion = @"LangVersion";
        public const string PiecesDirectory = @"Standard.Primary\Generation\CSharp\Project\Pieces\";

        public static ClientCsProjGenerator Get(string pieceName, string area)
        {
            if (area.IsNotEmpty() && !area.EndsWith(@"\"))
                area += @"\";
            
            var filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}{PiecesDirectory}{area}{pieceName}.xml";
            Assert.IsTrue(File.Exists(filePath), $"Can't find: {filePath}");
            return new ClientCsProjGenerator(filePath);
        }

        public static ClientCsProjGenerator GetEmptyClient()
        {
            return Get("EmptyClient", null);
            
        }

        public static ClientCsProjGenerator GetFullClient()
        {
            return Get("FullClient", null);
            
        }

        public const string GenerateToPath = @"GenerateToPath";
        public const string Emit = @"Emit";
    }
}