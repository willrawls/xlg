using System;
using MetX.Standard.Strings.Generics;

namespace WilliamPersonalMultiTool.Acting
{
    public class Actionables : AssocArrayOfT<ActionableItem>
    {
        public bool ContainsActionableKeyword(string line)
        {
            lock (SyncRoot)
            {
                foreach (var assocItem in this)
                {
                    var separator = " " + assocItem.Key;
                    if (line.Contains(separator, StringComparison.InvariantCultureIgnoreCase)
                        || line.EndsWith(separator, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }

                return false;
            }
        }

        public ActionableItem MatchingActionable(string line)
        {
            lock (SyncRoot)
            {
                foreach (var assocItem in this)
                {
                    var separator = " " + assocItem.Key;
                    if (line.Contains(separator, StringComparison.InvariantCultureIgnoreCase)
                        || line.EndsWith(separator, StringComparison.InvariantCultureIgnoreCase))
                        return assocItem.Item;
                }

                return null;
            }
        }

    }
}