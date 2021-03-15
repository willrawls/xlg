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

//~~Usings~~//

namespace MetX.Scripts
{
    public class QuickScriptProcessor : BaseLineProcessor
    {
//~~ClassMembers~~//

        public override bool? ReadInput(string inputType)
        {
//~~ReadInput~~//
            return base.ReadInput(inputType);
        }

        public override bool Start()
        {
//~~Start~~//
            return true;
        }

        public override bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;
//~~ProcessLine~~//
            return true;    // true = keep going
        }

        public override bool Finish()
        {
//~~Finish~~//
            Output.Finish();
            return true;
        }
    }
}