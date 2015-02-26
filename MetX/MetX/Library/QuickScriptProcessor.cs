using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX;
using MetX.Library;

namespace MetX
{
    public class QuickScriptProcessor : BaseLineProcessor
    {
//~~ClassMembers~~//

        public override void Start()
        {
//~~Start~~//
        }

        public override bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;
//~~ProcessLine~~//
            return true;
        }

        public override void Finish()
        {
//~~Finish~~//
        }

    }
}