using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using Altseed2;
using System.Linq;

namespace Kiwi
{
    /// <summary>
    /// ゲームを作る・管理するロジックをまとめた静的クラス
    /// </summary>
    static class GameOperator
    {
        //static List<Card> cardsInFiled = new List<Card>();
        public static Dictionary<int, Card> cardsInField = new Dictionary<int, Card>();
        private static string GetJsonFileAllLine(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string allLine = reader.ReadToEnd();
                    return allLine;
                }
            }
            catch(IOException e)
            {
                throw new FileNotFoundException("File not Found.", e);
            }
        }
        public static List<Card> GetCardList(string filePath)
        {
            var cards = new List<Card>();
            string json = GetJsonFileAllLine(filePath);
            CardsData[] cardsData = JsonSerializer.Deserialize<CardsData[]>(json);
            foreach(var c in cardsData)
            {
                var card = new Card(c);
                cards.Add(card);
            }
            return cards;
        }

        public static List<Card> Shuffle(List<Card> deck)
        {
            var newList = new List<Card>();
            var rnd = new Random();
            var shuffledCards = deck.OrderBy(item => rnd.Next());
            foreach(var v in shuffledCards)
            {
                newList.Add(v);
            }
            return newList;
        }

        /// <summary>
        /// 場を作ります、ゲームを開始するときのみ呼ばれます
        /// </summary>
        /// <param name="deck">山札</param>
        public static void MakeFiled(List<Card> deck)
        {
            var margin = new Vector2F(30, 30);
            var initPos = new Vector2F(800, 500);
            int filledCards = 8;
            for(int i = 0;i<filledCards;i++)
            {
                var top = deck[0];
                Vector2F cardSize = top.Texture.Size * top.Scale;
                int x = i / 2;
                int y = i % 2;
                var pos = initPos - ((cardSize + margin) * new Vector2F(x, y));
                PlayCardFromDeck(deck, pos,i);

            }
        }

        /// <summary>
        /// 山札から場にカードを出します
        /// </summary>
        /// <param name="deck">山札を表すカードが格納されたリスト</param>
        /// <param name="fieldPos">場のどの位置にカードを出すか</param>
        /// <param name="fieldKey">カードをどのキーの値のところに配置するか</param>
        public static void PlayCardFromDeck(List<Card> deck, Vector2F fieldPos, int fieldKey)
        {
            deck[0].isInField = true;
            //GameOperator.cardsInFiled.Add(deck[0]);
            GameOperator.cardsInField.Add(fieldKey, deck[0]);
            deck[0].Position = fieldPos;
            Engine.AddNode(deck[0]);
            deck.RemoveAt(0);
        }
        /// <summary>
        /// デバック用コード　すべての花札を8*6で一覧表示する
        /// </summary>
        /// <param name="deck"></param>
        public static void DeployAllCards(List<Card> deck)
        {
            int counter = 0;
            Vector2F initPos = new Vector2F(100, 50);
            foreach(var c in deck)
            {
                Vector2F size = c.Texture.Size * c.Scale;
                int x = counter % 8;
                int y = counter / 8;
                c.Position = initPos + (c.Size * new Vector2F(x, y));
                c.isInField = true;
                if(c.Month <= 12)
                {
                    Engine.AddNode(c);
                }
                counter++;
            }
        }
    }
}
