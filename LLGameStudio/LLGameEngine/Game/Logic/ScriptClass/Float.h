#pragma once
#include "..\Class.h"

class Float :public Class
{
public:
	Float() :Class(L"float") {}
	Class* GetInstance() override;
	void SetValue(float value);
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	float GetValue();
	Parameter Add(Class* classptr)override;
	Parameter Subtract(Class* classptr)override;
	Parameter Multiple(Class* classptr)override;
	Parameter Divide(Class* classptr)override;

	Parameter Greater(Class* classptr) override;
	Parameter Less(Class* classptr) override;
	Parameter Equal(Class* classptr) override;
	Parameter UnEqual(Class* classptr) override;

private:
	float value;
};