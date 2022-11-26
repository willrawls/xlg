using System;
using System.Linq;
using MetX.Standard.Strings;

namespace WilliamPersonalMultiTool.Acting
{
    public class ActionableItem
    {
        public Func<BaseActor> InternalFactory;

        public BaseActor Factory(string item, BaseActor previousActor = null)
        {
            if (InternalFactory == null)
                return null;

            var actor = InternalFactory();
            actor.Initialize(item);

            if (previousActor == null || actor.ID == previousActor.ID) return actor;

            var keysToPrepend = previousActor.KeySequence
                .Sequence
                .Take(previousActor.KeySequence.Sequence.Count - 1)
                .ToList();

            if (!item.Trim().ToLower().StartsWith("when") && keysToPrepend.IsNotEmpty())
            {
                actor.KeySequence.Sequence.InsertRange(0, keysToPrepend);
                actor.KeySequence.BackspaceCount = actor.KeySequence.ToBackspaceCount(actor.KeySequence.Sequence);
            }

            return actor;
        }

        public static ActionableItem WithActorFactory(Func<BaseActor> factory)
        {
            var sampleActor = factory();

            var assocItem = ActorHelper.Actionables[sampleActor.ActionableType.ToString()];
            assocItem.Item.InternalFactory = factory;
            assocItem.Name = assocItem.Key;
            return assocItem.Item;
        }

    }
}