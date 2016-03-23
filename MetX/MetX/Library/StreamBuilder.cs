using System;
using System.IO;

namespace MetX.Library
{
    public class StreamBuilder
    {
        public long Length { get; private set; }
        public StreamWriter Target { get; private set; }
        public string FilePath { get; private set; }

        public StreamBuilder(string filePath)
        {
            if (filePath.IsEmpty())
            {
                throw new ArgumentNullException("filePath");
            }
            FilePath = filePath;
            Target = new StreamWriter(File.Open(FilePath, FileMode.Create, FileAccess.Write));
        }

        ~StreamBuilder()
        {
            Finish();
        }

        public void Append(string value)
        {
            if (value.IsEmpty())
            {
                return;
            }
            Target.Write(value);
            Length += value.Length;
            Target.Flush();
        }

        public void AppendLine(string value)
        {
            if (value.IsNotEmpty())
            {
                Target.Write(value);
                Length += value.Length;
            }
            Target.Write(Environment.NewLine);
            Length += Environment.NewLine.Length;
            Target.Flush();
        }

        public void Finish()
        {
            try
            {
                if (Target == null)
                {
                    return;
                }

                Target.Flush();
                Target.Close();
            }
            finally
            {
                Target = null;
            }
        }

        public override string ToString()
        {
            Finish();
            return File.ReadAllText(FilePath);
        }
    }
}