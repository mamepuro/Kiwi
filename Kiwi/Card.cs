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

        const string ResourceFilePath = "Resource/";
        public Card(CardsData data)
        {}
        public Card(string name)
        {
            Texture = Texture2D.LoadStrict("Resource/1_H.jpg");
            Position = new Vector2F(500, 600);
            Scale = new Vector2F(0.5f, 0.5f);
            Engine.AddNode(this);
        }
    }
}
