#include "MapFrequencyDictionary.h"
#include <set>

void MapFrequencyDictionary::Add(string word)
{
	if (words.find(word) != words.end())
		words[word]++;
	else
		words[word] = 1;
}

size_t MapFrequencyDictionary::getSize()
{
	return words.size();
}

void MapFrequencyDictionary::save(ostream& os)
{
	/*for (map<string, int>::iterator i = words.begin(); i != words.end(); i++) {
		os << (*i).first << ":\t" << (*i).second << "\n";
	}*/
	set<pair<int,string>> s;  // The new (temporary) container.

	for (auto const& kv : words)
		s.emplace(kv.second, kv.first);  // Flip the pairs.

	for (auto const& vk : s)		
		os << vk.first << ":\t" << vk.second << "\n";
	
}
