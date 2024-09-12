using System.Text;
using MetX.Standard.Strings.ML;

namespace MetX.Standard.Library
{
    /// <summary>
    /// Basic implementation of a Sorted String List (Basically the same as System.Collections.Generic.SortedList&lt;string, string>)
    /// </summary>
    public class SortedStringList : System.Collections.Generic.SortedList<string, string>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SortedStringList()
        { }

        /// <summary>
        /// Creates a sorted string list with an initial capacity
        /// </summary>
        /// <param name="capacity">The initial capacity of the sorted list.</param>
        public SortedStringList(int capacity) : base(capacity) { }

        /// <summary>Retrieves the list as a xml string</summary>
        /// <param name="tagName">The name of each element and the name of the wrapping element (appending an "s")</param>
        /// <param name="tagAttributes">Any attributes to be added to the wrapping element</param>
        /// <returns>An xml string of TagName elements wrapped in a TagName + "s" element with attributes equal to TagAttributes</returns>
        public string ToXml(string tagName, string tagAttributes)
        {
            if (Count > 0)
            {
                var ret = new StringBuilder();
                ret.AppendLine("<" + tagName + "s" + (tagAttributes != null ? " " + tagAttributes : string.Empty) + ">");
                foreach (var item in this)
                    ret.AppendLine("<" + tagName + " Name=\"" + Xml.AttributeEncode(item.Key) + "\" Value=\"" + Xml.AttributeEncode(item.Value) + "\"/>");
                ret.AppendLine("</" + tagName + "s>");
                return ret.ToString();
            }
            return string.Empty;
        }
    }
} 