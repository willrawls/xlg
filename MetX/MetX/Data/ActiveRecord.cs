using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MetX.IO;

namespace MetX.Data
{
    /// <summary>
    /// Base class for persisting objects. Follows the "Active Record Design Pattern".
    /// You can read more on this pattern at http://en.wikipedia.org/wiki/Active_Record
    /// </summary>
	[Serializable]
	public abstract class ActiveRecord
    {
        #region State Properties
        [XmlIgnore] public bool _IsLoaded;
        [XmlIgnore] public bool _IsNew;
        [XmlIgnore] public string _TableName;
        [XmlIgnore] public object _Tag;
        [XmlIgnore] public int _RecordHashThen;
        #endregion
        #region default constructor
        public ActiveRecord() { _IsNew = true; }
        public ActiveRecord(IDataReader rdr) { Load(rdr); }
        public ActiveRecord(DataRow dr) { Load(dr); }
        #endregion

        public abstract void Load(IDataReader rdr);
        public abstract void Load(DataRow dr);
        public abstract string _ClassName();
        
        public virtual void Save()  {  Save(null);  }
        public virtual void SaveIfChanged()  {  if(HasChanged())  Save(null);  }
        public abstract void Save(string userName);

		public abstract string OuterXml();
        public abstract void ToXml(XmlWriter xw);
		public abstract void ToXml(TextWriter xw);
		public abstract void ToXml(StringBuilder sb);
		public abstract void ToXml(StringBuilder sb, string OuterTagName);

		public abstract QueryCommand GetInsertCommand(string userName);
        public abstract QueryCommand GetUpdateCommand(string userName);

        public abstract int RecordHashNow();
        public bool HasChanged()  { return _IsNew || _RecordHashThen == 0 || _RecordHashThen != RecordHashNow();  }
    }

    [Serializable]
    public abstract class ActiveList<T> : List<T> where T : ActiveRecord
    {
		public abstract string OuterXml();
		public abstract string InnerXml();
		public abstract void ToXml(TextWriter Output);
		public abstract void ToXml(XmlWriter xw);
		public abstract void ToXml(StringBuilder sb);
		public abstract void ToXml(StringBuilder sb, string OuterTagName);
	}

}
