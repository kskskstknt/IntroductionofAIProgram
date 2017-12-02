#include<stdio.h>
#include<math.h>
#include<stdlib.h>

#define MAX_PATH_LENGTH 40
#define MAX_SIZE 20
#define BLANK 0
#define WALL 1
#define PLAYER 2
#define PREDATOR 3
#define ROUTE 4

int map[MAX_SIZE + 2][MAX_SIZE + 2];

void init_map();
void build_path_to_target(int target_row, int target_col, int col, int row, int path[2][MAX_PATH_LENGTH]);
void print_path_on_map(int path[2][MAX_PATH_LENGTH], int player_row, int player_col, int predator_row, int predator_col);

int main()
{
	init_map();

	int player_col = 3;
	int player_row = 5;
	map[player_row][player_col] = PLAYER;

	int predator_col = 4;
	int predator_row = 17;
	map[predator_row][predator_col] = PREDATOR;

	int path[2][MAX_PATH_LENGTH];
	build_path_to_target(player_row, player_col, predator_col, predator_row, path);

	print_path_on_map(path, player_row, player_col, predator_row, predator_col);
	return 0;
}

void init_map()
{
	for (int i = 0; i < MAX_SIZE + 2; ++i)
	{
		for (int j = 0; j < MAX_PATH_LENGTH + 2; ++j)
		{
			map[i][j] = i == 0 || i == MAX_SIZE + 1 || j == 0 || j == MAX_SIZE + 1 ? WALL : BLANK;
		}
	}
}

void build_path_to_target(int target_row, int target_col, int col, int row, int path_col_and_row[2][MAX_PATH_LENGTH])
{
	int path_col_target;
	int path_row_target;

	// 次に動くpredatorの座標
	int next_col = col;
	int next_row = row;

	// 誤差関数の更新の正負フラグ
	int flag;

	// deltaはpredatorの進むべき方向を示すための変数
	int delta_col = target_col - col; // delta_xと同値
	int delta_row = target_row - row; // delta_yと同値
	int step_col, step_row;
	int current_step, fraction;

	// 経路の初期化
	for (current_step = 0; current_step < MAX_PATH_LENGTH; current_step++)
	{
		path_col_and_row[0][current_step] = path_col_and_row[1][current_step] = BLANK;
	}
	current_step = 0;
	path_col_target = target_col;
	path_row_target = target_row;

	// 候補座標の補助項
	// (x+step_x, y+step_y)はdelta_xとdelta_yの正負から求まる
	step_col = delta_col < 0 ? -1 : 1;
	step_row = delta_row < 0 ? -1 : 1;
	flag = (-1 * step_col) * step_row;

	printf("delta_col = %d\n", delta_col);
	printf("delta_row = %d\n", delta_row);

	printf("step_col = %2d\n", step_col);
	printf("step_row = %2d\n", step_row);
	printf("flag = %2d\n", flag);

	delta_col = abs(delta_col);
	delta_row = abs(delta_row);

	printf("|delta_col| = %d\n", delta_col);
	printf("|delta_row| = %d\n", delta_row);

	path_col_and_row[0][current_step] = next_col;
	path_col_and_row[1][current_step] = next_row;
	current_step++;

	/*
	y = (delta_y / delta_x) * (x - x0) + y0
	<=> f(x, y) = delta_y * x - delta_x * y + (delta_x * y0 - delta_y * x0)
	という線形関数からどこへ進めばいいのかを求める
	ここで、D = 2f(x, y)を誤差関数とする
	2倍することでステップを整数のまま扱うことができる
	D > 0 => playerが直線の右上領域
	D = 0 => playerが直線上
	D < 0 => playerが直線の左上領域
	*/
	if (delta_col > delta_row)
	{
		printf("delta_col > delta_row...\n");
		// D = 2 * f(中間座標)の正負で行くマスを決定
		fraction = (-1 * flag) * 2 * delta_row + flag * delta_col;
		printf("fraction[%d] = %d\n", current_step, fraction);
		while (next_col != path_col_target)
		{
			if (fraction > 0)
			{
				printf("fraction > 0...\n");
				next_row += flag == -1 ? step_row : 0;
				fraction -= delta_col;
			}
			else
			{
				printf("fraction <= 0...\n");
				next_row += flag == 1 ? step_row : 0;
				fraction += delta_col;
			}
			next_col += step_col;
			path_col_and_row[0][current_step] = next_col;
			path_col_and_row[1][current_step] = next_row;
			current_step++;
			fraction += (-1 * flag) * 2 * delta_row + flag * delta_col;
			printf("fraction[%d] = %d\n", current_step, fraction);
		}
	}
	else
	{
		printf("delta_col <= delta_row...\n");
		// D = 2 * f(中間座標)の正負で行くマスを決定
		fraction = (-1 * flag) * delta_row + 2 * flag * delta_col;
		printf("fraction[%d] = %d\n", current_step, fraction);
		while (next_row != path_row_target)
		{
			if (fraction > 0)
			{
				printf("fraction > 0...\n");
				next_col += flag == 1 ? step_col : 0;
				fraction -= delta_row;
			}
			else
			{
				printf("fraction <= 0...\n");
				next_col += flag == -1 ? step_col : 0;
				fraction += delta_row;
			}
			next_row += step_row;
			path_col_and_row[0][current_step] = next_col;
			path_col_and_row[1][current_step] = next_row;
			current_step++;
			fraction += (-1 * flag) * delta_row + 2 * flag * delta_col;
			printf("fraction[%d] = %d\n", current_step, fraction);
		}
	}
}

void print_path_on_map(int path[2][MAX_PATH_LENGTH], int player_row, int player_col, int predator_row, int predator_col)
{
	int length;
	for (length = 0; length < MAX_PATH_LENGTH; length++)
	{
		if (path[0][length] == BLANK)
		{
			break;
		}
		else
		{
			map[path[1][length]][path[0][length]] = ROUTE;
		}
	}
	map[player_row][player_col] = PLAYER;
	map[predator_row][predator_col] = PREDATOR;


	printf("print path on map...\n");
	printf("\n");
	for (int i = 0; i < MAX_SIZE + 2; i++)
	{
		for (int j = 0; j < MAX_SIZE + 2; j++)
		{
			switch (map[i][j])
			{
			case BLANK:
				printf("　");
				break;
			case WALL:
				printf("■");
				break;
			case PLAYER:
				printf("ｐ");
				break;
			case PREDATOR:
				printf("＆");
				break;
			case ROUTE:
				printf("ｘ");
				break;
			default:
				printf("？");
				break;
			}
		}
		printf("\n");
	}
	printf("\n");
	printf("player is p on (%2d, %2d)\n", player_col, player_row);
	printf("predator is & on (%2d, %2d)\n", predator_col, predator_row);
	printf("route is x\n");
}
