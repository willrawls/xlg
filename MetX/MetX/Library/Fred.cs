namespace MetX.Library
{
    public class Fred
    {
        public void X()
        {
            string sample = "Fred went home.";
            string[] example = sample.Split(' ');
            // example[0] == "Fred"
            // example[1] == "went"
            // example[2] == "home."
            example = new string[1];
        }
    }
}