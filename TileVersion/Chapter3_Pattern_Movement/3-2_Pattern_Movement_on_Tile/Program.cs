using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMovementForTile
{
    class Program
    {
        private static readonly Tuple<int, int> MapSize = new Tuple<int, int>(32, 42);
   
        private static readonly String Blank = "　";
        private static readonly String Wall  = "■";
        private static readonly String Route = "×";

        public static String[,] map = new string[MapSize.Item1, MapSize.Item2];

        static void Main(string[] args)
        {
            // entity1 : 簡単な周遊ルート
            InitializeMap();
            Entity entity1 = new Entity();
            entity1.BuildPathToTarget(10, 3, 18, 3);
            entity1.BuildPathToTarget(18, 3, 18, 12);
            entity1.BuildPathToTarget(18, 12, 10, 12);
            entity1.BuildPathToTarget(10, 12, 10, 3);

            UpdateMap(entity1.PathCol, entity1.PathRow);            
            ShowMap();

            // entity2 : 複雑な周遊ルート
            InitializeMap();
            Entity entity2 = new Entity();
            entity2.BuildPathToTarget(4, 2, 4, 11);
            entity2.BuildPathToTarget(4, 11, 2, 24);
            entity2.BuildPathToTarget(2, 24, 13, 27);
            entity2.BuildPathToTarget(13, 27, 16, 24);
            entity2.BuildPathToTarget(16, 24, 13, 17);
            entity2.BuildPathToTarget(13, 17, 13, 13);
            entity2.BuildPathToTarget(13, 13, 17, 5);
            entity2.BuildPathToTarget(17, 5, 4, 2);

            entity2.ShowPattern(MapSize);

            UpdateMap(entity2.PathCol, entity2.PathRow);
            ShowMap();

            entity2.CalcWanderingPattern(MapSize);
            InitializeMap();
            UpdateMapByWanderingPattern(entity2.WanderingPattern);
            ShowMap();

            // entity3 : 簡単な周遊ルートだが、分岐点がある
            InitializeMap();
            Entity entity3 = new Entity();
            entity3.BuildPathToTarget(3, 2, 16, 2);
            entity3.BuildPathToTarget(16, 2, 16, 11);
            entity3.BuildPathToTarget(16, 11, 9, 11);
            entity3.BuildPathToTarget(9, 11, 9, 2);
            entity3.BuildPathToTarget(9, 6, 3, 6);
            entity3.BuildPathToTarget(3, 6, 3, 2);

            entity3.ShowPattern(MapSize);

            UpdateMap(entity3.PathCol, entity3.PathRow);
            ShowMap();

            entity3.CalcWanderingPattern(MapSize);
            InitializeMap();
            UpdateMapByWanderingPattern(entity3.WanderingPattern);
            ShowMap();

        }

        private static void ShowMap()
        {
            Console.WriteLine("Entityのルートの表示を行います...");
            for (int i = 0; i < MapSize.Item1; i++)
            {
                for (int j = 0; j < MapSize.Item2; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static void InitializeMap()
        {
            for (int i = 0; i < MapSize.Item1; i++)
            {
                for (int j = 0; j < MapSize.Item2; j++)
                {
                    if (i == 0 || i == MapSize.Item1 - 1 || j == 0 || j == MapSize.Item2 - 1)
                    {
                        map[i, j] = Wall;
                    }
                    else
                    {
                        map[i, j] = Blank;
                    }
                }
            }
            Console.WriteLine("マップの初期化終了...");
        }

        private static void UpdateMap(int[] pathCol, int[] pathRow) 
        {
            Console.WriteLine("pathの反映を行います...");
            for (int k = 0; k < pathCol.Length; k++)
            {
                if (pathRow[k] != -1 && pathCol[k] != -1)
                {
                    map[pathCol[k], pathRow[k]] = Route;
                    //Console.WriteLine(k + " : ( " + pathCol[k] + ", " + pathRow[k] + " )");
                }
            }
            Console.WriteLine("pathの反映完了しました...");
        }

        private static void UpdateMapByWanderingPattern(Tuple<int, int>[] patternArray)
        {
            Console.WriteLine("pathの反映を行います...");
            for (int k = 0; k < patternArray.Length; k++)
            {
                if (patternArray[k].Item1 != -1 && patternArray[k].Item2 != -1)
                {
                    map[patternArray[k].Item2, patternArray[k].Item1] = Route;
                    //Console.WriteLine(k + " : ( " + pathCol[k] + ", " + pathRow[k] + " )");
                }
            }
            Console.WriteLine("pathの反映完了しました...");
        }
    }
}
