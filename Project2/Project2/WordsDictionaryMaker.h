#pragma once

#include <string>
#include "WordsFrequencyDictionary.h"

class WordsDictionaryMaker
{
protected:
	WordsFrequencyDictionary* dictionary = nullptr;
public:
	WordsDictionaryMaker(WordsFrequencyDictionary* _dictionary) : dictionary(_dictionary) {}
	~WordsDictionaryMaker() { if (dictionary) delete dictionary; }

	WordsFrequencyDictionary* GetDictionary();
	void SetDictionary(WordsFrequencyDictionary* _dictionary);
	void Process(std::string filename);
	void Save(std::string filename);
};