#pragma once
#include "Common/Net/LLGameProtocol.h"

class SStartGameProtocol : public LLGameProtocol
{
public:
	void Process(void* ptr)override;
};

class CStartGameProtocol : public LLGameProtocol
{
public:
};