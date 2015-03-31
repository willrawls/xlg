using System.Collections.Generic;

namespace MetX.Data
{
    public class CaseFreeDictionary : Dictionary<string, string>
    {
        public new string this[string name]
        {
            get
            {
                name = name.ToLower();
                return ContainsKey(name) ? base[name] : null;
            }
            set
            {
                name = name.ToLower();
                if(ContainsKey(name))
                    base[name.ToLower()] = value;
                else
                    Add(name, value);
            }
        }
    }
}