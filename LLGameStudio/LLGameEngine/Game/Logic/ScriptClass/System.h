#pragma once
#include "..\Class.h"

class System :public Class
{
public:
	System() ;
	Class* GetInstance() override;
private:
	Parameter ShowMessage(vector<Parameter>* inputList=nullptr);
	Parameter GetNowTime(vector<Parameter>* inputList = nullptr);
	Parameter LoadScriptFile(vector<Parameter>* inputList = nullptr);
	Parameter UnloadScriptFile(vector<Parameter>* inputList = nullptr);
};