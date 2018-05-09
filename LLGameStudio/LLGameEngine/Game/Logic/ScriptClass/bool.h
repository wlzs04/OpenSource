#pragma once
#include "..\Class.h"

class Bool :public Class
{
public:
	Bool() :Class(L"bool") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	bool GetValue();
private:
	bool value;
};