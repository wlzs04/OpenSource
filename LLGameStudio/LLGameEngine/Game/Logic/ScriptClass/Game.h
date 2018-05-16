#pragma once
#include "..\Class.h"

class Game :public Class
{
public:
	Game() ;
	Game* GetInstance() override;
	void AddCppFunction(Function* functionPtr);
private:

};
