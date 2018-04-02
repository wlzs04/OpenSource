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

class SGetQipanProtocol : public LLGameProtocol
{
public:
	void Process(void* ptr)override;

private:
};

class CPutQiziProtocol : public LLGameProtocol
{
public:
	CPutQiziProtocol(bool isBlack,int x,int y);

private:
	bool isBlack = true;
	int x = 0;
	int y = 0;
};

class SPutQiziProtocol : public LLGameProtocol
{
public:
	SPutQiziProtocol(bool isBlack, int x, int y);
	void Process(void* ptr)override;
private:
	bool isBlack = true;
	int x = 0;
	int y = 0;
};