using System;
using System.IO;
using System.Xml;

namespace MetX.Library
{
    public class Xhtml
    {
        public const string Declaration = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        public const string TagHtmlBegin = "<html xmlns=\"http://www.w3.org/1999/xhtml\">";

        public void BeginDoc(TextWriter output, XmlDocument target)
        {
            output.WriteLine(Declaration);
            output.WriteLine(TagHtmlBegin);
            output.WriteLine("<title>Auto Output</title>");
            output.WriteLine("</head>");
            output.WriteLine("<body>");
        }

        public void EndDoc(TextWriter output, XmlDocument target)
        {
            output.WriteLine("</body>");
            output.WriteLine("</html>");
        }

        public void ToDiv(TextWriter output, XmlElement target)
        {
            output.WriteLine("<span class=\"" + target.Name + "_span\">" + target.Name + "</span>");
            output.WriteLine("<div class=\"" + target.Name + "_div\">");
            throw new Exception("Coding not completed. Finish or don't use me.");
        }
    }
}