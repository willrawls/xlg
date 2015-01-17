using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Collections.Generic;
using MetX.Library;

namespace MetX.IO
{
    /// <summary>Given two strings (such as a unique userid and the path to a web page),
    /// StateCollection allows for an arbitrary number of name/value pairs to be quickly loaded and saved to the StateCollection table.</summary>
    public partial class StateCollection
    {
        /// <summary>The internal sorted list in memory</summary>
        private SortedStringList mState;
        /// <summary>The first of two unique strings that identify the secure state to work with.</summary>
        public string StateParent;
        /// <summary>The second of two unique strings that identify the secure state to work with.</summary>
        public string StateName;
        /// <summary>Used by InnerXml, This is the name of each element.</summary>
        public string TagName;

        /// <summary>The name of the connection to use to connect to the database. Defaults to 'xlgSecurity'</summary>
        public string ConnectionName = "xlgSecurity";

        /// <summary>Basic constructor. Does not load a profile, just sets up the properties.</summary>
        /// <param name="StateParent">The first of two unique strings that identify the secure state to work with. Usually the UserID guid.</param>
        /// <param name="StateName">The second of two unique strings that identify the secure state to work with. Usually the path to the web page.</param>
        /// <param name="TagName">Used by InnerXml, This is the name of each element. Usually something like "Profile" or "PageState".</param>
        public StateCollection(string StateParent, string StateName, string TagName)
        {
            if (StateParent == null || StateParent.Length == 0)
                throw new Exception("StateParent must be set to a non empty string.");
            if (StateName == null || StateName.Length == 0)
                throw new Exception("StateName must be set to a non empty string.");
            if (TagName == null || TagName.Length == 0)
                throw new Exception("TagName must be set to a non empty string.");
            this.StateParent = StateParent;
            this.StateName = StateName;
            this.TagName = TagName;
        }

        /// <summary>Basic constructor that additionally sets the intial state. Useful when loading state from an existing list or when setting up a new secure state.</summary>
        /// <param name="StateParent">The first of two unique strings that identify the secure state to work with. Usually the UserID guid.</param>
        /// <param name="StateName">The second of two unique strings that identify the secure state to work with. Usually the path to the web page.</param>
        /// <param name="TagName">Used by InnerXml, This is the name of each element. Usually something like "Profile" or "PageState".</param>
        /// <param name="InitialState">The initial set of name/value pairs for the named secure state.</param>
        public StateCollection(string StateParent, string StateName, string TagName, SortedStringList InitialState)
            : this(StateParent, StateName, TagName)
        {            
            this.mState = InitialState;
        }

        /// <summary>Retrieves/Sets the string value for a name/value pair. If the item doesn't exist, the item will be created.</summary>
        public string this[string Name]
        {
            get
            {
                if (State.ContainsKey(Name))
                    return System.Convert.ToString(mState[Name]);
                else
                    mState.Add(Name, string.Empty);
                return string.Empty;
            }
            set
            {
                if (State.ContainsKey(Name))
                    mState[Name] = value;
                else
                    mState.Add(Name, value);
            }
        }

        /// <summary>The secure state will load the first time this property is called and when a name/value list is not supplied in the constructor. Setting this property will overwrite the entire list.</summary>
        public SortedStringList State
        {
            get
            {
                if (mState == null)
                {
                    mState = new SortedStringList(5);
                    DataRowCollection rstState = sql.ToDataRows("SELECT Name, Value FROM ItemState WHERE StateParent=" + Worker.s2db(StateParent.ToLower()) + " AND StateName=" + Worker.s2db(StateName.ToLower()), ConnectionName);
                    if (rstState != null)
                    {
                        if (rstState.Count > 0)
                        {
                            foreach (DataRow CurrRow in rstState)
                            {
                                mState.Add(System.Convert.ToString(CurrRow[0]), System.Convert.ToString(CurrRow[1]));
                            }
                        }
                    }
                }
                return mState;
            }
            set
            {
                mState = value;
            }
        }

        /// <summary>Saves the current state to the StateCollection table. All previous items in the state are deleted.</summary>
        public void Save()
        {
            List<string> SQLs = null;
            if (mState != null)
            {
                SQLs = new List<string>(1 + mState.Count);
                SQLs.Add("DELETE FROM ItemState WHERE StateParent=" + Worker.s2db(StateParent.ToLower()) + " AND StateName=" + Worker.s2db(StateName.ToLower()));

                if (mState.Count > 0)
                    foreach (System.Collections.Generic.KeyValuePair<string, string> DE in mState)
                        SQLs.Add("INSERT INTO ItemState VALUES ('" + Guid.NewGuid().ToString() + "'," + Worker.s2db(StateParent) + "," + Worker.s2db(StateName) + "," + Worker.s2db(DE.Key) + "," + Worker.s2db(DE.Value) + ", getdate())");
                sql.Execute(SQLs, ConnectionName);
            }
        }

        /// <summary>Returns a string containing the xml representation of this state. The elements are wrapped with another element with the same name as TagName followed by an "s". So if TagName = "Item", then InnerXml will return a Items elemnt with one Item element per name/value pair.</summary>
        public string InnerXml
        {
            get
            {
                return State.ToXml(TagName, "StateParent=\"" + Xml.AttributeEncode(StateParent) + "\" StateName=\"" + Xml.AttributeEncode(StateName) + "\"");
            }
        }
    }
}
