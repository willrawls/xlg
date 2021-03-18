using System;
using MetX.Standard.Library;

namespace MetX.Library
{
    public class RunResult
    {
        public bool InputMissing;
        public bool KeepGoing = true;
        public bool StartReturnedFalse = false;
        public Exception Error;
        public bool ProcessLineReturnedFalse;
        public bool FinishReturnedFalse;
        public BaseLineProcessor QuickScriptProcessor;
    }
}