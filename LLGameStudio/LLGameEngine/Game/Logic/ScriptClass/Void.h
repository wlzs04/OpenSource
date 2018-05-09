#pragma once
#include "..\Class.h"

class Void : public Class
{
public:
	Void() :Class(L"void") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
};