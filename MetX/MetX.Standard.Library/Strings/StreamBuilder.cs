using System;
using System.IO;
using System.Text;
using System.Threading;
using MetX.Standard.Library.Extensions;

// ReSharper disable UnusedMember.Global

namespace MetX.Standard.Library.Strings
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

        public bool IsOpenAndReady
        {
            get
            {
                if (TargetStringBuilder != null)
                    return true;
                
                if (TargetStream != null)
                {
                    if (!TargetStream.CanWrite || TargetStreamWriter != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public StreamBuilder(string filePath, bool appendToExistingFile = false)
        {
            SwitchTo(filePath, appendToExistingFile);
        }

        public StreamBuilder()
        {
        }

        public StreamBuilder(StringBuilder targetStringBuilder)
        {
            TargetStringBuilder = targetStringBuilder ?? throw new ArgumentException("StringBuilder is required", nameof(targetStringBuilder));
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
            Finish();

            while (--retries < 10 && (TargetStream == null || TargetStreamWriter == null))
            {
                Finish();
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
                catch
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
            Finish();
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
            catch (ObjectDisposedException)
            {
                TargetStream?.Close();
            }
            finally
            {
                try { TargetStreamWriter?.Dispose(); } catch { /* Ignored */ }
                try { TargetStream?.Dispose(); } catch { /* Ignored */ }

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