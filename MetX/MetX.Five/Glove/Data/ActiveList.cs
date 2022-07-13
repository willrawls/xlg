using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MetX.Five.Glove.Data;

[Serializable]
public abstract class ActiveList<T> : List<T> where T : ActiveRecord
{
    public abstract string OuterXml();
    public abstract string InnerXml();
    public abstract void ToXml(TextWriter output);
    public abstract void ToXml(XmlWriter xw);
    public abstract void ToXml(StringBuilder sb);
    public abstract void ToXml(StringBuilder sb, string outerTagName);
}