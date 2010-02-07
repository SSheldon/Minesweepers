#include<iostream>
#include "Field.cpp"

using namespace std;

int main()
{
	Field field;
	for (int i = 0; i < 9; i++)
	{
		for (int j = 0; j < 9; j++)
		{
			if (field.tiles[i][j].mined)
				cout << "X ";
			else
				cout << field.tiles[i][j].number << " ";
		}
		cout << endl;
	}
	int a;
	cin >> a;
	return 0;
}