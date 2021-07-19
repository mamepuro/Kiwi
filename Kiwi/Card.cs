using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

namespace Kiwi
{
    class Card : SpriteNode
    {
        /// <summary>
        /// カードID
        /// </summary>
        private int Id { get; set; }
        public CardRank CardRank { get; set; }
        public CardSpecialRank CardSpecialRank{ get; set; }
        public string CardName { get; set; }
        public int Month { get; set; }
        /// <summary>
        /// テクスチャーのスケールを考慮した場合のサイズ
        /// </summary>
        public Vector2F Size { get; set; }
        /// <summary>
        /// 場にこのカードが存在するかどうか(山札や取り札にあるときは偽の値を取る)
        /// </summary>
        public bool isInField { get; set; }

        const string ResourceFilePath = "Resource/";
        public Card(CardsData data)
        {
            Id = data.Id;
            CardRank = (CardRank)Enum.Parse(typeof(CardRank), data.CardRank, true);
            CardSpecialRank = (CardSpecialRank)Enum.Parse(typeof(CardSpecialRank), data.CardSpecialRank, true);
            Month = data.Month;
            CardName = data.CardName;
            string filePath = ResourceFilePath + CardName + ".jpg";
            Console.WriteLine(filePath);
            Texture = Texture2D.LoadStrict(ResourceFilePath + CardName + ".jpg");
            Position = new Vector2F(500, 600);
            Scale = new Vector2F(0.5f, 0.5f);
            Size = Texture.Size * Scale;
            isInField = false;
        }

        /*デバック用
        public Card(string name)
        {
            Texture = Texture2D.LoadStrict("Resource/1_H.jpg");
            Position = new Vector2F(500, 600);
            Scale = new Vector2F(0.5f, 0.5f);
            Engine.AddNode(this);
        }*/
    }
}
