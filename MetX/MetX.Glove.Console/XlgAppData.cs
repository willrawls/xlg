using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using Microsoft.Win32;

namespace XLG.Pipeliner
{
    [Serializable]
    public class XlgAppData
    {
        private static RegistryKey _mAppDataRegKey;
        [XmlAttribute] public string BasePath;
        [XmlAttribute] public string LastXlgsFile;
        [XmlAttribute] public string ParentNamespace;
        [XmlAttribute] public string SupportPath;
        [XmlAttribute] public string TextEditor;
        // ReSharper disable once EmptyConstructor
        public string ToXml() { return Xml.ToXml(this); }
        public static XlgAppData FromXml(string xmlDoc) { return Xml.FromXml<XlgAppData>(xmlDoc); }

        public static XlgAppData ResetPreferences()
        {
            var drive = Environment.CurrentDirectory.Substring(0, 1);

            var ret = new XlgAppData
            {
                BasePath = drive + @":\data\code\xlg\DAL\",
                SupportPath = drive + @":\data\code\xlg\Support\",
                ParentNamespace = "xlg.dal",
            };
            ret.LastXlgsFile = ret.SupportPath + "Default.xlgs";
            ret.TextEditor = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Notepad.exe");
            ret.Save();
            return ret;
        }

        public string CheckIfTextEditorExistsAndAskIfNot(IGenerationHost host)
        {
            if (TextEditor.Contains("$SystemRoot"))
                TextEditor = TextEditor.Replace("$SystemRoot", "%SystemRoot");

            if (File.Exists(TextEditor)) return TextEditor;

            if (TextEditor.ToLower().Contains("notepad2"))
            {
                TextEditor = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Notepad.exe");
            }
            else if (File.Exists(@"c:\Program Files (x86)\Notepad++\notepad++.exe"))
            {
                TextEditor = @"c:\Program Files (x86)\Notepad++\notepad++.exe";
                return TextEditor;
            }

            if (File.Exists(TextEditor)) return TextEditor;

            var textEditorPath = TextEditor;
            if (host.InputBox("PATH TO TEXT EDITOR",
                "What is the full path to the text editor you would like to use?",
                ref textEditorPath) == MessageBoxResult.OK)
            {
                if(File.Exists(textEditorPath))
                    TextEditor = textEditorPath;
            }

            return TextEditor;
        }

        public static XlgAppData Load()
        {
            XlgAppData ret = null;
            _mAppDataRegKey = Application.UserAppDataRegistry;
            if (_mAppDataRegKey != null)
            {
                var valueNames = new List<string>(_mAppDataRegKey.GetValueNames());

                try
                {
                    if (valueNames.Count == 0 || !valueNames.Contains("Preferences"))
                    {
                        ret = ResetPreferences();
                    }
                    else
                    {
                        try
                        {
                            ret = FromXml(_mAppDataRegKey.GetValue("Preferences") as string);
                        }
                        catch
                        {
                            ret = ResetPreferences();
                        }
                    }
                }
                finally
                {
                    if (_mAppDataRegKey != null)
                    {
                        _mAppDataRegKey.Close();
                        _mAppDataRegKey = null;
                    }
                }
            }
            return ret;
        }

        public void Save()
        {
            var openedKey = false;
            if (_mAppDataRegKey == null)
            {
                _mAppDataRegKey = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (_mAppDataRegKey == null)
            {
                return;
            }
            _mAppDataRegKey.SetValue("Preferences", ToXml(), RegistryValueKind.String);

            if (!openedKey || _mAppDataRegKey == null)
            {
                return;
            }
            _mAppDataRegKey.Close();
            _mAppDataRegKey = null;
        }
    }
}