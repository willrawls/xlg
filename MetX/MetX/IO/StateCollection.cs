using System;
using System.Data;
using System.Collections.Generic;
using MetX.Library;

namespace MetX.IO
{
    /// <summary>Given two strings (such as a unique userid and the path to a web page),
    /// StateCollection allows for an arbitrary number of name/value pairs to be quickly loaded and saved to the StateCollection table.</summary>
    public class StateCollection
    {
        /// <summary>The internal sorted list in memory</summary>
        private SortedStringList _mState;
        /// <summary>The first of two unique strings that identify the secure state to work with.</summary>
        public string StateParent;
        /// <summary>The second of two unique strings that identify the secure state to work with.</summary>
        public string StateName;
        /// <summary>Used by InnerXml, This is the name of each element.</summary>
        public string TagName;

        /// <summary>The name of the connection to use to connect to the database. Defaults to 'xlgSecurity'</summary>
        public string ConnectionName = "xlgSecurity";

        /// <summary>Basic constructor. Does not load a profile, just sets up the properties.</summary>
        /// <param name="stateParent">The first of two unique strings that identify the secure state to work with. Usually the UserID guid.</param>
        /// <param name="stateName">The second of two unique strings that identify the secure state to work with. Usually the path to the web page.</param>
        /// <param name="tagName">Used by InnerXml, This is the name of each element. Usually something like "Profile" or "PageState".</param>
        public StateCollection(string stateParent, string stateName, string tagName)
        {
            if (stateParent == null || stateParent.Length == 0)
                throw new Exception("StateParent must be set to a non empty string.");
            if (stateName == null || stateName.Length == 0)
                throw new Exception("StateName must be set to a non empty string.");
            if (tagName == null || tagName.Length == 0)
                throw new Exception("TagName must be set to a non empty string.");
            StateParent = stateParent;
            StateName = stateName;
            TagName = tagName;
        }

        /// <summary>Basic constructor that additionally sets the intial state. Useful when loading state from an existing list or when setting up a new secure state.</summary>
        /// <param name="stateParent">The first of two unique strings that identify the secure state to work with. Usually the UserID guid.</param>
        /// <param name="stateName">The second of two unique strings that identify the secure state to work with. Usually the path to the web page.</param>
        /// <param name="tagName">Used by InnerXml, This is the name of each element. Usually something like "Profile" or "PageState".</param>
        /// <param name="initialState">The initial set of name/value pairs for the named secure state.</param>
        public StateCollection(string stateParent, string stateName, string tagName, SortedStringList initialState)
            : this(stateParent, stateName, tagName)
        {            
            _mState = initialState;
        }

        /// <summary>Retrieves/Sets the string value for a name/value pair. If the item doesn't exist, the item will be created.</summary>
        public string this[string name]
        {
            get
            {
                if (State.ContainsKey(name))
                    return Convert.ToString(_mState[name]);
                else
                    _mState.Add(name, string.Empty);
                return string.Empty;
            }
            set
            {
                if (State.ContainsKey(name))
                    _mState[name] = value;
                else
                    _mState.Add(name, value);
            }
        }

        /// <summary>The secure state will load the first time this property is called and when a name/value list is not supplied in the constructor. Setting this property will overwrite the entire list.</summary>
        public SortedStringList State
        {
            get
            {
                if (_mState != null) return _mState;
                
                _mState = new SortedStringList(5);
                var rstState = Sql.ToDataRows("SELECT Name, Value FROM ItemState WHERE StateParent=" + Worker.S2Db(StateParent.ToLower()) + " AND StateName=" + Worker.S2Db(StateName.ToLower()), ConnectionName);
                if (rstState == null || rstState.Count <= 0) return _mState;
                    
                foreach (DataRow currRow in rstState)
                {
                    _mState.Add(currRow[0].AsString(), currRow[1].AsString());
                }
                return _mState;
            }
            // set => _mState = value;
        }

        /// <summary>Saves the current state to the StateCollection table. All previous items in the state are deleted.</summary>
        public void Save()
        {
            if (_mState != null)
            {
                var sqLs = new List<string>(1 + _mState.Count)
                {
                    "DELETE FROM ItemState WHERE StateParent=" + Worker.S2Db(StateParent.ToLower()) +
                    " AND StateName=" + Worker.S2Db(StateName.ToLower())
                };

                if (_mState.Count > 0)
                    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var de in _mState)
                    {
                        sqLs.Add("INSERT INTO ItemState VALUES ('" + Guid.NewGuid() + "'," 
                                 + Worker.S2Db(StateParent) + "," 
                                 + Worker.S2Db(StateName) + "," 
                                 + Worker.S2Db(de.Key) + "," 
                                 + Worker.S2Db(de.Value) + ", getdate())");
                    }

                Sql.Execute(sqLs, ConnectionName);
            }
        }

        /// <summary>Returns a string containing the xml representation of this state. The elements are wrapped with another element with the same name as TagName followed by an "s". So if TagName = "Item", then InnerXml will return a Items elemnt with one Item element per name/value pair.</summary>
        public string InnerXml => State.ToXml(TagName, "StateParent=\"" + Xml.AttributeEncode(StateParent) + "\" StateName=\"" + Xml.AttributeEncode(StateName) + "\"");
    }
}
