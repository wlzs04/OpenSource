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

	virtual void Intersection(Class* classptr) override;
	virtual void Union(Class* classptr) override;

private:
	bool value;
};