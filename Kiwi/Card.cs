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
        /// カードがどこに存在するか
        /// </summary>
        public Place whereAmI { get; set; }

        const string ResourceFilePath = "Resource/";

        Texture2D backTexture;
        Texture2D texture;
        public Card(CardsData data)
        {
            Id = data.Id;
            CardRank = (CardRank)Enum.Parse(typeof(CardRank), data.CardRank, true);
            CardSpecialRank = (CardSpecialRank)Enum.Parse(typeof(CardSpecialRank), data.CardSpecialRank, true);
            Month = data.Month;
            CardName = data.CardName;
            string filePath = ResourceFilePath + CardName + ".jpg";
            Console.WriteLine(filePath);
            Position = new Vector2F(500, 600);
            Scale = new Vector2F(0.5f, 0.5f);
            backTexture = Texture2D.LoadStrict(ResourceFilePath + "Back.jpg");
            texture = Texture2D.LoadStrict(ResourceFilePath + CardName + ".jpg");
            Texture = texture;
            Size = Texture.Size * Scale;
            whereAmI = Place.Deck;
        }

        private void InvisualizeMyselfIfIAmCPUsHand()
        {
            if(whereAmI == Place.CPUsHand)
            {
                Texture = backTexture;
            }
            else 
            {
                Texture = texture;
            }
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            InvisualizeMyselfIfIAmCPUsHand();
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
