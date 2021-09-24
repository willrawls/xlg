using System;
using System.Collections.Generic;
using System.IO;
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
                    return result;
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("-----[ Output Folder ]-----");
            sb.AppendLine($"{result.Settings.OutputFolder}");

            sb.AppendLine();
            sb.AppendLine("-----[ Compilation failure ]-----");
            sb.AppendLine();
            sb.AppendLine(result.CompileErrorText);
            sb.AppendLine();
            sb.AppendLine("-----[ Output from dotnet.exe ]-----");
            sb.AppendLine();
            sb.AppendLine(result.OutputText);
            sb.AppendLine();

            var answer = settings.Host.MessageBox.Show(sb.ToString(), "OPEN OUTPUT FOLDER?", MessageBoxChoices.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                QuickScriptWorker.ViewFolder(result.Settings.OutputFolder, settings.Host);
            }
            return result;
        }

        public static ActualizationSettings ActualizationSettingsFactory(XlgQuickScript script, bool forExecutable, bool simulate, IGenerationHost host)
        {
            if (script == null)
                return null;

            var templateName = forExecutable
                ? script.ExeTemplateName
                : script.NativeTemplateName;

            var template = host.Context.Templates[templateName];
            var settings = new ActualizationSettings(template, simulate, script);
            return settings;
        }

    }
}