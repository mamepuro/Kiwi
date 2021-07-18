using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Altseed2;


namespace Kiwi
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize("Kiwi", 1280, 960);
            //var c = new Card("a");
            var c = new List<Card>();
            c = GameMaker.GetCardList("../../../json1.json");
            GameMaker.DeployAllCards(c);
            while(Engine.DoEvents())
            {
                Engine.Update();
                if(Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
            }
            Engine.Terminate();
        }
    }
}
