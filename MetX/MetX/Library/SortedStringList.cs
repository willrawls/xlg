using System.Text; 

namespace MetX
{
    /// <summary>
    /// Basic implmentation of a Sorted String List (Basically the same as System.Collections.Generic.SortedList&lt;string, string>)
    /// </summary>
    public class SortedStringList : System.Collections.Generic.SortedList<string, string>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SortedStringList() : base() { }

        /// <summary>
        /// Creates a sorted string list with an initial capacity
        /// </summary>
        /// <param name="Capacity">The initial capacity of the sorted list.</param>
        public SortedStringList(int Capacity) : base(Capacity) { }

        /// <summary>Retrieves the list as a xml string</summary>
        /// <param name="TagName">The name of each element and the name of the wrapping element (appending an "s")</param>
        /// <param name="TagAttributes">Any attributes to be added to the wrapping element</param>
        /// <returns>An xml string of TagName elements wrapped in a TagName + "s" element with attributes equal to TagAttributes</returns>
        public string ToXml(string TagName, string TagAttributes)
        {
            if (Count > 0)
            {
                StringBuilder ret = new StringBuilder();
                ret.AppendLine("<" + TagName + "s" + (TagAttributes != null ? " " + TagAttributes : string.Empty) + ">");
                foreach (System.Collections.Generic.KeyValuePair<string, string> CurrItem in this)
                    ret.AppendLine("<" + TagName + " Name=\"" + xml.AttributeEncode(CurrItem.Key) + "\" Value=\"" + xml.AttributeEncode(CurrItem.Value) + "\"/>");
                ret.AppendLine("</" + TagName + "s>");
                return ret.ToString();
            }
            return string.Empty;
        }
    }
} 