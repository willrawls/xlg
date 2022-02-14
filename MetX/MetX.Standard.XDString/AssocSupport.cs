/*using System;
using System.Security.Cryptography.X509Certificates;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Library
{
    public static class AssocSupport
    {
        //  Cetas = Consciousness
        //      www.learnsanskrit.cc/index.php?mode=3&direct=au&script=hk&tran_input=conciousness
        public static Func<string> KeyWhenThereIsNoKey { get; set; } = () => "Cetas";

        public static string ToAssocKey(this string target, string defaultKey = null) => InternalToAssocKey(target, defaultKey);
        
        public static Func<string, string, string> InternalToAssocKey = (target, defaultKey) =>             
            target.IsEmpty()
            ? defaultKey.IsEmpty() 
            ? KeyWhenThereIsNoKey() 
            : defaultKey
        : $"{target.ToLowerInvariant()}_aak";


    }
}*/