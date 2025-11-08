using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Function
{
    internal class ColumnIndexManager
    {
        // A - Z 의 Coul Index 관리.. A = 1

        private int minIndexOfAscii;
        private int maxIndexOfAscii;

        public Dictionary<char, int> defaultIndexes;

        public ColumnIndexManager()
        {
            minIndexOfAscii = 'A';
            maxIndexOfAscii = 'Z';

            SetDefaltIndex();
        }

        public void SetDefaltIndex()
        {
            var indexmap = new Dictionary<char, int>();

            var diff = maxIndexOfAscii - minIndexOfAscii;
            for (int i = 0; i <= diff; i++)
            {
                var c = (char)(minIndexOfAscii + i);

                indexmap.Add(c, i + 1);
            }

            defaultIndexes = indexmap;
        }

        public int ColNameToIndex(char colName)
        {
            var indexOfAscii = (int)colName;

            if (indexOfAscii < minIndexOfAscii || maxIndexOfAscii < indexOfAscii)
            {
                return 0;
            }

            return defaultIndexes[colName];
        }

        public char IndexToColName(int colIndex)
        {
            return defaultIndexes.FirstOrDefault(f => f.Value == colIndex).Key;
        }
    }
}
