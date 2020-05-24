using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
// ReSharper disable UnusedMember.Global

namespace MetX.Library
{
    public class StreamBuilder
    {
        public bool CloseOnFinish { get; set; } = true;
        public string FilePath { get; set; }
        public long Length { get; set; }
        public TextWriter TargetStreamWriter { get; set; }
        public Stream TargetStream { get; set; } 
        public StringBuilder TargetStringBuilder { get; set; }
        public int FinishCount { get; set; }

        public bool IsOpenAndReady => (TargetStream != null || !TargetStream.CanWrite) || (TargetStreamWriter != null);

        public StreamBuilder(string filePath, bool appendToExistingFile = false)
        {
            SwitchTo(filePath, appendToExistingFile);
        }

        public StreamBuilder()
        {
        }

        public StreamBuilder(StringBuilder targetStringBuilder)
        {
            TargetStringBuilder = targetStringBuilder ?? throw new ArgumentException("StringBuilder is required", "targetStringBuilder");
            TargetStreamWriter = new StringWriter(targetStringBuilder);
        }

        public bool AppendTo(string filePath)
        {
            return SwitchTo(filePath, true);
        }

        public bool SwitchTo(string filePath, bool append = false)
        {
            if (filePath.IsEmpty())
                throw new ArgumentNullException(nameof(filePath));

            filePath = filePath.Replace(@"/", @"\");
            var folderPath = filePath.TokensBeforeLast(@"\");

            if (!folderPath.IsEmpty() && !Directory.Exists($"{folderPath}"))
                Directory.CreateDirectory(folderPath);

            FilePath = filePath;
            var retries = 10;
            Finish(false);
            while (--retries < 10 && (TargetStream == null || TargetStreamWriter == null))
            {
                Finish(false);
                try
                {
                    if (!append && File.Exists(FilePath))
                    {
                        File.SetAttributes(FilePath, FileAttributes.Normal);
                        File.Delete(FilePath);
                    }
                    
                    var fileStream = new FileStream(FilePath, append ? FileMode.Append : FileMode.Create);
                    if (!fileStream.CanWrite)
                        return false;

                    TargetStream = fileStream;

                    if (TargetStream == null || !TargetStream.CanWrite)
                        return false;
                    TargetStreamWriter = new StreamWriter(TargetStream);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                }
            }

            if (TargetStream == null || TargetStreamWriter == null)
            {
                throw new Exception($"StreamBuilder.SwitchTo was unable to open {filePath}");
            }
            return true;
        }

        ~StreamBuilder()
        {
            Finish(false);
        }

        public void Finish(bool leaveOpen = false)
        {
            FinishCount++;
            try
            {
                TargetStreamWriter?.Flush();
                if (leaveOpen || !CloseOnFinish) return;
                TargetStreamWriter?.Close();
            }
            catch (ObjectDisposedException odex)
            {
                TargetStream?.Close();
            }
            finally
            {
                try { TargetStreamWriter?.Dispose(); } catch { }
                try { TargetStream?.Dispose(); } catch { }

                TargetStream = null;
                TargetStreamWriter = null;
                TargetStringBuilder = null;
            }
        }

        public void AppendLine()
        {
            TargetStreamWriter.Write(Environment.NewLine);
            Length += Environment.NewLine.Length;
            TargetStreamWriter.Flush();
        }
        public void Append(string value)
        {
            if (value.IsEmpty())
            {
                return;
            }

            TargetStreamWriter.Write(value);
            Length += value.Length;
            TargetStreamWriter.Flush();
        }

        /// <summary>
        /// Appends a string to the stream followed by a new line.
        /// </summary>
        /// <param name="value">The string to append to the stream</param>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void AppendLine(string value)
        {
            if (value.IsNotEmpty() && TargetStreamWriter != null)
            {
                TargetStreamWriter?.Write(value);
                Length += value.Length;
            }

            TargetStreamWriter?.Write(Environment.NewLine);
            Length += Environment.NewLine.Length;
            TargetStreamWriter?.Flush();
        }
    }
}