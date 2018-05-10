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
	Parameter returnP; 
private:
	//寻找有效的变量，顺序为方法内变量，类变量，全局变量。
	Parameter* GetParameter(wstring pName);
	//获得右侧的值当作临时变量。
	Parameter GetTempValue(wstringstream& wsstream);
	//获得等号右侧的值。
	//Parameter dhyc(wistringstream& wsstream);
	Function* GetFunction(wstring fName);
	//在作用域中运行，大括号包围内的内容为一个作用域
	void RunInSpace(wstringstream& wsstream);
	//跳过作用域，大括号包围内的内容为一个作用域
	void JumpOverSpace(wstringstream& wsstream);
	wstringstream  wsstream;
	wostringstream  valueStream;
	
};
