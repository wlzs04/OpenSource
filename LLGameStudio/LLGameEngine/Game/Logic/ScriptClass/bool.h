#pragma once
#include "..\Class.h"

class Bool :public Class
{
public:
	Bool() :Class(L"bool") {}
	Class* GetInstance() override;
	void SetValue(bool value);
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	bool GetValue();

	Parameter Intersection(Class* classptr) override;
	Parameter Union(Class* classptr) override;

private:
	bool value;
};