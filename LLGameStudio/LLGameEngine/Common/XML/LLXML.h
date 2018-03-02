#pragma once
#include <fstream>
#include <string>
#include <codecvt>
#include <unordered_map>
#include <stack>
#include <sstream>

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
	wstring GetName();
	void AddProperty(LLXMLProperty* llProperty);
	void RemoveProperty(wstring name);
	LLXMLProperty* GetProperty(wstring name);
	void SetInnerText(wstring innerText);
	wstring GetInnerText();
	void AddNode(LLXMLNode* llNode);
	void RemoveNode(LLXMLNode* llNode);
	unordered_map<wstring, LLXMLProperty*>& GetPropertyMap();
	list<LLXMLNode*>& GetChildNodeList();
private:
	wstring name;
	wstring innerText;
	unordered_map<wstring, LLXMLProperty*> propertyMap;
	list<LLXMLNode*> childNodeList;
};

enum class FileEncode
{
	UNKNOWN,//未知编码
	ANSI,//ANSI编码
	UTF_8_WITH_BOM,//UTF_8使用BOM标记
	UTF_8_NO_BOM,//UTF_8无BOM标记
	UTF_16_LITTLE_ENDIAN,//UTF_16小端，低字节在前,高字节在后
	UTF_16_BIG_ENDIAN//UTF_16大端，高字节在前,低字节在后
};

class LLXMLDocument
{
public:
	bool LoadXMLFromFile(wstring filePath, FileEncode fileEncode = FileEncode::UNKNOWN);
	bool SaveXMLToFile(wstring filePath, FileEncode fileEncode= FileEncode::UTF_8_WITH_BOM,bool writeDefine=false);
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
	wstring LoadPropertyValue(wchar_t*& fileBuffer, int& bufferSize);//加载属性值
	wstring FormatWStringFromXML(wstring ws);
	wstring FormatWStringToXML(wstring ws);

	void SaveNode(wofstream& file, LLXMLNode* node,int depth);
	void SaveProperty(wofstream& file, LLXMLProperty* llProperty);

	LLXMLNode* rootNode;
	stack<LLXMLNode*> nodeStack;
	wstringstream wsstream;
};