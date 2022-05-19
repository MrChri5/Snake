using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Game
    {

        Snake snake;
        Apple[] apples;
        int width = 0;
        int height = 0;
        int score = 0;
        char[,] map;

        public Game (int inputWidth, int inputHeight)
        {
            //set width and height
            width = inputWidth;
            height = inputHeight;
            map = new char[width, height];
 
        }

        public void Level (int mapNum, int numApples)
        {
            int freeSpaces = (width * height)-1; 
            //build map layout
            switch (mapNum)
            {
                //## rooms, include one blocked from inside
                //## pacman maze
                //## 
                case 5:
                    // random boulders
                  
                    int rndXnum = 0;
                    int rndYnum = 0;
                    Random rndX;
                    Random rndY;

                    for (int i = 0; i < width / 2; i++)
                    {
                        if (rndXnum != 0)
                        {
                            rndX = new Random(rndYnum * i);
                            rndY = new Random(rndXnum + i*i);
                        }
                        else
                        {
                            rndX = new Random();
                            rndY = new Random();
                        }
                        rndXnum = rndX.Next(width-1);
                        rndYnum = rndY.Next(height-1);
                        if (rndYnum == height/3 || rndYnum == (height/3) -1)
                        {
                            i--;
                            continue;
                        }
                        if (map[rndXnum,rndYnum] == 0)
                        {
                            map[rndXnum, rndYnum] = '#';
                            freeSpaces -= 1;
                        }
                        if (map[rndXnum, rndYnum +1] == 0)
                        {
                            map[rndXnum, rndYnum +1] = '#';
                            freeSpaces -= 1;
                        }
                        if (map[rndXnum +1, rndYnum] == 0)
                        {
                            map[rndXnum +1, rndYnum] = '#';
                            freeSpaces -= 1;
                        }
                        if (map[rndXnum +1, rndYnum +1] == 0)
                        {
                            map[rndXnum +1, rndYnum +1] = '#';
                            freeSpaces -= 1;
                        }


                    }             
                    break;

                case 4:
                    // X
                    for (int i = 0; i < (width /2) - 2 && i < (height / 2) - 2; i++)
                    {
                        map[width / 2 - 1 - i, height / 2 - 1 - i] = '/';
                        map[width / 2 - 1 - i, height / 2 + i] = '\\';
                        map[width / 2 + i, height / 2 - 1 - i] = '\\';
                        map[width / 2 + i, height / 2 + i] = '/';
                        freeSpaces -= 4;
                    }
                    for (int i = 0; i < height; i++)
                    {
                        map[0, i] = '│';
                        map[width - 1, i] = '│';
                        freeSpaces -= 2;
                    }
                    break;

                case 3:
                    //cross
                    for(int i = width / 6; i < (5 * width) / 6; i++)
                    {
                        if (i==width / 2)
                        {
                            for(int j = height/6; j<(5*height)/6; j++)
                            {
                                if (j == height / 2)
                                {
                                    map[i, j] = '┼';
                                }
                                else
                                {
                                    map[i,j] = '│';
                                }
                                freeSpaces--;
                            }
                        }
                        else
                        {
                            map[i, height /2] = '─';
                            freeSpaces--;
                        }
                    }
                    break;

                case 2:
                    //none
                    break;

                case 1:
                default:
                    //standard box                                     
                    map[0, height - 1] = '┌';
                    map[width - 1, height - 1] = '┐';
                    map[0, 0] = '└';
                    map[width - 1, 0] = '┘';
                    freeSpaces -= 4;
                    for (int i = 1; i < width - 1; i++)
                    {
                        map[i, 0] = '─';
                        map[i, height - 1] = '─';
                        freeSpaces -= 2;
                    }
                    for (int i = 1; i < height - 1; i++)
                    {
                        map[0, i] = '│';
                        map[width - 1, i] = '│';
                        freeSpaces -= 2;
                    }
                    break;
            }

            //set default starting snake in middle of screen
            if (mapNum >= 1 && mapNum <=5)
            {
                int wFactor = 2;
                int hFactor = 2;
                if(mapNum == 3 || mapNum == 5)
                {
                    //for cross map, spawn in corner
                    wFactor = hFactor = 3;
                }
                if (mapNum == 4)
                {
                    wFactor = 4;
                    hFactor = 2;
                }
                int[][] initBody = new int[][]
                {
                new int[]{ (width/wFactor) + 1 , (height/hFactor)},
                new int[]{ width/wFactor, (height/hFactor)},
                new int[]{ (width/wFactor) - 1, (height/hFactor)},
                new int[]{ (width/wFactor) - 2, (height/hFactor)},
                new int[]{ (width/wFactor) - 3, (height/hFactor)},
                };
                snake = new Snake(initBody, Snake.Direction.Right, width, height);
            }


            //generate apples and initialize apple counter
            score = 0;
            if (numApples <= 0)
            {
                numApples = 1;
            }
            if (numApples > freeSpaces)
            {
                numApples = freeSpaces;
            }
            apples = new Apple[numApples];

            //create an array that has the map, head, and 
            //exisiting apples to find an empty space.
            char[,] checkSpace = new char [width, height];
            Array.Copy(map, checkSpace, map.Length);  
            checkSpace[snake.Head[0], snake.Head[1]] = 'X';
            for (int i =0; i < numApples; i++)
            {
                apples[i] = new Apple(checkSpace, i);
                checkSpace[apples[i].X,apples[i].Y] = '@';
            }
        }

        //input method for converting string input to moves
        public char[,] Input(string input)
        {
            if (input.Length < 1)
            {
                input = " ";
            }
            switch (input[0])
            {
                case 'W':
                case 'w':
                    Move(Snake.Direction.Up);
                    break;
                case 'A':
                case 'a':
                    Move(Snake.Direction.Left);
                    break;
                case 'S':
                case 's':
                    Move(Snake.Direction.Down);
                    break;
                case 'D':
                case 'd':
                    Move(Snake.Direction.Right);
                    break;
                //if blank or invalid input, continue in current direction
                default:
                    Move(snake.HeadDirection);
                    break;
            }
            //return matrix with updated screen
            return Render();
        }

        //work out if snake has eaten apple and call MoveSnake with correct eatApple flag
        //generate new apple if last one was eaten
        public void Move(Snake.Direction dir)
        {
            int eaten = -1;
            //create an array that has the map, head, and 
            //exisiting apples to find an empty space.
            char[,] checkSpace = new char[width,height];
            Array.Copy(map, checkSpace, map.Length);
            checkSpace[snake.Head[0], snake.Head[1]] = 'X';            
            
            for (int i = 0; i < apples.Length; i++)
            {
                if (eaten == -1 && snake.AddDir(snake.Head, dir).SequenceEqual(apples[i].Location))
                {
                    eaten = i;  
                }
                else
                {    
                    checkSpace[apples[i].X, apples[i].Y] = '@';
                }
            }
            //if not eaten, move normally
            if (eaten == -1)
            {
                snake.MoveSnake(dir);
            }
            else
            {
                //eat apple, generate a new one, and increase score
                snake.MoveSnake(dir, true);
                apples[eaten] = new Apple (checkSpace,0);
                score++;
                
            }            
        }

        //render screen
        public enum RenderMode{Normal,Start,GameOver1,GameOver2,ChooseMap,ChooseApples,ChooseSpeed};
        public char[,] Render()
        {
            return Render(RenderMode.Normal);
        }
        public char[,] Render(RenderMode mode)
        {
            char[,] renderGame = new char[width, height];
            int widthOffset = 0;

            switch (mode)
            {
                case RenderMode.Normal:
                    //copy map layout
                    Array.Copy(map, renderGame, map.Length);
                    //build snake 
                    for (int i = 0; i < snake.Body.Length; i++)
                    {
                        int[] seg = snake.Body[i];
                        //skip null parts
                        if (seg == null)
                        {
                            continue;
                        }
                        //Head
                        if (seg == snake.Head)
                        {
                            renderGame[seg[0], seg[1]] = 'X';
                        }
                        //tail
                        else if (i == snake.Body.Length - 1 || snake.Body[i + 1] == null)
                        {
                            //up tail
                            if (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up)))
                            {
                                renderGame[seg[0], seg[1]] = 'V';
                            }
                            //down tail
                            if (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down)))
                            {
                                renderGame[seg[0], seg[1]] = '^';
                            }
                            //left tail
                            if (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left)))
                            {
                                renderGame[seg[0], seg[1]] = '>';
                            }
                            //right tail
                            if (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right)))
                            {
                                renderGame[seg[0], seg[1]] = '<';
                            }
                        }
                        //vertical body
                        else if ((snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))) ||
                            (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))))
                        {
                            renderGame[seg[0], seg[1]] = 'N';
                        }
                        //horizontal body
                        else if ((snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) ||
                          (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))))
                        {
                            renderGame[seg[0], seg[1]] = 'Z';
                        }
                        //top-left and bottom-right diagonal body
                        else if ((snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))) ||
                          (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))) ||
                          (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) ||
                          (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))))
                        {
                            renderGame[seg[0], seg[1]] = '/';
                        }
                        //top-right and bottom-left diagonal body
                        else if ((snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) ||
                            (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Right))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Up))) ||
                            (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))) ||
                            (snake.Body[i - 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Left))) && (snake.Body[i + 1].SequenceEqual(snake.AddDir(seg, Snake.Direction.Down))))
                        {
                            renderGame[seg[0], seg[1]] = '\\';
                        }
                    }

                    if (apples != null)
                    {
                        //build apples
                        for (int i = 0; i < apples.Length; i++)
                        {
                            renderGame[apples[i].X, apples[i].Y] = '@';
                        }
                    }           
                    break;

                case RenderMode.Start:
                    //write start instructions
                    string[] instruction1 = { "Press W/A/S/D to move the snake.",
                                          "      Press ENTER to start.     ",
                                          "       Input EXIT to exit.      "};
                    widthOffset = 1 + (width - 2 - instruction1[0].Length) / 2;
                    for (int i = 0; i < instruction1[0].Length; i++)
                    {
                        renderGame[i + widthOffset, height - 2] = instruction1[0][i];
                        renderGame[i + widthOffset, height - 4] = instruction1[1][i];
                        renderGame[i + widthOffset, height - 6] = instruction1[2][i];
                    }
                    goto default;

                case RenderMode.GameOver1:
                case RenderMode.GameOver2:
                    //write game over on screen
                    string[] gameover = {
                    " GGGG   A   M   M EEEEE",
                    "G      A A  MM MM E    ",
                    "G  GG AAAAA M M M EEE  ",
                    "G   G A   A M   M E    ",
                    " GGGG A   A M   M EEEEE",

                    " OOO  V   V EEEEE RRRR ",
                    "O   O V   V E     R   R",
                    "O   O V   V EEE   RRRR ",
                    "O   O  V V  E     R  R ",
                    " OOO    V   EEEEE R   R",

                    "Number of apples eaten:",

                    "Input START to restart."};

                    string printScore = score.ToString();
                    widthOffset = 1 + (width - 2 - gameover[0].Length) / 2;

                    for (int i = 0; i < 23; i++)
                    {
                        renderGame[i + widthOffset, height - 3] = gameover[0][i];
                        renderGame[i + widthOffset, height - 4] = gameover[1][i];
                        renderGame[i + widthOffset, height - 5] = gameover[2][i];
                        renderGame[i + widthOffset, height - 6] = gameover[3][i];
                        renderGame[i + widthOffset, height - 7] = gameover[4][i];
                        renderGame[i + widthOffset, height - 9] = gameover[5][i];
                        renderGame[i + widthOffset, height - 10] = gameover[6][i];
                        renderGame[i + widthOffset, height - 11] = gameover[7][i];
                        renderGame[i + widthOffset, height - 12] = gameover[8][i];
                        renderGame[i + widthOffset, height - 13] = gameover[9][i];
                        if (mode == RenderMode.GameOver2)
                        {
                            renderGame[i + widthOffset, height - 15] = gameover[10][i];
                            renderGame[i + widthOffset, height - 19] = gameover[11][i];
                        }
                    }

                    //print score
                    if (mode == RenderMode.GameOver2)
                    {
                        for (int i = 0; i < printScore.Length; i++)
                        {
                            renderGame[i + 10 + widthOffset, height - 17] = printScore[i];
                        }
                    }
                    goto default;

                case RenderMode.ChooseMap:
                    //build screen to choose the map layout
                    string[] instruction2 = { "Input a map layout:",
                                          "  1) Box           ",
                                          "  2) None          ",
                                          "  3) Cross         ",
                                          "  4) X             ",
                                          "  5) Boulders      "};
                    widthOffset = 1 + (width - 2 - instruction2[0].Length) / 2;
                    for (int i = 0; i < instruction2[0].Length; i++)
                    {
                        renderGame[i + widthOffset, height - 2] = instruction2[0][i];
                        renderGame[i + widthOffset, height - 4] = instruction2[1][i];
                        renderGame[i + widthOffset, height - 5] = instruction2[2][i];
                        renderGame[i + widthOffset, height - 6] = instruction2[3][i];
                        renderGame[i + widthOffset, height - 7] = instruction2[4][i];
                        renderGame[i + widthOffset, height - 8] = instruction2[5][i];
                    }
                    goto default;

                case RenderMode.ChooseApples:
                    //build screen to choose the number of apples
                    string[] instruction3 = { "Input number of apples." };
                    widthOffset = 1 + (width - 2 - instruction3[0].Length) / 2;
                    for (int i = 0; i < instruction3[0].Length; i++)
                    {
                        renderGame[i + widthOffset, height - 2] = instruction3[0][i];
                    }
                    goto default;

                case RenderMode.ChooseSpeed:
                    //build screen to choose speed
                    string[] instruction4 = { "Input speed (1 - 5)." };
                    widthOffset = 1 + (width - 2 - instruction4[0].Length) / 2;
                    for (int i = 0; i < instruction4[0].Length; i++)
                    {
                        renderGame[i + widthOffset, height - 2] = instruction4[0][i];
                    }
                    goto default;

                default:
                    //build frame for menu screens
                    renderGame[0, height - 1] = '┌';
                    renderGame[width - 1, height - 1] = '┐';
                    renderGame[0, 0] = '└';
                    renderGame[width - 1, 0] = '┘';
                    for (int i = 1; i < width - 1; i++)
                    {
                        renderGame[i, 0] = '─';
                        renderGame[i, height - 1] = '─';
                    }
                    for (int i = 1; i < height - 1; i++)
                    {
                        renderGame[0, i] = '│';
                        renderGame[width - 1, i] = '│';
                    }
                    break;
            }       
            
            return renderGame; 
        }

        //work out if game is over
        public bool GameOver
        {
            get
            {
                bool gameOver = false;
                bool nullSeg = false;
                
                for(int i = 1; i < snake.Body.Length; i++)
                {
                    //check if there any any spaces left in snake.Body array
                    if (snake.Body[i] == null)
                    {
                        nullSeg = true;
                        continue;
                    }
                    //check if the snake has eaten part of itself
                    if (snake.Head.SequenceEqual(snake.Body[i]))
                    {
                        gameOver = true;
                    }
                }
                //check the snake has hit the map
                if(map[snake.Head[0],snake.Head[1]] != 0)
                {
                    gameOver = true;
                }
                //check if snake has reached maximum size
                if (!nullSeg)
                {
                    gameOver = true;
                }
                return gameOver;
                
            }
        }             
    }
}
