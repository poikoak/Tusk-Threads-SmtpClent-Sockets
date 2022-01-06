#include "WordsDictionaryMaker.h"
#include <iostream>
#include <time.h>
#include <fstream>

using namespace std;

WordsFrequencyDictionary* WordsDictionaryMaker::GetDictionary()
{
	return dictionary;
}

void WordsDictionaryMaker::SetDictionary(WordsFrequencyDictionary* _dictionary)
{
	if (dictionary)
		delete dictionary;

	dictionary = _dictionary;
}

void WordsDictionaryMaker::Process(std::string filename)
{
	cout << dictionary->GetTitle() << " started..." << endl;

	string word;
	ifstream file1(filename);

	clock_t t = clock();
	if (file1.is_open())
	{
		while (file1 >> word)
		{
			// ןמכטלמנפםûי גûחמג ג סכמגאנו
			dictionary->Add(word);
		}
		file1.close();
	}
	t = clock() - t;
	double time_taken = ((double)t) / CLOCKS_PER_SEC;

	cout << dictionary->GetTitle() << " finished." << endl;
	cout << "Words count: " << dictionary->getSize() << endl;
	//cout << "Time taken: " << time_taken << endl << endl;
}

void WordsDictionaryMaker::Save(std::string filename)
{
	ofstream file1(filename);

	if (file1.is_open())
	{
		dictionary->save(file1);
		file1.close();
	}
}
