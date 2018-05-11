#pragma once
#include "..\Class.h"

class Int:public Class
{
public:
	Int() :Class(L"int") {}
	Class* GetInstance() override;
	void SetValue(int value);
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	int GetValue();
	Parameter Add(Class* classptr)override;
	Parameter Subtract(Class* classptr)override;
	Parameter Multiple(Class* classptr) override;
	Parameter Divide(Class* classptr) override;

	Parameter Complementation(Class* classptr) override;


	Parameter Greater(Class* classptr) override;
	Parameter Less(Class* classptr) override;
	Parameter Equal(Class* classptr) override;
	Parameter UnEqual(Class* classptr) override;
private:
	int value;
};