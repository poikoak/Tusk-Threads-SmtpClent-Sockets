#pragma once

#include <map>
#include "WordsFrequencyDictionary.h"

using namespace std;

class MapFrequencyDictionary : public WordsFrequencyDictionary
{
	map<string, int> words;

public:
	MapFrequencyDictionary() : WordsFrequencyDictionary("MapFrequencyDictionary") {}

	void Add(string word);
	size_t getSize();
	void save(ostream& os);
};

