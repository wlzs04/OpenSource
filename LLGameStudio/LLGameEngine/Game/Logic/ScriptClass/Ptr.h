#pragma once
#include "..\Class.h"

class Ptr :public Class
{
public:
	Ptr() :Class(L"*") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	Class* GetValue();
private:
	Class* classPtr = nullptr;
};