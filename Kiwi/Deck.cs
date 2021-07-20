using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Altseed2;

namespace Kiwi
{
    /// <summary>
    /// 山札を表示するためのクラス
    /// </summary>
    class Deck:SpriteNode
    {
        /// <summary>
        /// 山札の実体
        /// </summary>
        public static List<Card> deck = new List<Card>();
        public static Vector2F pos = new Vector2F(100, 400);
        private GameMainScene mainNode;
        public Deck(GameMainScene scene)
        {
            Texture = Texture2D.LoadStrict("Resource/Back.jpg");
            Position = pos;
            Scale = new Vector2F(0.5f, 0.5f);
            mainNode = scene;
            mainNode.AddChildNode(this);
        }
    }
}
