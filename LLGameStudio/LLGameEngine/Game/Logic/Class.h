#pragma once
#include "Function.h"

class Class
{
public:
	Class(wstring name);
	~Class();
	//只有在类被取消定义，如类所在的脚本文件被移除，才能使用。
	void RemoveClassDefine();
	virtual Class* GetInstance();
	virtual void SetValue(wstring value) {};
	virtual wstring GetValueToWString() { return wstring(); };
	wstring GetName();
	Parameter* GetParameter(wstring pName);

	//在类定义时添加类的变量
	void AddParamterDefine(Parameter* p);
	//在类定义时添加类的变量
	void AddFunctionDefine(Function* f);

	Function* GetFunction(wstring fName);
	virtual Parameter Add(Class* classptr);
	virtual Parameter Subtract(Class* classptr);
	virtual Parameter Multiple(Class* classptr);
	virtual Parameter Divide(Class* classptr);
	//取余
	virtual Parameter Complementation(Class* classptr);
	//交集
	virtual Parameter Intersection(Class* classptr);
	//并集
	virtual Parameter Union (Class* classptr);
	//大于
	virtual Parameter Greater(Class* classptr);
	//小于
	virtual Parameter Less(Class* classptr);
	//等于
	virtual Parameter Equal(Class* classptr);
	//不等于
	virtual Parameter UnEqual(Class* classptr);
protected:
	wstring name;//类名
	unordered_map<wstring, Parameter*> parameterMap;
	unordered_map<wstring, Function*> functionMap;

};