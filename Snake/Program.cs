using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {

        static Game game;
        static bool timerOn = false;
        static Timer timer;    

        static void Main(string[] args)
        {
            //set console width to 1/3 of screen and create new game
            //set minimum 25x18
            int height = Console.LargestWindowHeight / 3;
            int minHeight = 18;
            int width = Console.LargestWindowWidth / 3;
            int minwidth = 25;
            if (height < minHeight)
            {
                height = minHeight;
            }
            if (width < minwidth)
            {
                width = minwidth;
            }
            Console.BufferHeight = Console.WindowHeight = height + 1;
            Console.BufferWidth = Console.WindowWidth = width + 1;

            string input = " ";
            char menuMode = ' ';
            int prevMap = 1;
            int prevApples = 1;
            int prevSpeed = 1;

            //display start screen
            game = new Game(width, height);
            Render(game.Render(Game.RenderMode.Start));
            menuMode = '1';

            //only process when user presses enter. exit if exit is typed.
            while ((input = Console.ReadLine().ToUpper()) != null && input != "EXIT")
            {
                switch (menuMode)
                {
                    case 'O':
                        //start new game
                        if (input == "START")
                        {
                            goto case 'R';
                        }
                        break;
                    case 'R':
                        //if game was reset, redisplay start screen
                        game = new Game(width, height);
                        Render(game.Render(Game.RenderMode.Start));
                        menuMode = '1';
                        break;

                    case '1':
                        //display screen to choose map layout
                        Render(game.Render(Game.RenderMode.ChooseMap));
                        menuMode = '2';
                        break;

                    case '2':
                        //set map layout and display screen to choose num apples
                        int.TryParse(input, out prevMap);                
                        Render(game.Render(Game.RenderMode.ChooseApples));
                        menuMode = '3';
                        break;

                    case '3':
                        //set number of apples and display screen to choose speed
                        int.TryParse(input, out prevApples);
                        game.Level(prevMap, prevApples);
                        Render(game.Render(Game.RenderMode.ChooseSpeed));
                        menuMode = '4';
                        break;

                    case '4':
                        //set speed and start game
                        int.TryParse(input, out prevSpeed);
                        if(prevSpeed <= 0)
                        {
                            prevSpeed = 1;
                        }
                        Render(game.Render(Game.RenderMode.Normal));
                        menuMode = ' ';
                        if (!timerOn)
                        {
                            timerOn = true;
                            timer = new Timer(InputMove, 0, 0, (1000/prevSpeed));
                            while (timerOn)
                            {
                                Thread.Sleep(3000);
                            }
                            menuMode = 'O';
                            Render(game.Render(Game.RenderMode.GameOver2));
                        }
                        break;

                    default:                         
                        break;
                }
            }
        }

        static void InputMove(object state)
        {
            char input = ' ';
            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).KeyChar;
            }
            char[,] screen = game.Input(input.ToString());
            if(game.GameOver)
            {
                screen = game.Render(Game.RenderMode.GameOver1);              
                timer.Dispose();
                timerOn = false;
            }
            Render(screen);
        }

        static void Render(char[,] render)
        {
            string output = "";
            //write to console
            for (int j = render.GetLength(1)-1; j >= 0; j--)
            {
                output += "\n";
                for (int i = 0; i<render.GetLength(0); i++)
                {
                    output += render[i,j];
                }                
            }
            Console.WriteLine(output);
        }
    }
}
