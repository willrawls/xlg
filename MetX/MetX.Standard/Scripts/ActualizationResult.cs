using System;
using System.Collections.Generic;
using System.IO;
using MetX.Standard.Library;

namespace MetX.Standard.Scripts
{
    public class ActualizationResult
    {
        public ActualizationSettings Settings { get; set; }

        public string ErrorText { get; set; }
        public AssocArray OutputFiles { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public ActualizationResult(ActualizationSettings settings)
        {
            Settings = settings;
        }
    }
}