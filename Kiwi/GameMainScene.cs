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
        public GameMainScene()
        {

        }

        /// <summary>
        /// クリックされたときに場に出ているカードと衝突判定を行う
        /// </summary>
        /// <returns>衝突している場合は衝突したカードと対応するキー値を、何も衝突していなければ-1が返ります</returns>
        public int GetKeyOfCardsInFiledWithMousePointer()
        {
            if(Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push)
            {
                var clickedPos = Engine.Mouse.Position;
                bool isCollide = false;
                foreach(var item in GameOperator.cardsInField)
                {
                    var card = item.Value;
                    var edgePos = card.Position + card.Size;
                    var cardPos = card.Position;
                    if(clickedPos.X >= cardPos.X && 
                        clickedPos.X <= edgePos.X &&
                        clickedPos.Y >= cardPos.Y &&
                        clickedPos.Y <= edgePos.Y)
                    {
                        Console.WriteLine("花札" + item.Key + "と衝突した");
                        return item.Key;
                    }
                }
            }
            return -1;
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            GetKeyOfCardsInFiledWithMousePointer();
        }
    }
}
