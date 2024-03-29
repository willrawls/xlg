﻿using MetX.Standard.Primary.Interfaces;

namespace MetX.Standard.Primary.Scripts
{
    public static class QuickScriptProcessorFactory
    {
        public static ActualizationResult ActualizeAndCompile(this ActualizationSettings settings)
        {
            var result = settings.QuickScriptTemplate.ActualizeCode(settings);
            if (result == null) 
                return null;

            if (!result.ActualizationSuccessful) return result;

            var compileResult = result.Compile();
            if (!compileResult) return result;

            result.Settings.UpdateBinPath();
            return result;
        }

        public static ActualizationSettings BuildSettings(this XlgQuickScript script, bool simulate, IGenerationHost host)
        {
            if (script == null) return null;

            var templateLoaded = host.Context.Templates.Contains(script.TemplateName);
            if (!templateLoaded || host.Context.Templates[script.TemplateName].Assets.Count == 0)
            {
                host.Context.LoadTemplates();
            }
            
            templateLoaded = host.Context.Templates.Contains(script.TemplateName);
            if (!templateLoaded)
            {
                host.MessageBox.Show($"ERROR: Unable to find template: {script.TemplateName}");
            }

            var template = host.Context.Templates[script.TemplateName];
            var settings = new ActualizationSettings(template, simulate, script, true, host);
            return settings;
        }

    }
}