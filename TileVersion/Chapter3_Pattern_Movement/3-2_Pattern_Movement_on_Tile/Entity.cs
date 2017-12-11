using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMovementForTile
{
    /// <summary>
    /// パターンムーブメントを行うエンティティ
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="init_col">entityの最初のx座標</param>
        /// <param name="init_row">entityの最初のy座標</param>
        public Entity()
        {
            row = col = -1;
            pathRow = new int[MaxPathLength];
            pathCol = new int[MaxPathLength];
            pattern = new int[MaxPathLength, MaxPathLength];
            wanderingPattern = new Tuple<int, int>[MaxPathLength];
            currentStep = 0;
            InitializePathArrays();
            InitializePatternMatrix();
        }

        /// <summary>
        /// 引数に取った座標へのpathを作成し、それをpathRow,pathColに記録
        /// </summary>
        /// <param name="startCol">出発点のx座標</param>
        /// <param name="startRow">出発点のy座標</param>
        /// <param name="goalCol">目標点のx座標</param>
        /// <param name="goalRow">目標点のy座標</param>
        public void BuildPathToTarget(int startCol, int startRow, int goalCol, int goalRow)
        {
            int deltaRow = goalRow - startRow;
            int deltaCol = goalCol - startCol;

            int stepRow = deltaRow < 0 ? -1 : 1;
            int stepCol = deltaCol < 0 ? -1 : 1;
            int sw = -stepCol * stepRow; // 誤差関数の更新用スイッチ

            deltaRow = Math.Abs(deltaRow);
            deltaCol = Math.Abs(deltaCol);
            
            int nextRow = startRow;
            int nextCol = startCol;
            int startCurrentStep = currentStep;
            pathRow[currentStep] = nextRow;
            pathCol[currentStep] = nextCol;
            currentStep++;

            Console.WriteLine("( " + startCol + ", " + startRow + ")から( " + goalCol + ", " + goalRow + ")へのPathの生成を行います...");
            if (deltaCol > deltaRow)
            {
                int fraction = -sw * 2 * deltaRow + sw * deltaCol;
                while (nextCol != goalCol)
                {
                    if (fraction > 0)
                    {
                        nextRow += sw == -1 ? stepRow : 0;
                        fraction -= deltaCol;
                    }
                    else
                    {
                        nextRow += sw == 1 ? stepRow : 0;
                        fraction += deltaCol;
                    }
                    nextCol += stepCol;
                    pathRow[currentStep] = nextRow;
                    pathCol[currentStep] = nextCol;
                    currentStep++;
                    CheckPathLength();
                    fraction += -sw * 2 * deltaRow + sw * deltaCol;
                }
            }
            else
            {
                int fraction = -sw * deltaRow + 2 * sw * deltaCol;
                while (nextRow != goalRow)
                {
                    if (fraction > 0)
                    {
                        nextCol += sw == 1 ? stepCol : 0;
                        fraction -= deltaRow;
                    }
                    else
                    {
                        nextCol += sw == -1 ? stepCol : 0;
                        fraction += deltaRow;
                    }
                    nextRow += stepRow;
                    pathRow[currentStep] = nextRow;
                    pathCol[currentStep] = nextCol;
                    currentStep++;
                    CheckPathLength();
                    fraction += -sw * deltaRow + 2 * sw * deltaCol;
                }
            }

            row = pathRow[currentStep - 1];
            col = pathCol[currentStep - 1];

            Console.Write("path : ");
            for (int i = startCurrentStep; i < currentStep; i++)
            {
                Console.Write("(" + pathCol[i] + ", " + pathRow[i] + ") ");
                if (i != currentStep - 1)
                {
                    Console.Write(" => ");
                }
            }
            Console.WriteLine();

            Console.WriteLine("現在地は(" + col + ", " + row + ")です...");

            if (goalRow == row && goalCol == col)
            {
                Console.WriteLine("pathの生成終了...");
                RegisterPattern();
            }
            else
            {
                Console.WriteLine("pathの生成ができませんでした...");
                // CannotBuildPathExceptionを投げる
                throw new CannotBuildPathException("(" + goalCol + ", " + goalRow + ")へのパスを生成できませんでした...");
            }
        }

        /// <summary>
        /// 登録したパターンでentityを徘徊させる
        /// </summary>
        public void CalcWanderingPattern(Tuple<int, int> mapSize)
        {
            for (int i = 0; i < wanderingPattern.Length; i++)
            {
                wanderingPattern[i] = Tuple.Create(-1, -1);
            }

            // entityの移動できる方向を保存するための配列
            int[] possibleRowPath = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] possibleColPath = { 0, 0, 0, 0, 0, 0, 0, 0 };

            // 行、列方向のそれぞれの変化量(ラスタ走査)
            int[] rowOffset = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] colOffset = { -1, 0, 1, -1, 1, -1, 0, 1 };

            int currentRow = pathRow[0];
            int currentCol = pathCol[0];
            int previousRow = currentRow;
            int previousCol = currentCol;

            wanderingPattern[0] = Tuple.Create(currentRow, currentCol);
            //wanderingPattern[1] = Tuple.Create(previousRow, previousCol);
            Random random = new Random();

            Console.WriteLine("徘徊ルートを求めています...");
            for (int step = 1; step < wanderingPattern.Length; step++)
            {
                int j = 0;
                int i;
                for (i = 0; i < OffsetLength; i++)
                {
                    //Console.Write("({0, 3}, {1, 3}) + Offset({2, 3}, {3, 3}) =>",currentcol, currentrow, colOffset[i], rowOffset[i]);
                    int nextRow = currentRow + rowOffset[i];
                    int nextCol = currentCol + colOffset[i];
                    //Console.WriteLine("({0, 3}, {1, 3})", nextCol, nextRow);
                    if (pattern[nextRow, nextCol] == 1)
                    {
                        if ((nextRow != previousRow) || (nextCol != previousCol))
                        {
                            //Console.WriteLine("\tj = {0} : ({1, 3}, {2, 3})", j, nextCol, currentrow);
                            possibleRowPath[j] = nextRow;
                            possibleColPath[j] = nextCol;
                            j++;
                        }
                    }             
                }
                //Console.WriteLine("j range = [0, {0}]", j);
                //foreach (var c in possibleColPath)
                //{
                //    Console.Write("{0, 3}", c);
                //}
                //Console.WriteLine("\t");
                //foreach (var r in possibleRowPath)
                //{
                //    Console.Write("{0, 3}", r);
                //}

                i = random.Next(0, j);
                //Console.WriteLine("\ni = {0}", i);
                previousCol = currentCol;
                previousRow = currentRow;

                currentRow = possibleRowPath[i];
                currentCol = possibleColPath[i];
                wanderingPattern[step] = Tuple.Create(currentRow, currentCol);

                for (int k = 0; k < possibleColPath.Length; k++)
                {
                    possibleColPath[k] = possibleRowPath[k] = 0;
                }
            }
            Console.WriteLine("徘徊ルートが決まりました...");

            Console.WriteLine("start");
            int index = 0;
            foreach (var t in wanderingPattern)
            {
                Console.WriteLine("[{0, 3}] ({1, 3}, {2, 3}) => ", index, t.Item1, t.Item2);
                index++;
            }
            Console.WriteLine("end");
        }

        public void ShowPattern(Tuple<int, int> mapSize)
        {
            if (MaxPathLength < mapSize.Item1 || MaxPathLength < mapSize.Item2)
            {
                throw new IndexOutOfRangeException("(" + mapSize.Item1 + ", " + mapSize.Item2 + ")は表示できるサイズを超えています...");
            }

            Console.WriteLine("entityのパターンを表示します...");
            for (int i = 0; i < mapSize.Item1; i++)
            {
                for (int j = 0; j < mapSize.Item2; j++)
                {
                    if (pattern[j, i] == 1)
                    {
                        Console.Write("■");
                    }
                    else if (pattern[j, i] == 0)
                    {
                        Console.Write("　");
                    }
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// pathCol, pathRowの初期化
        /// </summary>
        private void InitializePathArrays()
        {
            for (int i = 0; i < MaxPathLength; i++)
            {
                pathRow[i] = pathCol[i] = -1;
            }
        }

        /// <summary>
        /// patternの初期化
        /// </summary>
        private void InitializePatternMatrix()
        {
            for (int i = 0; i < MaxPathLength; i++)
            {
                for (int j = 0; j < MaxPathLength; j++)
                {
                    pattern[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// パスを記録できるかチェックする。内部でStackOverflowExceptionを投げる
        /// </summary>
        private void CheckPathLength()
        {
            if (currentStep == MaxPathLength)
            {
                throw new StackOverflowException("もうパスを記録できません...");
            }
        }

        /// <summary>
        /// patternにentityの通る道を記録する
        /// </summary>
        private void RegisterPattern()
        {
            Console.WriteLine("patternにpathを登録中...");
            for (int i = 0; i < MaxPathLength; i++)
            {
                if (pathCol[i] == -1 || pathRow[i] == -1)
                {
                    Console.WriteLine("pattern登録完了...");
                    return;
                }
                else
                {
                    pattern[pathRow[i], pathCol[i]] = 1;
                }
            }
        }

        private const int MaxPathLength = 100;
        private const int OffsetLength = 8;

        /// <summary>
        /// entityの現在地
        /// </summary>
        private int row, col;
        /// <summary>
        /// 現在のpathのステップ数
        /// </summary>
        private int currentStep;

        /// <summary>
        /// entityの通る道の座標
        /// </summary>
        private int[] pathRow, pathCol;

        /// <summary>
        /// entityの徘徊するパターン
        /// </summary>
        private int[,] pattern;

        /// <summary>
        /// 実際にentityの徘徊するルート
        /// </summary>
        private Tuple<int, int>[] wanderingPattern;

        /// <summary>
        /// entityが通るy座標
        /// </summary>
        public int[] PathRow { get { return pathRow; } }

        /// <summary>
        /// entityが通るx座標
        /// </summary>
        public int[] PathCol { get { return pathCol; } }

        /// <summary>
        /// entityが通る座標=1とした行列（[row, col]は(y, x)の並びである）
        /// </summary>
        public int[,] Pattern { get { return pattern; } }

        /// <summary>
        /// entityが実際に徘徊するルート(Item1, Item2)=(y, x)
        /// </summary>
        public Tuple<int, int>[] WanderingPattern { get { return wanderingPattern; } }
    }
}
