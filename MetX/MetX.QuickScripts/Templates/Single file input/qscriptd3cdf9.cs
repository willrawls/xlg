using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Windows;
using MetX.Windows.Library;
using MetX.Standard;
using MetX.Standard.IO;
using MetX.Standard.Data;
using MetX.Standard.Scripts;
using MetX.Standard.Library;


namespace MetX.Scripts
{
    public class QuickScriptProcessor : BaseLineProcessor
    {
        public override bool? ReadInput(string inputType)
        {
            return base.ReadInput(inputType);
        }

        public override bool Start()
        {
            Output.AppendLine("Top of output file");
            return true;
        }

        public override bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;
            Output.AppendLine("Processing line " + number);
            Output.AppendLine("Switching to I:\\OneDrive\\data\\HenryTest" + number + ".txt");
            Output.SwitchTo(@"I:\OneDrive\data\HenryTest" + number + ".txt");
            Output.AppendLine(" # " + number + " = " + line);
            return true; // true = keep going
        }

        public override bool Finish()
        {
            Output.AppendTo(@"I:\OneDrive\data\HenryTestA.txt");
            Output.AppendLine("Next to last line in output path");
            Output.AppendLine("Switching to I:\\OneDrive\\data\\HenryTestB.txt");
            Output.SwitchTo(@"I:\OneDrive\data\HenryTestB.txt");
            Output.AppendLine();
            Output.AppendLine(" Ding!");
            Output.AppendLine("Switching to I:\\OneDrive\\data\\HenryTestB.txt");
            Output.AppendTo(@"I:\OneDrive\data\HenryTestB.txt");
            Output.AppendLine();
            Output.AppendLine(" Dong!");
            Output.AppendLine("Switching back to original output path");
            Output.AppendTo(@"I:\OneDrive\data\HenryTestA.txt");
            Output.AppendLine(" Bottom of output file");
            Output.Finish();
            return true;
        }
    }
}