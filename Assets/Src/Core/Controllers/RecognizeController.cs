using System;
using System.Collections;
using System.Collections.Generic;
using Shkoda.RecognizeMe.Core.Game.Achievements;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using Shkoda.RecognizeMe.Core.Mechanics;
using Shkoda.RecognizeMe.Core.Mechanics.Actions;
using Shkoda.RecognizeMe.Core.Mechanics.Actions.Handlers;
using UnityEngine;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

namespace Shkoda.Rec.Core.Controllers
{
    public class RecognizeController
    {
        private readonly List<IAction> actions = new List<IAction>();
        protected readonly Graphics Graphics;
             public event Action<GameFinishedEventArgs> GameFinished = delegate { };
             public event Action TilesInitialized = delegate { };
        protected RecognizeController(GameActionHandler actionHandler)
        {
            Graphics = Graphics.Instance;

            ActionsHandler = actionHandler;
        }

        protected Mechanics Mechanics { get; private set; }
        public GameActionHandler ActionsHandler { get; set; }

        /// <summary>
        ///     Called when game is finished
        /// </summary>
        public void CleanUp()
        {
            Graphics.GatherAndClear();
            UnSubscribe();
        }

        public void StartGame()
        {
            AppController.StartRoutine(StartGameRoutine(false));
        }

        public void Subscribe()
        {
            Graphics.TileSelectionStarted += OnTileSelectionStarted;
            Graphics.TileSelectionUpdated += OnTileSelectionUpdated;
            Graphics.TileSelectionFinished += OnTileSelectionFinished;
        }

        public void UnSubscribe()
        {
            Graphics.TileSelectionStarted -= OnTileSelectionStarted;
            Graphics.TileSelectionUpdated -= OnTileSelectionUpdated;
            Graphics.TileSelectionFinished -= OnTileSelectionFinished;
        }

        public void FinishGame()
        {
            GameFinished(ActionsHandler.CreateGameFinishedEventArgs());
        }

        protected virtual void OnTileSelectionStarted(object obj, StartTileSelectionEventArgs args)
        {
        }

        protected virtual void OnTileSelectionUpdated(object obj, UpdateTileSelectionEventArgs args)
        {
        }

        protected virtual void OnTileSelectionFinished(object obj, FinishTileSelectionEventArgs args)
        {
        }

        protected IEnumerator StartGameRoutine(bool isTutorial)
        {
            // Should wait till the end of init
            yield return AppController.StartRoutine(Graphics.Init());

            // Init all cards
            if (isTutorial)
            {
               Mechanics.DealTutorial();
            }
            else
            {
                Mechanics.Deal();
            }

            // Deal closed cards
            var delay = 0;
            const float Duration = 0.06f;

//            // Deal tableaux 
//            for (var tableauNumber = 0; tableauNumber < 7; tableauNumber++)
//            {
//                var dstId = new DeckId(DeckClass.Tableau, tableauNumber);
//                var tableau = mechanics.Tableaux[tableauNumber];
//                var cardsInTableau = tableau.Count;
//                delay += Math.Max(0, cardsInTableau - 1);
//
//                for (var cardNumber = 0; cardNumber < cardsInTableau; cardNumber++)
//                {
//                    // Move closed card (animation)
//                    Graphics.MoveTopCard(DeckId.DefaultDeck, dstId, Duration*(delay + cardNumber));
//
//                    // Define card value
//                    var cardValue = tableau.CardValues[cardsInTableau - cardNumber - 1];
//                    Graphics.SetCardValue(dstId, cardNumber, false, cardValue);
//                }
//            }
//
//            // Deal foundations 
//            delay += this.DealDecksWithOpenedCards(this.mechanics.Foundations, Duration, delay*Duration);
//
//            // Deal Waste
//            delay += this.DealDeckWithOpenedCards(this.mechanics.Waste, Duration, delay*Duration);
//
//            // Stock deal animation 
//            var stockDeckId = new DeckId(DeckClass.Stock);
//            var cardsInStock = mechanics.Stock.Count;
//            for (var i = 0; i < cardsInStock; i++)
//            {
//                // Move closed card (animation)
//                Graphics.MoveTopCard(DeckId.DefaultDeck, stockDeckId, (delay + 7 + i)*Duration);
//
//                // Define card value
//                var cardValue = mechanics.Stock[cardsInStock - 1 - i].CardValue;
//                Graphics.SetCardValue(stockDeckId, i, false, cardValue);
//            }
//
//            // Wait for deal animations to finish
//            yield return new WaitForSeconds(6*Duration + 0.1f);
//
//            Graphics.MoveToGame(GameType.Klondike);
//
//            yield return new WaitForSeconds((delay + 7)*Duration);
//
//            // Flip all opened tableau cards
//            for (var tableauNumber = 0; tableauNumber < 7; tableauNumber++)
//            {
//                var tableau = mechanics.Tableaux[tableauNumber];
//                if (tableau.Any)
//                {
//                    var deckId = new DeckId(DeckClass.Tableau, tableauNumber);
//
//                    for (var i = 0; i < tableau.Count; i++)
//                    {
//                        if (tableau[i].Opened)
//                        {
//                            Graphics.FlipCard(deckId, i, true, Duration*tableauNumber);
//                        }
//                    }
//                }
//            }
//
//            yield return new WaitForSeconds(8*Duration); // tableau count = 8
//            this.OnCardsInitialized();
        }


        public void OnApplicationQuit()
        {
//            throw new NotImplementedException();
        }
    }


}