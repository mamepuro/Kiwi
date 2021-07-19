using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Altseed2;

namespace Kiwi
{
    class GameMainScene:Node
    {
        GamePhase phase { get; set; }
        Card selectedHandCard { get; set; }
        Card selectedFieldCard { get; set; }

        public GameMainScene()
        {
            phase = GamePhase.SelectingHandCardPhase;
            selectedFieldCard = null;
            selectedHandCard = null;
        }

        /// <summary>
        /// クリックされたときに場に出ているカードと衝突判定を行う
        /// </summary>
        /// <returns>衝突している場合は衝突したカードと対応するキー値を、何も衝突していなければ-1が返ります</returns>
        public Card GetKeyOfCardsInFiledWithMousePointer()
        {
            if(Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push && phase == GamePhase.SelectingFieldCardPhase)
            {
                var clickedPos = Engine.Mouse.Position;
                bool isCollide = false;
                foreach(var card in GameOperator.cardsInField)
                {
                    var edgePos = card.Position + card.Size;
                    var cardPos = card.Position;
                    if(clickedPos.X >= cardPos.X && 
                        clickedPos.X <= edgePos.X &&
                        clickedPos.Y >= cardPos.Y &&
                        clickedPos.Y <= edgePos.Y)
                    {
                        Console.WriteLine("花札" + card + "と衝突した");
                        phase = GamePhase.checkingPhase;
                        return card;
                    }
                }
            }
            return null;
        }

        public Card GetKeyOfCardsInPlayersHandCollideWithMousePointer()
        {
            if (Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push && phase == GamePhase.SelectingHandCardPhase)
            {
                var clickedPos = Engine.Mouse.Position;
                foreach (var card in GameOperator.cardsInPlayersHand)
                {
                    var edgePos = card.Position + card.Size;
                    var cardPos = card.Position;
                    if (clickedPos.X >= cardPos.X &&
                        clickedPos.X <= edgePos.X &&
                        clickedPos.Y >= cardPos.Y &&
                        clickedPos.Y <= edgePos.Y)
                    {
                        Console.WriteLine("花札" + GameOperator.cardsInPlayersHand.IndexOf(card)+" ("+ card.Month +"月の花札)"+"と衝突した");
                        phase = GamePhase.SelectingFieldCardPhase;
                        return card;
                    }
                }
            }
            return null;

        }

        public void CheckMatchingSelectedCardMonth(Card handcard, Card fieldcard)
        {
            if (handcard == null || fieldcard == null || phase == GamePhase.SelectingFieldCardPhase)
            {
                return;
            }
            else
            {
                if(handcard.Month == fieldcard.Month)
                {
                    Engine.RemoveNode(handcard);
                    Engine.RemoveNode(fieldcard);
                    handcard = null;
                    fieldcard = null;
                    phase = GamePhase.SelectingHandCardPhase;
                }
            }
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if(phase == GamePhase.SelectingHandCardPhase)
            {
                selectedHandCard = GetKeyOfCardsInPlayersHandCollideWithMousePointer();
            }
            if(phase == GamePhase.SelectingFieldCardPhase)
            {
                selectedFieldCard = GetKeyOfCardsInFiledWithMousePointer();
            }
            CheckMatchingSelectedCardMonth(selectedHandCard, selectedFieldCard);
        }
    }
}
