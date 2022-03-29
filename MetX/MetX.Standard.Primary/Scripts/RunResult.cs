namespace MetX.Standard.Primary.Scripts
{
    public class RunResult
    {
        public bool InputMissing = false;
        public bool KeepGoing = true;
        public bool StartReturnedFalse = false;
        public bool ProcessLineReturnedFalse;
        public bool FinishReturnedFalse;

        public ActualizationResult ActualizationResult { get; set; }
        public string GatheredOutput { get; set; }
        public string ErrorOutput { get; set; }
    }
}