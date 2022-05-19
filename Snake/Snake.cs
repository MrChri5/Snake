using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Snake
    {
        private int[][] snakeBody;
        private Direction headDirection;
        private int screenWidth = 0;
        private int screenHeight = 0;

        public Snake(int[][] body, Direction direction, int ScreenWidth, int ScreenHeight)
        {
            //set screen dimensions to find max snake length
            screenWidth = ScreenWidth;
            screenHeight = ScreenHeight;
            snakeBody = new int[screenWidth*screenHeight][];
            //set starting body parts 
            snakeBody[0] = new int[] {1,1};
            int i = 0;
            foreach(int[] seg in body)
            {
                if (seg.Length == 2)
                {
                    snakeBody[i] = seg;
                    i++;
                }
            }
            //set starting direction
            headDirection = direction;           
        }

        //return info about snake location and direction
        public int[][] Body
        {
            get { return snakeBody; }
        }
        public int[] Head
        {
            get { return Body[0]; }
        }
        public Direction HeadDirection
        {
            get { return headDirection; }
        }

        //input directions and method to add them to coordinates
        public enum Direction {Up, Down, Left, Right};
        public int[] AddDir(int[] start, Direction direction)
        {
            //if invalid return input
            if (start == null || start.Length != 2)
            {
                return start;
            }
            int[] returnPos = new int[2];
            
            //calculate new coordinate
            switch (direction)
            {
                case Direction.Up:
                    returnPos = new int[2]{ start[0], start[1] + 1 };
                    break;
                case Direction.Down:
                    returnPos = new int[2] { start[0], start[1] - 1 };
                    break;
                case Direction.Left:
                    returnPos = new int[2] { start[0] -1 , start[1] };
                    break;
                case Direction.Right:
                    returnPos = new int[2] { start[0] + 1, start[1] };
                    break;
                default:
                    return start;
            }
            //loop coordinates round edges of screen
            if (screenWidth != 0 && returnPos[0]>= screenWidth)
            {
                returnPos[0] -= screenWidth;
            }
            if (screenWidth !=0 && returnPos[0] < 0)
            {
                returnPos[0] += screenWidth;
            }
            if (screenHeight !=0 && returnPos[1] >= screenHeight)
            {
                returnPos[1] -= screenHeight;
            }
            if (screenHeight !=0 && returnPos[1] < 0)
            {
                returnPos[1] += screenHeight;
            }
            return returnPos;
        }

        //default eatApple to false if no flag passed
        public void MoveSnake (Direction direction)
        {
            MoveSnake(direction, false);
        }
        public void MoveSnake (Direction direction, bool eatApple)
        {
            //if input direction is opposite of current direction 
            //continue in current direction
            if(HeadDirection == Direction.Up && direction == Direction.Down 
                || HeadDirection == Direction.Down && direction == Direction.Up
                || HeadDirection == Direction.Left && direction == Direction.Right
                || HeadDirection == Direction.Right && direction == Direction.Left)
            {
                direction = HeadDirection;
            }

            //move body parts along one
            for(int i = snakeBody.Length - 1; i >0; i--)
            {
                //ignore null parts
                if (snakeBody[i-1] != null)
                {
                    //skip moving tail unless flag set to get longer
                    if (eatApple)
                    {
                        snakeBody[i] = snakeBody[i-1];
                    }
                    //set eatApple flag so remaining body parts are moved along
                    eatApple = true;
                }
            }
            //set head location and direction using direction passed
            snakeBody[0] = AddDir(snakeBody[1], direction);
            headDirection = direction;            
        }


    }
}
