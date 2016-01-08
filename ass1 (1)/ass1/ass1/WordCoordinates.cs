using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ass1
{
    /// <summary>
    /// Class for storing the end coordinates of a word and its orientation
    /// </summary>
    public class WordCoordinates
    {
        public string word;
        public int x, y; // X and Y axis for last alphabet of the word
        public bool horizontal;
        public WordCoordinates(string Word, int X, int Y, bool hor)
        {
            word = Word;
            x = X;
            y = Y;
            horizontal = hor;
        }
    }
}
