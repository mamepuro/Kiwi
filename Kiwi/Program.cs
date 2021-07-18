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
            c = GameMaker.Shuffle(c);
            int counter = 0;
            var initPos = new Vector2F(100, 50);
            //GameMaker.DeployAllCards(c);
            while(Engine.DoEvents())
            {
                Engine.Update();
                if(Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
                else if(Engine.Keyboard.GetKeyState(Key.Enter) == ButtonState.Push)
                {
                    if(c.Count > 0)
                    {
                        var card= c[0];
                        Vector2F size = card.Texture.Size * card.Scale;
                        int x = counter % 8;
                        int y = counter / 8;
                        card.Position = initPos + (size * new Vector2F(x, y));
                        Engine.AddNode(card);
                        c.RemoveAt(0);
                        counter++;
                    }
                }
            }
            Engine.Terminate();
        }
    }
}
