#pragma once
#include "Class.h"

//llscript文件类，暂时只支持utf-8格式编码。
class LLScript
{
public:
	LLScript(wstring filePath);
	~LLScript();
	Parameter RunFunction(wstring functionName);
	Parameter* GetParameter(wstring pName);
	Function* GetFunction(wstring fName);
	
protected:
	bool LoadScriptFromFile();
	bool LoadUnknown(wifstream& file, Class* classptr);
	bool LoadFunction(wifstream& file, Function* functionptr);
	bool LoadClass(wifstream& file, Class* classptr);
	Parameter LoadValue(wifstream& file);
	wstring filePath = L"";
	unordered_map<wstring, Class*> classMap;
	unordered_map<wstring, Function*> functionMap;
	unordered_map<wstring, Parameter*> parameterMap;

	wstringstream wsstream;
};
