﻿using System.Collections.Generic;
using UAT_MS539.Core.Code.Cryptid;
using UAT_MS539.Core.Code.Food;
using UAT_MS539.Core.Code.StateMachine.Interactions;
using UAT_MS539.Core.Code.StateMachine.Signals;
using UAT_MS539.Core.Code.Utility;

namespace UAT_MS539.Core.Code.StateMachine.States
{
    public class CorralMorningState : IState
    {
        public string LocationLocId => "Location/Corral";
        public string TimeLocId => "Time/Morning";

        private Context _sharedContext;

        public void PerformSetup(Context context, IState previousState)
        {
            _sharedContext = context;
        }

        public void PerformContent(Context context)
        {
            var playerData = context.Get<PlayerData>();

            string speciesName = context.Get<LocDatabase>().Localize(playerData.ActiveCryptid.Species.NameId);

            var finalFoodList = new List<Food.Food>(playerData.FoodInventory);
            finalFoodList.Add(FoodUtilities.CreateBasicRation());

            context.Get<InteractionEventRaised>().Fire(new IInteraction[]
            {
                new Dialog("Corral/Morning/FeedingPrompt", new KeyValuePair<string, string>("{species}", speciesName)),
                new FoodSelection(finalFoodList, OnFoodSelected)
            });
        }

        public void PerformTeardown(Context context, IState nextState)
        {
        }

        private void OnFoodSelected(Food.Food food)
        {
            var playerData = _sharedContext.Get<PlayerData>();
            var trainingData = new DailyTrainingData();
            _sharedContext.Set(trainingData);

            playerData.FoodInventory.Remove(food);
            trainingData.Food = food;

            var interactions = new List<IInteraction>
            {
                new Dialog("Corral/Morning/ActivityPrompt"),
                new Option("Button/ToTown", OnTownSelected)
            };

            if (playerData.ActiveCryptid != null) interactions.Add(new Option("Button/Train", OnTrainingSelected));

            if (playerData.Day % 7 == 6) interactions.Add(new Option("Button/ToColiseum", OnColiseumSelected));

            _sharedContext.Get<InteractionEventRaised>().Fire(interactions);
        }

        private void OnTrainingSelected()
        {
            _sharedContext.Get<StateMachine>().ChangeState<CorralDayState>();
        }

        private void OnTownSelected()
        {
            _sharedContext.Get<StateMachine>().ChangeState<TownMainState>();
        }

        private void OnColiseumSelected()
        {
            _sharedContext.Get<StateMachine>().ChangeState<ColiseumMainState>();
        }
    }
}