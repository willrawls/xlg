using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MetX.Standard;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;
using MetX.Standard.Scripts;
using MetX.Windows.Library;
using Microsoft.CSharp;

namespace MetX.Controls
{
    public static class QuickScriptProcessorFactory
    {
        public static ActualizationResult ActualizeAndCompile(this ActualizationSettings settings)
        {
            var result = settings.QuickScriptTemplate.ActualizeCode(settings);
            if (result == null) 
                return null;

            if (result.ActualizationSuccessful)
            {
                var compileResult = result.Compile();
                if (compileResult)
                {
                    result.Settings.UpdateBinPath();
                    return result;
                }
            }

            return result;
        }

        public static ActualizationSettings BuildSettings(this XlgQuickScript script, bool forExecutable, bool simulate, IGenerationHost host)
        {
            if (script == null)
                return null;

            var templateName = forExecutable
                ? script.ExeTemplateName
                : script.NativeTemplateName;

            var template = host.Context.Templates[templateName];
            var settings = new ActualizationSettings(template, simulate, script, forExecutable, host);
            return settings;
        }

    }
}