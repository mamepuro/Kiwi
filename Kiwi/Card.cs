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
    }
}
