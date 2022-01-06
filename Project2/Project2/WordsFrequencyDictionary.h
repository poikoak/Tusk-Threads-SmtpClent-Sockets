#pragma once

#include <iostream>

using namespace std;

struct Word
{
	string word;
	unsigned int frequency = 1;

	Word(string _word) : word(_word) {}
};

class WordsFrequencyDictionary
{
protected:
	string title = "";
public:
	WordsFrequencyDictionary(string _title) : title(_title) {}
	virtual void Add(string word) = 0;
	virtual size_t getSize() = 0;
	virtual void save(ostream& os) = 0;
	string GetTitle() { return title; };
};

