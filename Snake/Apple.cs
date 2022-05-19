using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Apple
    {
        private int appleX = 0;
        private int appleY = 0;

        /// <include file='Snake.doc' path='docs/members[@name="Snake"]/Apple/*'/>
        public Apple(char[,] map, int seed)
        {
            Random rnd;
            //generate random coordinate that is a free space
            //required when first generating all apples to stop
            //them being generated all in a group;
            if (seed == 0)
            {
                rnd = new Random();
            }
            else
            {
                rnd = new Random(seed);
            }                      

            int index = rnd.Next(map.Length);

            for(int i =0; i < map.GetLength(0); i++)
            {
                for(int j =0; j < map.GetLength(1); j++)
                {
                    if(map[i,j] == 0)
                    {
                        index--;
                    }
                    if(index == 0)
                    {
                        appleX = i;
                        appleY = j;
                        return;
                    }
                }
            }         
        }
        /// <include file='Snake.doc' path='docs/members[@name="Snake"]/X/*'/>
        //return coordinates
        public int X
        {
            get { return appleX; }
        }
        /// <include file='Snake.doc' path='docs/members[@name="Snake"]/Y/*'/>
        public int Y
        {
            get { return appleY; }
        }
        /// <include file='Snake.doc' path='docs/members[@name="Snake"]/Location/*'/>
        public int[] Location
        {
            get
            {
                int[] loc = new int[] { appleX, appleY };
                return loc;
            }
        }

    }
}
