#include <WinSock2.h>
#include <WS2tcpip.h>
#include <errno.h>
#include "LLGameNetClient.h"
#include "..\Helper\WStringHelper.h"

LLGameNetClient::LLGameNetClient()
{
	WSADATA wsaData;
	WSAStartup(MAKEWORD(2, 2), &wsaData);
}

LLGameNetClient::~LLGameNetClient()
{
	WSACleanup();
}

void LLGameNetClient::StartConnect(wstring ip, int port)
{
	if (connecting)
	{
		return;
	}
	this->serverIP = ip;
	this->serverPort = port;
	clientSocket = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	connectThread = thread([this]() {
		sockaddr_in sockAddr;
		memset(&sockAddr, 0, sizeof(sockAddr));
		sockAddr.sin_family = PF_INET;

		string utf8Buffer = WStringHelper::WStringToUTF8Buffer(serverIP);
		inet_pton(PF_INET, utf8Buffer.c_str(), &sockAddr.sin_addr);
		sockAddr.sin_port = htons(serverPort);
		int error = connect(clientSocket, (sockaddr*)&sockAddr, sizeof(sockaddr));

		if (error == SOCKET_ERROR)
		{
			if (OnConnectFailHandle) { OnConnectFailHandle(); }
		}
		else
		{
			connecting = true;
			if (OnConnectSuccessHandle) { OnConnectSuccessHandle(); }
			AcceptProtocol();
		}
	});
}

void LLGameNetClient::AcceptProtocol()
{
	getProtocolThread = thread([this]() {
		while (connecting)
		{
			char szBuffer[MAXBYTE] = { 0 };
			int netState;
			netState = recv(clientSocket, szBuffer, MAXBYTE, NULL);

			if (netState > 0)
			{
				LLGameProtocol protocol;

				protocol.SetContent(WStringHelper::UTF8BufferToWString(string(szBuffer,0,netState)));
				if (OnProcessProtocolHandle)
				{
					OnProcessProtocolHandle(protocol);
				}
			}
			else
			{
				connecting = false;
				if (OnDisconnectHandle)
				{
					OnDisconnectHandle();
				}
			}
		}
		closesocket(clientSocket);
	});
}

bool LLGameNetClient::SendProtocol(LLGameProtocol protocol)
{
	int returnCode = 0;
	if (connecting)
	{
		string content = WStringHelper::WStringToUTF8Buffer(protocol.GetContent());
		returnCode = send(clientSocket, content.c_str(), content.length(), 0);
	}
	return connecting && returnCode != SOCKET_ERROR;
}

void LLGameNetClient::StopConnect()
{
	if (connecting)
	{
		closesocket(clientSocket);
	}
	connecting = false;
}
