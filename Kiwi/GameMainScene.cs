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
        Player TurnMaster;
        int tmp = 0;
        /// <summary>
        /// 場の取られたカード達の位置を格納する
        /// </summary>
        public Stack<Vector2F> gottenCardsPosLog;
        /// <summary>
        /// フィールドのカードが取られた際にその取られたカードがいた位置
        /// </summary>
        Vector2F CardLostPosInFiled { get; set; }

        public GameMainScene()
        {
            phase = GamePhase.SelectingHandCardPhase;
            selectedFieldCard = null;
            selectedHandCard = null;
            player = new Player(this, PlayerAttribute.Human);
            cpu = new Player(this, PlayerAttribute.CPU);
            var d = new Deck(this);
            GameOperator.InitializeGame(this, ref Deck.deck, player, cpu);
            TurnMaster = player;
            gottenCardsPosLog = new Stack<Vector2F>();

        }

        /// <summary>
        /// クリックされたときに場に出ているカードと衝突判定を行う
        /// </summary>
        /// <returns>衝突している場合は衝突したカードと対応するキー値を、何も衝突していなければ-1が返ります</returns>
        public Card GetKeyOfCardsInFiledWithMousePointer()
        {
            if(Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push)
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
                        if(phase == GamePhase.SelectingFieldCardPhase)
                        {
                            phase = GamePhase.checkingPhase;
                        }
                        return card;
                    }
                }
            }
            else if (Engine.Mouse.GetMouseButtonState(MouseButton.ButtonRight) == ButtonState.Push && phase == GamePhase.SelectingFieldCardPhase)
            {
                Console.WriteLine("選択をキャンセルしました");
                phase = GamePhase.SelectingHandCardPhase;
            }
            return null;
        }

        public Card GetKeyOfCardsInPlayersHandCollideWithMousePointer()
        {
            if (Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push)
            {
                var clickedPos = Engine.Mouse.Position;
                foreach (var card in player.hand)
                { 
                    var edgePos = card.Position + card.Size;
                    var cardPos = card.Position;
                    if (clickedPos.X >= cardPos.X &&
                        clickedPos.X <= edgePos.X &&
                        clickedPos.Y >= cardPos.Y &&
                        clickedPos.Y <= edgePos.Y)
                    {
                        Console.WriteLine("花札" + player.hand.IndexOf(card)+" ("+ card.Month +"月の花札)"+"と衝突した");
                        phase = GamePhase.SelectingFieldCardPhase;
                        return card;
                    }
                }
            }
            if (Engine.Mouse.GetMouseButtonState(MouseButton.ButtonRight) == ButtonState.Push)
            {
                var clickedPos = Engine.Mouse.Position;
                foreach (var card in player.hand)
                {
                    var edgePos = card.Position + card.Size;
                    var cardPos = card.Position;
                    if (clickedPos.X >= cardPos.X &&
                        clickedPos.X <= edgePos.X &&
                        clickedPos.Y >= cardPos.Y &&
                        clickedPos.Y <= edgePos.Y)
                    {
                        Console.WriteLine("花札" + player.hand.IndexOf(card) + " (" + card.Month + "月の花札)" + "と衝突した");
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
                    player.hand.Remove(selectedHandCard);
                    //Engine.RemoveNode(handcard);
                    //Engine.RemoveNode(fieldcard);
                    //handcard = null;
                    //fieldcard = null;
                    phase = GamePhase.SwappingTurnMasterPhase;
                    GettingCards(handcard, fieldcard, player);
                }
                else
                {
                    if(phase == GamePhase.SecondCheckingPhase)
                    {
                        phase = GamePhase.SelectingOneMoreCardPhase;
                    }
                    else
                    {
                        phase = GamePhase.SelectingFieldCardPhase;
                    }                }
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
                handCard.whereAmI = Place.PlayersStock;
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
                        x = player.CardStockPosition.X  + handCard.Size.X * (player.KasuCardCollection.Count % 5);
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * (3 + (player.KasuCardCollection.Count / 5));
                        handCard.Position = new Vector2F(x, y);
                        player.KasuCardCollection.Add(handCard);
                        break;
                    default:
                        break;
                }

                fieldCard.Scale = new Vector2F(0.25f, 0.25f);
                fieldCard.Size = fieldCard.Texture.Size * fieldCard.Scale;
                fieldCard.whereAmI = Place.PlayersStock;
                gottenCardsPosLog.Push(fieldCard.Position);
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
                        x = player.CardStockPosition.X + fieldCard.Size.X * (player.KasuCardCollection.Count % 5);
                        y = player.CardStockPosition.Y + (margin.Y + handCard.Size.Y) * (3 + (player.KasuCardCollection.Count / 5));
                        fieldCard.Position = new Vector2F(x, y);
                        player.KasuCardCollection.Add(fieldCard);
                        break;
                    default:
                        break;
                }
                DrawOneMoreCard();
            }
        }

        private void DrawOneMoreCard()
        {
            var top = GameOperator.DisplayDeckTopCard(this);
            bool canGetOneMoreCard = false;
            foreach(var card in GameOperator.cardsInField)
            {
                if(card.Month == top.Month)
                {
                    canGetOneMoreCard = true;
                    break;
                }
            }
            if(canGetOneMoreCard)
            {
                //選んだ手札のカードを山札トップのカードとする
                selectedHandCard = top;
                phase = GamePhase.SelectingOneMoreCardPhase;
            }
            else
            {
                top.whereAmI = Place.Field;
                GameOperator.cardsInField.Add(top);
                top.Position = gottenCardsPosLog.Pop();
            }
            
        }

        private void CheckHanafudaHand(Player player)
        {
            if(player.HikariCardCollection.Count >= 3)
            {
                if(player.HikariCardCollection.Count == 5)
                {
                    player.HanafudadHands.Add(HanafudaHand.Gokou);
                    player.Score += 10;
                }
                else if(player.HikariCardCollection.Count == 3)
                {
                    player.HanafudadHands.Add(HanafudaHand.Sankou);
                    player.Score += 3;
                }
                else if(player.HikariCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.AmeShikou) != null)
                {
                    player.HanafudadHands.Add(HanafudaHand.AmeShikou);
                    player.Score += 7;
                }
                else
                {
                    player.HanafudadHands.Add(HanafudaHand.Shikou);
                    player.Score += 8;
                }
            }

            if(player.TaneCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Sakazuki) != null)
            {
                if(player.HikariCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Tsuki) != null)
                {
                    player.HanafudadHands.Add(HanafudaHand.TsukimiDeIppai);
                    player.Score += 5;
                }
                if (player.HikariCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Sakura) != null)
                {
                    player.HanafudadHands.Add(HanafudaHand.HanamiDeIppai);
                    player.Score += 5;
                }
            }

            if (player.TaneCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Inoshishi) != null &&
                player.TaneCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Shika) != null &&
                player.TaneCardCollection.Find(x => x.CardSpecialRank == CardSpecialRank.Chou) != null)
            {
                player.HanafudadHands.Add(HanafudaHand.InoShikaChou);
                player.Score += 5;
            }

            var akaTanList = player.TanzakuCardCollection.FindAll(x => x.CardSpecialRank == CardSpecialRank.AkaTan);
            var aoTanList = player.TanzakuCardCollection.FindAll(x => x.CardSpecialRank == CardSpecialRank.AoTan);

            if (akaTanList.Count == 3)
            {
                player.HanafudadHands.Add(HanafudaHand.AkaTan);
                player.Score += 5;
            }

            if (aoTanList.Count == 3)
            {
                player.HanafudadHands.Add(HanafudaHand.AoTan);
                player.Score += 5;
            }
            if(player.TaneCardCollection.Count >= 5)
            {
                player.HanafudadHands.Add(HanafudaHand.Tane);
                player.Score += 1;
            }
            if (player.KasuCardCollection.Count >= 10)
            {
                player.HanafudadHands.Add(HanafudaHand.Kasu);
                player.Score += 1;
            }
        }

        private bool doFinishMatch()
        {
            if(player.HanafudadHands.Count > 0)
            {
                foreach (var h in player.HanafudadHands)
                {
                    Console.WriteLine(player.HanafudadHands.Count);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SwitchTrunMaster()
        {
            if(TurnMaster == player)
            {
                TurnMaster = cpu;
            }
            else
            {
                TurnMaster = player;
            }
        }

        private void ActivateAI()
        {

        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (TurnMaster == player)
            {
                switch (phase)
                {
                    case GamePhase.SelectingHandCardPhase:
                        selectedHandCard = GetKeyOfCardsInPlayersHandCollideWithMousePointer();
                        break;
                    case GamePhase.SelectingFieldCardPhase:
                        selectedFieldCard = GetKeyOfCardsInFiledWithMousePointer();
                        break;
                    case GamePhase.SelectingOneMoreCardPhase:
                        selectedFieldCard = GetKeyOfCardsInFiledWithMousePointer();
                        break;
                    case GamePhase.SecondCheckingPhase:
                    case GamePhase.checkingPhase:
                        CheckMatchingSelectedCardMonth(selectedHandCard, selectedFieldCard);
                        break;

                }
                CheckHanafudaHand(player);
                //SwitchTrunMaster();
            }
            else
            {

            }
        }
    }
}
