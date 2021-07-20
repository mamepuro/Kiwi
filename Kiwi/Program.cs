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
            var scene = new GameMainScene();
            Engine.AddNode(scene);
            while(Engine.DoEvents())
            {
                Engine.Update();
                if(Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
                else if(Engine.Keyboard.GetKeyState(Key.Enter) == ButtonState.Push)
                {
                    Console.WriteLine("残り" + Deck.deck.Count + "枚");
                }
            }
            Engine.Terminate();
        }
    }
}
