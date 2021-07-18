using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

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
    }
}
