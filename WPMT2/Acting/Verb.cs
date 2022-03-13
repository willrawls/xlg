namespace WilliamPersonalMultiTool.Acting
{
    public class Verb
    {
        public Verb Excludes;
        public int Max = 1;
        public string Name;
        public bool Mentioned { get; set; }

        public static Verb Factory(string name, int max = 1, Verb modifies = null)
        {
            return new Verb
            {
                Name = name,
                Max = max,
                Excludes = modifies
            };
        }

        public static Verb Factory(string name, Verb modifies, int max = 1)
        {
            var verb = new Verb
            {
                Name = name,
                Max = max,
                Excludes = modifies
            };
            if (modifies != null)
                modifies.Excludes = verb;
            return verb;
        }
    }
}