using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MetX;
using MetX.Library;

namespace MetX
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
            return true;
        }

        public override bool Finish()
        {
//~~Finish~~//
            Output.Finish();
            return true;
        }
    }
}