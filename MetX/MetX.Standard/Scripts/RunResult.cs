using System;

namespace MetX.Standard.Scripts
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