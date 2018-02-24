#pragma once
#include <fstream>
#include <list>
#include <string>

using namespace std;

class LLXMLNode
{

};

class LLXMLProperty
{

};

enum FileEncode
{
	ANSI,//ANSI编码
	UTF_8_WITH_BOM,//UTF_8使用BOM标记
	UTF_8_NO_BOM,//UTF_8无BOM标记
	UTF_16_LITTLE_ENDIAN,//UTF_16小端，低字节在前,高字节在后
	UTF_16_BIG_ENDIAN//UTF_16大端，高字节在前,低字节在后
};

class LLXMLDocument
{
public:
	LLXMLDocument();
	~LLXMLDocument();

	bool LoadXMLFromFile(wstring filePath);
	bool SaveXMLToFile(wstring filePath);
	FileEncode CheckFileEncode(wifstream& file);
private:
	LLXMLNode rootNode;
};

