using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using MetX.Library;
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
        public XlgAppData() { }
        public string ToXml() { return Xml.ToXml(this, true); }
        public static XlgAppData FromXml(string xmlDoc) { return Xml.FromXml<XlgAppData>(xmlDoc); }

        public static XlgAppData ResetPreferences()
        {
            string drive = Environment.CurrentDirectory.Substring(0, 1);

            XlgAppData ret = new XlgAppData
            {
                BasePath = drive + @":\data\code\xlg\DAL\",
                SupportPath = drive + @":\data\code\xlg\Support\",
                ParentNamespace = "xlg.dal",
            };
            ret.LastXlgsFile = ret.SupportPath + "Default.xlgs";
            ret.TextEditor = ret.SupportPath + "Notepad2.exe";
            ret.Save();
            return ret;
        }

        public static XlgAppData Load()
        {
            XlgAppData ret = null;
            _mAppDataRegKey = Application.UserAppDataRegistry;
            if (_mAppDataRegKey != null)
            {
                List<string> valueNames = new List<string>(_mAppDataRegKey.GetValueNames());

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
            bool openedKey = false;
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