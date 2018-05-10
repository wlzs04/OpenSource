#pragma once
#include "..\Class.h"

class Float :public Class
{
public:
	Float() :Class(L"float") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	float GetValue();
	void Add(Class* classptr)override;
	void Subtract(Class* classptr)override;
	void Multiple(Class* classptr)override;
	void Divide(Class* classptr)override;

private:
	float value;
};