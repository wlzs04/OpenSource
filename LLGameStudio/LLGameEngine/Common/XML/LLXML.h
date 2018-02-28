#pragma once
#include <fstream>
#include <string>
#include <codecvt>
#include <unordered_map>
#include <stack>

using namespace std;

class LLXMLProperty
{
public:
	LLXMLProperty(wstring name);
	void SetValue(wstring value);
	wstring GetName();
	wstring GetValue();
	int GetValueInt();
	float GetValueFloat();
	bool GetValueBool();
private:
	wstring name;
	wstring value;
};

class LLXMLNode
{
public:
	LLXMLNode(wstring name);
	void AddProperty(LLXMLProperty* llProperty);
	void AddNode(LLXMLNode* llNode);
	wstring GetName();
private:
	wstring name;
	unordered_map<wstring, LLXMLProperty*> propertyMap;
	list<LLXMLNode*> childNodeList;
};

enum class FileEncode
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
	bool LoadXMLFromFile(wstring filePath);
	bool SaveXMLToFile(wstring filePath);
	LLXMLNode* GetRootNode();
private:
	FileEncode CheckFileEncode(wifstream& file);
	bool WCharCanIgnore(wchar_t wc);
	bool WCharIsLegalNameStart(wchar_t wc);
	bool WCharIsLegalName(wchar_t wc);
	bool LoadUnknown(wchar_t*& fileBuffer,int& bufferSize);//不知道接下来的内容,用于判断。
	bool LoadDefine(wchar_t*& fileBuffer, int& bufferSize);//加载声明
	bool LoadComment(wchar_t*& fileBuffer, int& bufferSize);//加载注释
	bool LoadNode(wchar_t*& fileBuffer, int& bufferSize);//加载节点
	bool LoadProperty(wchar_t*& fileBuffer, int& bufferSize);//加载属性
	wstring LoadFormatValue(wchar_t*& fileBuffer, int& bufferSize);//加载属性值

	LLXMLNode* rootNode;
	stack<LLXMLNode*> nodeStack;
};