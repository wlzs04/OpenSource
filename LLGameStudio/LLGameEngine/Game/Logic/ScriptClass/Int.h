#pragma once
#include "..\Class.h"

class Int:public Class
{
public:
	Int() :Class(L"int") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	int GetValue();
	void Add(Class* classptr)override;
	void Subtract(Class* classptr)override;
	void Multiple(Class* classptr) override;
	void Divide(Class* classptr) override;

	virtual void Complementation(Class* classptr) override;

private:
	int value;
};