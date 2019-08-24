#include "loader.h"

#include <string>
#include <fstream>
#include <iostream>

using std::ifstream;
using std::ios_base;

std::string load(std::string fileName)
{
	ifstream fileIn;
	fileIn.open(fileName, ios_base::in);

	std::string ret = "";
	std::string buffer;
	while (std::getline(fileIn, buffer))
	{
		ret += (buffer + "\n").c_str();
	}

	fileIn.close();
	return ret;
}

