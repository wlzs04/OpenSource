#pragma once
#include "Parameter.h"

class LLScript;

class Function
{
public:
	Function(wstring name,wstring returnClassName,LLScript* scriptPtr,Class* classptr);
	Parameter Run(vector<Parameter>* inputList = nullptr);
	void SetContent(wstring content);
protected:
	unordered_map<wstring, Parameter*> localParameterMap;
	vector<Parameter> inputParameterList;

	Class* classPtr = nullptr;
	LLScript* scriptPtr = nullptr;
	wstring name = L"";
	wstring returnClassName = L"int";
	wstring content = L"";
private:
	//寻找有效的变量，顺序为方法内变量，类变量，全局变量。
	Parameter* GetParameter(wstring pName);
	//获得右侧的值当作临时变量。
	Parameter GetTempValue(wistringstream& wsstream);
	//获得等号右侧的值。
	//Parameter dhyc(wistringstream& wsstream);
	//脚本语言语法：是否是可忽略字符
	bool WCharCanIgnore(wchar_t wc);
	//脚本语言语法：是否为特殊符号，用来截断读取的标记
	bool WCharSpecial(wchar_t wc);
	//脚本语言语法：是否为运算符号
	bool WCharIsOperator(wchar_t wc);
	wistringstream  wsstream;
	
};
