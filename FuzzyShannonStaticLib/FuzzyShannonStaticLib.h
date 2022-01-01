#pragma once
#include <string>
class LibFuzzyShannon
{
private:

public:

	void setByte(int byte);
	double Shannon(const char* filepath);
	std::string computehash(const char* filepath);
	int comparehash(std::string hash1, std::string hash2);

};