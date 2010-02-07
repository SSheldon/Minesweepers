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
	static const int height = 9;
	static const int width = 9;
	static const int mines = 9;

public:
	Tile tiles[height][width];

public: Field()
	{
		GenerateMines();
		AssignTileNumbers();
	}

private:
	void GenerateMines()
	{
		int randomNumbers[mines];
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
			tiles[randomNumbers[i] / width][randomNumbers[i] % width].mined = true;
		}
	}
	void AssignTileNumbers()
	{
		for (int row = 0; row < height; row++)
		{
			for (int col = 0; col < width; col++)
			{
				tiles[row][col].number = TileNumber(row, col);
			}
		}
	}
	int TileNumber(int row, int col)
	{
		int accumulator = 0;
        if (row != 0 && tiles[row - 1][col].mined)
            accumulator++;
        if (col != 0 && tiles[row][col - 1].mined)
            accumulator++;
        if (row != 0 && col != 0 && tiles[row - 1][col - 1].mined)
            accumulator++;
        if (col != width - 1 && tiles[row][col + 1].mined)
            accumulator++;
        if (row != 0 && col != width - 1 && tiles[row - 1][col + 1].mined)
            accumulator++;
        if (row != height - 1 && tiles[row + 1][col].mined)
            accumulator++;
        if (row != height - 1 && col != 0 && tiles[row + 1][col - 1].mined)
            accumulator++;
        if (row != height - 1 && col != width - 1 && tiles[row + 1][col + 1].mined)
            accumulator++;
        return accumulator;
	}

public:
	bool Click(int row, int col)
    {
        tiles[row][col].Reveal();
        //check to see if there are zero tiles touching hidden tiles
        bool checkAgain;
        do
        {
            checkAgain = false;
            for (int rowCounter = 0; rowCounter < height; rowCounter++)
            {
                for (int colCounter = 0; colCounter < width; colCounter++)
                {
                    if (tiles[rowCounter][colCounter].revealed && !tiles[rowCounter][colCounter].mined &&
                        tiles[rowCounter][colCounter].number == 0 && TouchingHiddenTile(rowCounter, colCounter))
                    {
                        RevealTouching(rowCounter, colCounter);
                        checkAgain = true;
                    }
                }
            }
        } while (checkAgain);
        return tiles[row][col].mined && !tiles[row][col].flagged;
    }
	bool TouchingHiddenTile(int row, int col)
    {
        bool anyHidden = false;
        if (row != 0) 
            if (!tiles[row - 1][col].revealed && !tiles[row - 1][col].flagged) anyHidden = true;
        if (col != 0) 
            if (!tiles[row][col - 1].revealed && !tiles[row][col - 1].flagged) anyHidden = true;
        if (row != 0 && col != 0)
            if (!tiles[row - 1][col - 1].revealed && !tiles[row - 1][col - 1].flagged) anyHidden = true;
        if (col != width - 1)
            if (!tiles[row][col + 1].revealed && !tiles[row][col + 1].flagged) anyHidden = true;
        if (row != 0 && col != width - 1)
            if (!tiles[row - 1][col + 1].revealed && !tiles[row - 1][col + 1].flagged) anyHidden = true;
        if (row != height - 1)
            if (!tiles[row + 1][col].revealed && !tiles[row + 1][col].flagged) anyHidden = true;
        if (row != height - 1 && col != 0)
            if (!tiles[row + 1][col - 1].revealed && !tiles[row + 1][col - 1].flagged) anyHidden = true;
        if (row != height - 1 && col != width - 1)
            if (!tiles[row + 1][col + 1].revealed && !tiles[row + 1][col + 1].flagged) anyHidden = true;
        return anyHidden;
    }
	void RevealTouching(int row, int col)
    {
        if (row != 0)
            tiles[row - 1][col].Reveal();
        if (col != 0)
            tiles[row][col - 1].Reveal();
        if (row != 0 && col != 0)
            tiles[row - 1][col - 1].Reveal();
        if (col != width - 1)
            tiles[row][col + 1].Reveal();
        if (row != 0 && col != width - 1)
            tiles[row - 1][col + 1].Reveal();
        if (row != height - 1)
            tiles[row + 1][col].Reveal();
        if (row != height - 1 && col != 0)
            tiles[row + 1][col - 1].Reveal();
        if (row != height - 1 && col != width - 1)
            tiles[row + 1][col + 1].Reveal();
    }
	bool AllUnminedRevealed()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (!tiles[row][col].mined && !tiles[row][col].revealed) return false;
            }
        }
        return true;
    }
};