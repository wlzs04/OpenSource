#pragma once
#include "Common/Net/LLGameProtocol.h"

namespace MyLLGameProtocol
{
	class SStartGameProtocol : public LLGameServerProtocol
	{
	public:
		SStartGameProtocol() :LLGameServerProtocol(L"SStartGameProtocol") {}
		void Process(LLGame* ptr)override;
	};

	class CStartGameProtocol : public LLGameClientProtocol
	{
	public:
		CStartGameProtocol() :LLGameClientProtocol(L"CStartGameProtocol") {}
	};

	class SGetQipanProtocol : public LLGameServerProtocol
	{
	public:
		SGetQipanProtocol() :LLGameServerProtocol(L"SGetQipanProtocol") {}
		void Process(LLGame* ptr)override;
	};

	class CPutQiziProtocol : public LLGameClientProtocol
	{
	public:
		CPutQiziProtocol() :LLGameClientProtocol(L"CPutQiziProtocol") {}

	private:
		bool isBlack = true;
		int x = 0;
		int y = 0;
	};

	class SPutQiziProtocol : public LLGameServerProtocol
	{
	public:
		SPutQiziProtocol() :LLGameServerProtocol(L"SPutQiziProtocol") {}
		void Process(LLGame* ptr)override;
	private:
		bool isBlack = true;
		int x = 0;
		int y = 0;
	};
};

