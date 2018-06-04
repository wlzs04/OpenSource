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
	if (encryptClass != nullptr)
	{
		delete encryptClass;
	}

	WSACleanup();
	connectThread.join();
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

void LLGameNetClient::StartConnect(wstring ipport)
{
	int mhi = ipport.find(L':');
	wstring ip = ipport.substr(0, mhi);
	wstring portstr = ipport.substr(mhi+1);
	int port = WStringHelper::GetInt(portstr);
	StartConnect(ip, port);
}

void LLGameNetClient::AcceptProtocol()
{
	getProtocolThread = thread([this]() {
		while (connecting)
		{
			char szBuffer4[4] = { 0 };
			int netState;
			int protocolLength = 0;
			recv(clientSocket, szBuffer4, 4, NULL);
			protocolLength = atoi(szBuffer4);
			unsigned char* szBuffer = new unsigned char[protocolLength];
			netState = recv(clientSocket, (char*)szBuffer, protocolLength, NULL);

			if (netState > 0)
			{
				string strContent = DecodeProtocol(string((char*)szBuffer, 0, netState));
				wstring content = WStringHelper::UTF8BufferToWString(strContent);

				wstring name = content.substr(0, content.find(L' '));
				LLGameServerProtocol* protocol = legalProtocolMap[name]->GetInstance();

				protocol->LoadContentFromWString(content);
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

std::string LLGameNetClient::EncryptProtocol(string sBuffer)
{
	if (encryptClass != nullptr)
	{
		sBuffer = encryptClass->Encrypt(sBuffer);
	}
	char ss[10];
	sprintf_s(ss, "%04d", sBuffer.size());
	return ss + sBuffer;
}

std::string LLGameNetClient::DecodeProtocol(string sBuffer)
{
	if (encryptClass != nullptr)
	{
		sBuffer = encryptClass->Decode(sBuffer);
	}
	return sBuffer;
}

bool LLGameNetClient::SendProtocol(LLGameProtocol protocol)
{
	int returnCode = 0;
	if (connecting)
	{
		string content = WStringHelper::WStringToUTF8Buffer(protocol.ExportContentToWString());
		content = EncryptProtocol(content);
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

void LLGameNetClient::AddLegalProtocol(LLGameServerProtocol* protocol)
{
	legalProtocolMap[protocol->GetName()] = protocol;
}

void LLGameNetClient::SetEncryptClass(IEncryptClass* encryptClass)
{
	this->encryptClass = encryptClass;
}
