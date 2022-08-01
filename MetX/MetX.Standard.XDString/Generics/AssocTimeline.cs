using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;
using System;
using System.Linq;

namespace MetX.Standard.XDString.Generics;

public class AssocTimeline : AssocTimeline<BasicAssocItem, BasicAssocItem>
{

}

public class AssocTimeline<TAxis, TItem> 
    where TAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArrayOfT<AssocTimelineItem<TAxis, TItem>> Timeline = new();

    public TItem this[DateTime at, string firstAxisKey, string secondAxisKey, string thirdAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() 
                || secondAxisKey.IsEmpty()
                || thirdAxisKey.IsEmpty())
                return null;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            return timeline.Item.FirstAxis[firstAxisKey, secondAxisKey, thirdAxisKey];
        }
        set
        {
            if (firstAxisKey.IsEmpty() 
                || secondAxisKey.IsEmpty()
                || thirdAxisKey.IsEmpty())
                return;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            timeline.Item.FirstAxis[firstAxisKey, secondAxisKey, thirdAxisKey] = value;
        }
    }

    public TItem this[DateTime at, TAxis first, TAxis second, TAxis third]
    {
        get
        {
            if (first == null 
                || second == null
                || third == null
                || first.ID == Guid.Empty
                || second.ID == Guid.Empty
                || third.ID == Guid.Empty)
                return null;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            return timeline.Item.FirstAxis[first.ID, second.ID , third.ID];
        }
        set
        {
            if (first == null 
                || second == null
                || third == null
                || first.ID == Guid.Empty
                || second.ID == Guid.Empty
                || third.ID == Guid.Empty)
                return;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            timeline.Item.FirstAxis[first.ID, second.ID, third.ID] = value;
        }
    }

    public TItem this[DateTime at, Guid firstAxisId, Guid secondAxisId, Guid thirdAxisId]
    {
        get
        {
            if (firstAxisId == Guid.Empty 
                || secondAxisId == Guid.Empty
                || thirdAxisId == Guid.Empty)
                return null;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            return timeline.Item.FirstAxis[firstAxisId, secondAxisId, thirdAxisId];
        }
        set
        {
            if (firstAxisId == Guid.Empty 
                || secondAxisId == Guid.Empty
                || thirdAxisId == Guid.Empty)
                return;

            var timeline = Timeline[at.ToString("s")];
            timeline.Item.At = at;
            timeline.Item.FirstAxis[firstAxisId, secondAxisId, thirdAxisId] = value;
        }
    }
}