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
        Player player;
        Player cpu;

        public GameMainScene()
        {
            phase = GamePhase.SelectingHandCardPhase;
            selectedFieldCard = null;
            selectedHandCard = null;
            player = new Player(this, PlayerAttribute.Human);
            cpu = new Player(this, PlayerAttribute.CPU);
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
                    GameOperator.cardsInField.Remove(selectedFieldCard);
                    GameOperator.cardsInPlayersHand.Remove(selectedHandCard);
                    //Engine.RemoveNode(handcard);
                    //Engine.RemoveNode(fieldcard);
                    //handcard = null;
                    //fieldcard = null;
                    phase = GamePhase.SelectingHandCardPhase;
                    GettingCards(handcard, fieldcard, player);
                }
            }
        }

        public void GettingCards(Card handCard, Card fieldCard, Player player)
        {
            var margin = new Vector2F(2, 4);
            float x, y;
            if(player._attribute == PlayerAttribute.Human)
            {
                handCard.Scale = new Vector2F(0.25f, 0.25f);
                handCard.Size = handCard.Texture.Size * handCard.Scale;
                switch (handCard.CardRank)
                {
                    case CardRank.Hikari:
                        x = player.CardStockPosition.X + handCard.Size.X * player.HikariCardCollection.Count;
                        y = player.CardStockPosition.Y;
                        handCard.Position = new Vector2F(x, y);
                        player.HikariCardCollection.Add(handCard);
                        break;
                    case CardRank.Tane:
                        x = player.CardStockPosition.X  + handCard.Size.X * player.TaneCardCollection.Count;
                        y = player.CardStockPosition.Y + margin.Y + handCard.Size.Y;
                        handCard.Position = new Vector2F(x, y);
                        player.TaneCardCollection.Add(handCard);
                        break;
                    case CardRank.Tanzaku:
                        x = player.CardStockPosition.X  + handCard.Size.X * player.TanzakuCardCollection.Count;
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * 2;
                        handCard.Position = new Vector2F(x, y);
                        player.TanzakuCardCollection.Add(handCard);
                        break;
                    case CardRank.Kasu:
                        x = player.CardStockPosition.X  + handCard.Size.X * player.KasuCardCollection.Count;
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * 3;
                        handCard.Position = new Vector2F(x, y);
                        player.KasuCardCollection.Add(handCard);
                        break;
                    default:
                        break;
                }

                fieldCard.Scale = new Vector2F(0.25f, 0.25f);
                fieldCard.Size = fieldCard.Texture.Size * fieldCard.Scale;
                Console.WriteLine(player.HikariCardCollection.Count);
                Console.WriteLine(player.TaneCardCollection.Count);
                Console.WriteLine(player.TanzakuCardCollection.Count);
                Console.WriteLine(player.KasuCardCollection.Count);
                switch (fieldCard.CardRank)
                {
                    case CardRank.Hikari:
                        x = player.CardStockPosition.X  +(fieldCard.Size.X) * player.HikariCardCollection.Count;
                        y = player.CardStockPosition.Y;
                        fieldCard.Position = new Vector2F(x, y);
                        player.HikariCardCollection.Add(fieldCard);
                        break;
                    case CardRank.Tane:
                        x = player.CardStockPosition.X + (fieldCard.Size.X) * player.TaneCardCollection.Count;
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y);
                        fieldCard.Position = new Vector2F(x, y);
                        player.TaneCardCollection.Add(fieldCard);
                        break;
                    case CardRank.Tanzaku:
                        x = player.CardStockPosition.X + fieldCard.Size.X * player.TanzakuCardCollection.Count;
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * 2;
                        fieldCard.Position = new Vector2F(x, y);
                        player.TanzakuCardCollection.Add(fieldCard);
                        break;
                    case CardRank.Kasu:
                        x = player.CardStockPosition.X + fieldCard.Size.X * player.KasuCardCollection.Count;
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * 3;
                        fieldCard.Position = new Vector2F(x, y);
                        player.KasuCardCollection.Add(fieldCard);
                        break;
                    default:
                        break;
                }
            }
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            switch (phase)
            {
                case GamePhase.SelectingHandCardPhase:
                    selectedHandCard = GetKeyOfCardsInPlayersHandCollideWithMousePointer();
                    break;
                case GamePhase.SelectingFieldCardPhase:
                    selectedFieldCard = GetKeyOfCardsInFiledWithMousePointer();
                    break;
                case GamePhase.checkingPhase:
                    CheckMatchingSelectedCardMonth(selectedHandCard, selectedFieldCard);
                    break;

            }
        }
    }
}
