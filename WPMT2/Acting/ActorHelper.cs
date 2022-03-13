using System.Collections.Generic;
using MetX.Standard.Library;
using MetX.Standard.Library.Strings;
using NHotPhrase.Keyboard;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Acting
{
    public static class ActorHelper
    {
        private static Actionables _actionables;

        public static Actionables Actionables
        {
            get { return _actionables ??= Initialize(); }
            set => _actionables = value;
        }

        public static Actionables Initialize()
        {
            _actionables = new Actionables();

            ActionableItem.WithActorFactory(() => new TypeActor());
            ActionableItem.WithActorFactory(() => new ChooseActor());
            ActionableItem.WithActorFactory(() => new MoveActor());
            ActionableItem.WithActorFactory(() => new RunActor());
            ActionableItem.WithActorFactory(() => new SizeActor());
            ActionableItem.WithActorFactory(() => new RandomActor());
            Continuation = ActionableItem.WithActorFactory(() => new ContinuationActor());
            Unknown = ActionableItem.WithActorFactory(() => new UnknownActor());

            return Actionables;
        }

        public static ActionableItem Unknown { get; set; }

        public static ActionableItem Continuation { get; set; }

        public static ActionableItem GetActionType(string line)
        {
            var lower = line.Replace("\r", "\n").ToLower();
            if (lower.StartsWith("when ")) lower = lower.TokensAfterFirst("when ");
            if (lower.StartsWith("or ")) lower = lower.TokensAfterFirst("or ");

            var item = Actionables.MatchingActionable(lower);

            return item;
        }

        public static TActor Factory<TActor>(string item, CustomPhraseManager customPhraseManager, TActor previousActor = default) 
            => Factory(item, customPhraseManager, previousActor);

        public static BaseActor Factory(string item, CustomPhraseManager customPhraseManager, BaseActor previousActor = null)
        {
            var actionableItem = GetActionType(item);
            if (actionableItem == null)
            {
                if (previousActor is not { CanContinue: true }) return null;

                return previousActor.OnContinue(item) ? previousActor : null;
            }

            var actor = actionableItem.Factory(item, previousActor);
            actor.Manager = customPhraseManager;
            return actor is {ActionableType: ActionableType.Unknown} 
                ? null 
                : actor;
        }
    }
}