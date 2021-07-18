using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using Altseed2;

namespace Kiwi
{
    /// <summary>
    /// ゲームを作るロジックをまとめた静的クラス
    /// </summary>
    static class GameMaker
    {
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

        public static void DeployAllCards(List<Card> cards)
        {
            int counter = 0;
            Vector2F initPos = new Vector2F(100, 50);
            foreach(var c in cards)
            {
                Vector2F size = c.Texture.Size * c.Scale;
                int x = counter % 8;
                int y = counter / 8;
                c.Position = initPos + (size * new Vector2F(x, y));
                if(c.Month <= 12)
                {
                    Engine.AddNode(c);
                }
                counter++;
            }
        }
    }
}
