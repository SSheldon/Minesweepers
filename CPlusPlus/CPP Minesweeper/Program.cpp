#include<iostream>
#include "Field.cpp"

using namespace std;

int main()
{
	Field field(9, 9, 10);
	for (int i = 0; i < 9; i++)
	{
		for (int j = 0; j < 9; j++)
		{
			if (field.Get(i, j).mined)
				cout << "X ";
			else
				cout << field.Get(i, j).number << " ";
		}
		cout << endl;
	}
	int a;
	cin >> a;
	return 0;
}