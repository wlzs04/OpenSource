#pragma once
#include "..\..\Common\XML\LLXML.h"

class Class;

class Parameter
{
public:
	Parameter() {};
	Parameter(wstring className);
	Parameter(wstring className,wstring name);
	Parameter(const Parameter& p);
	Parameter& operator=(const Parameter& p);
	~Parameter();
	//获得变量值的指针，方便操作，但不建议使用
	Class* GetClassPtr();
	//从另一个变量中拷贝内容
	void CopyClass(Parameter& p);
	//获得变量名
	wstring GetName() const;
	//获得变量内容，目前只有基础类型才有效。
	wstring GetValueToWString() const;
	//设置变量内容，目前只有基础类型才有效。
	void SetValue(wstring value);
	//设置变量类名，只有在类还没设置类型时才有效。
	void SetClassName(wstring className);
	//获得变量类名
	wstring GetClassName() const;
	//根据运算符作相应算法
	void DoFunctionByoperator(wchar_t wc, Parameter& p);
private:
	Class* classPtr = nullptr;
	wstring name = L"p1";
};