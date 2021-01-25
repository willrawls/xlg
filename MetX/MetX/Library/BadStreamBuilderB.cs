using System;
using System.IO;
using System.Text;
using System.Threading;

namespace MetX.Library
{
    /// <summary>
    /// Duck type (ish) interface similar to StringBuilder only write operations go directly to a stream.
    /// While several of the functions from StringBuilder are supported, some are not implemented
    /// due to the fact that said functions would be far less efficient than in memory operations.
    /// </summary>
    public class BadStreamBuilderB
    {
        /// <summary>
        /// Set this to true and a call to Finish() or during disposal will automatically close <see cref="Target"/>.
        /// </summary>
        public bool CloseOnFinish { get; set; } = true;

        /// <summary>
        /// The path and filename being written to. NULL if a stream was passed in.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The number of bytes written so far. Note: This may not be the same as the length of the file.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// The wrapper to targetStream
        /// </summary>
        public TextWriter Target { get; set; }

        /// <summary>
        /// The stream that is ultimately written to
        /// </summary>
        public Stream TargetStream { get; set; } 

        /// <summary>
        /// The StringBuilder being written to (for that mode)
        /// </summary>
        public StringBuilder TargetStringBuilder { get; set; }

        public int FinishCount { get; set; }

        public bool IsOpenAndReady => TargetStream == null || Target == null;

        /// <summary>
        /// Attaches a new file stream to filePath (overwriting file if there).
        /// </summary>
        /// <param name="filePath">Path to the file to write to. NOTE: When append is false, file will be overwritten if it exists.</param>
        /// <param name="appendToExistingFile">True to start writing at the end of the file.</param>
        /// <exception cref="ArgumentNullException">File path is required</exception>
        /// <exception cref="ArgumentException"><see cref="FilePath" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.-or- access specified Read and mode specified Create, CreateNew, Truncate, or Append. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="NotSupportedException"><see cref="FilePath" /> is in an invalid format. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="FilePath" /> specified a file that is read-only access is not Read.-or- <see cref="FilePath" /> specified a directory.-or- The caller does not have the required permission. -or-mode is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
        /// <exception cref="FileNotFoundException">The file specified in <see cref="FilePath" /> was not found. </exception>
        /// <exception cref="ArgumentOutOfRangeException">mode or access specified an invalid value. </exception>
        public BadStreamBuilderB(string filePath, bool appendToExistingFile = false)
        {
            SwitchTo(filePath, appendToExistingFile);
        }

        public BadStreamBuilderB()
        {
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
            while (--retries < 10 && (TargetStream == null || Target == null))
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
                    TargetStream = fileStream;

                    if (TargetStream == null)
                        return false;
                    Target = new StreamWriter(TargetStream);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                }
            }

            if (TargetStream == null || Target == null)
            {
                throw new Exception($"StreamBuilder.SwitchTo was unable to open {filePath}");
            }
            return true;
        }

        /// <summary>
        /// Attaches to an existing TextWriter (or StreamWriter)
        /// </summary>
        /// <param name="textWriter"></param>
        /// <param name="underlyingStream"></param>
        /// <exception cref="ArgumentException"><paramref name="textWriter" /> is null or not writable. </exception>
        public BadStreamBuilderB(TextWriter textWriter, Stream underlyingStream)
        {
            if (underlyingStream == null || !underlyingStream.CanWrite)
                throw new ArgumentException("You must supply a writable stream", "underlyingStream");

            Target = textWriter ?? throw new ArgumentException("TextWriter is required", "textWriter");
            TargetStream = underlyingStream;
        }

        /// <summary>
        /// Attaches to an existing TextWriter (or StreamWriter)
        /// </summary>
        /// <param name="textWriter"></param>
        /// <param name="underlyingStream"></param>
        public BadStreamBuilderB(StringBuilder targetStringBuilder)
        {
            if (targetStringBuilder == null)
                throw new ArgumentException("StringBuilder is required", "targetStringBuilder");

            TargetStringBuilder = targetStringBuilder;
            Target = new StringWriter(targetStringBuilder);
        }

        public void Finish(bool leaveOpen = false)
        {
            FinishCount++;
            try
            {
                Target?.Flush();
                if (leaveOpen || !CloseOnFinish) return;
                Target?.Close();
            }
            catch (ObjectDisposedException odex)
            {
                TargetStream?.Close();
            }
            finally
            {
                Target?.Dispose();
                Target = null;
                TargetStream?.Dispose();
                TargetStream = null;
                TargetStringBuilder = null;
            }
        }

/*
        /// <summary>
        /// Attaches to an existing Stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentException"><paramref name="stream" /> is null or not writable. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is null. </exception>
        public StreamBuilder(Stream stream)
        {
            if ((stream == null) || !stream.CanWrite)
                throw new ArgumentException("You must supply a writable stream", "stream");
            Target = new StreamWriter(stream);
            TargetStream = stream;
        }
#1#
        ~StreamBuilderB()
        {
            Finish(false);
        }

        /// <summary>
        /// Appends a string to the stream
        /// </summary>
        /// <param name="value">The string to append to the stream</param>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
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

        /// <summary>
        /// Appends a string to the stream
        /// </summary>
        /// <param name="format">The format string to append to the stream (after being resolved)</param>
        /// <param name="args">Format args needed by <paramref name="format"/></param>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="format" /> or <paramref name="args" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="format" /> is invalid.-or- The index of a format item is less than zero, or greater than or equal to the length of the <paramref name="args" /> array. </exception>
        public void AppendFormat(string format, params object[] args)
        {
            if (format.IsEmpty())
            {
                return;
            }

            var value = string.Format(format, args);
            Target.Write(value);
            Length += value.Length;
            Target.Flush();
        }

        /// <summary>
        /// Appends a string to the stream followed by a new line.
        /// </summary>
        /// <param name="value">The string to append to the stream</param>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void AppendLine(string value)
        {
            if (value.IsNotEmpty() && Target != null)
            {
                Target?.Write(value);
                Length += value.Length;
            }

            Target?.Write(Environment.NewLine);
            Length += Environment.NewLine.Length;
            Target?.Flush();
        }

        /// <summary>
        /// Appends a new line to the stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void AppendLine()
        {
            Target.Write(Environment.NewLine);
            Length += Environment.NewLine.Length;
            Target.Flush();
        }

        /// <summary>
        /// Flushes and closes the stream.
        /// NOTE: Occurs automatically when the object is disposed.
        /// </summary>
        public void Finish(bool leaveOpen = false)
        {
            FinishCount++;
            try
            {
                Target?.Flush();
                if (leaveOpen || !CloseOnFinish) return;
                Target?.Close();
            }
            catch (ObjectDisposedException odex)
            {
                TargetStream?.Close();
            }
            finally
            {
                Target?.Dispose();
                Target = null;
                TargetStream?.Dispose();
                TargetStream = null;
                TargetStringBuilder = null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="returnToOriginalPosition">True to restore the seek location after writing.</param>
        /// <exception cref="InvalidOperationException">TargetStream must not be null</exception>
        /// <exception cref="ArgumentOutOfRangeException">index</exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public void Overwrite(int index, string value, int count = -1, bool returnToOriginalPosition = false)
        {
            if ((TargetStream == null) || !TargetStream.CanSeek)
                throw new InvalidOperationException("TargetStream must not be null and seekable.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "Can't seek to a negative index");
            if (value == null)
                return;
            if (count == 0)
                return;

            var position = TargetStream.Position;
            TargetStream.Seek(index, SeekOrigin.Begin);

            if ((count == -1) || (count >= value.Length))
            {
                Append(value);
            }
            else
            {
                Append(value.Left(count));
            }

            if (returnToOriginalPosition)
            {
                TargetStream.Seek(position, SeekOrigin.Begin);
            }
        }

        /// <exception cref="IOException">An I/O error has occurred. </exception>
        /// <exception cref="ArgumentException"><see cref="FilePath" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
        /// <exception cref="ArgumentNullException"><see cref="FilePath" /> is null. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="NotSupportedException"><see cref="FilePath" /> is in an invalid format. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="FilePath" /> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <see cref="FilePath" /> specified a directory.-or- The caller does not have the required permission. </exception>
        /// <exception cref="FileNotFoundException">The file specified in <see cref="FilePath" /> was not found. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        public override string ToString()
        {
            try
            {
                if (Target != null)
                {
                    Target.Flush();
                    Target.Close();
                }

                TargetStream?.Close();

                if (FilePath.IsEmpty())
                {
                    if (TargetStringBuilder == null)
                        return string.Empty;
                    return TargetStringBuilder.ToString();
                }

                return File.ReadAllText(FilePath);
            }
            finally
            {
                Target?.Dispose();
                TargetStream?.Dispose();
            }
        }
    */
    }
}