#include <cstdlib>
#include <time.h>

struct Tile
{
	int number;
	bool mined;
	bool revealed;
	bool flagged;

	Tile()
	{
		number = 0;
		mined = false;
		revealed = false;
		flagged = false;
	}

	void Reveal()
	{
		if (!flagged) revealed = true;
	}
};

class Field
{
private:
	int height;
	int width;
	int mines;
	Tile *tiles;

public:
	Field(int height, int width, int mines)
	{
		this->height = height;
		this->width = width;
		this->mines = mines;
		tiles = new Tile[height * width];
		GenerateMines();
		AssignTileNumbers();
	}
	~Field()
	{
		delete[] tiles;
	}
	Tile Get(int row, int col)
	{
		return tiles[row * width + col];
	}

private:
	void GenerateMines()
	{
		int *randomNumbers = new int[mines];
		srand((unsigned)time(NULL));
		for (int i = 0; i < mines; i++)
		{
			bool alreadyContained;
			do
			{
				alreadyContained = false;
				int randomNumber = (rand() % (height * width - 1));
				for (int j = 0; j < i; j++)
				{
					if (randomNumbers[j] == randomNumber)
					{
						alreadyContained = true;
						break;
					}
				}
				if (!alreadyContained) randomNumbers[i] = randomNumber;
			} while (alreadyContained);
		}
		for (int i = 0; i < mines; i++)
		{
			tiles[randomNumbers[i]].mined = true;
		}
		delete[] randomNumbers;
	}
	void AssignTileNumbers()
	{
		for (int i = 0; i < height * width; i++)
		{
			tiles[i].number = TileNumber(i / width, i % width);
		}
	}
	int TileNumber(int row, int col)
	{
		int accumulator = 0;
		if (row != 0 && Get(row - 1, col).mined)
			accumulator++;
		if (col != 0 && Get(row, col - 1).mined)
			accumulator++;
		if (row != 0 && col != 0 && Get(row - 1, col - 1).mined)
			accumulator++;
		if (col != width - 1 && Get(row, col + 1).mined)
			accumulator++;
		if (row != 0 && col != width - 1 && Get(row - 1, col + 1).mined)
			accumulator++;
		if (row != height - 1 && Get(row + 1, col).mined)
			accumulator++;
		if (row != height - 1 && col != 0 && Get(row + 1, col - 1).mined)
			accumulator++;
		if (row != height - 1 && col != width - 1 && Get(row + 1, col + 1).mined)
			accumulator++;
		return accumulator;
	}

public:
	bool Click(int row, int col)
	{
		Get(row, col).Reveal();
		//check to see if there are zero tiles touching hidden tiles
		bool checkAgain;
		do
		{
			checkAgain = false;
			for (int rowCounter = 0; rowCounter < height; rowCounter++)
			{
				for (int colCounter = 0; colCounter < width; colCounter++)
				{
					if (Get(rowCounter, colCounter).revealed && !Get(rowCounter, colCounter).mined &&
						Get(rowCounter, colCounter).number == 0 && TouchingHiddenTile(rowCounter, colCounter))
					{
						RevealTouching(rowCounter, colCounter);
						checkAgain = true;
					}
				}
			}
		} while (checkAgain);
		return Get(row, col).mined && !Get(row, col).flagged;
	}
	bool TouchingHiddenTile(int row, int col)
	{
		bool anyHidden = false;
		if (row != 0) 
			if (!Get(row - 1, col).revealed && !Get(row - 1, col).flagged) anyHidden = true;
		if (col != 0) 
			if (!Get(row, col - 1).revealed && !Get(row, col - 1).flagged) anyHidden = true;
		if (row != 0 && col != 0)
			if (!Get(row - 1, col - 1).revealed && !Get(row - 1, col - 1).flagged) anyHidden = true;
		if (col != width - 1)
			if (!Get(row, col + 1).revealed && !Get(row, col + 1).flagged) anyHidden = true;
		if (row != 0 && col != width - 1)
			if (!Get(row - 1, col + 1).revealed && !Get(row - 1, col + 1).flagged) anyHidden = true;
		if (row != height - 1)
			if (!Get(row + 1, col).revealed && !Get(row + 1, col).flagged) anyHidden = true;
		if (row != height - 1 && col != 0)
			if (!Get(row + 1, col - 1).revealed && !Get(row + 1, col - 1).flagged) anyHidden = true;
		if (row != height - 1 && col != width - 1)
			if (!Get(row + 1, col + 1).revealed && !Get(row + 1, col + 1).flagged) anyHidden = true;
		return anyHidden;
	}
	void RevealTouching(int row, int col)
	{
		if (row != 0)
			Get(row - 1, col).Reveal();
		if (col != 0)
			Get(row, col - 1).Reveal();
		if (row != 0 && col != 0)
			Get(row - 1, col - 1).Reveal();
		if (col != width - 1)
			Get(row, col + 1).Reveal();
		if (row != 0 && col != width - 1)
			Get(row - 1, col + 1).Reveal();
		if (row != height - 1)
			Get(row + 1, col).Reveal();
		if (row != height - 1 && col != 0)
			Get(row + 1, col - 1).Reveal();
		if (row != height - 1 && col != width - 1)
			Get(row + 1, col + 1).Reveal();
	}
	bool AllUnminedRevealed()
	{
		for (int row = 0; row < height; row++)
		{
			for (int col = 0; col < width; col++)
			{
				if (!Get(row, col).mined && !Get(row, col).revealed) return false;
			}
		}
		return true;
	}
};