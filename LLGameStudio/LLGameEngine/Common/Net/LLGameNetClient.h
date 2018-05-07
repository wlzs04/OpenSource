#pragma once
#include <string>
#include <functional>
#include <thread>
#include "LLGameProtocol.h"
#pragma comment(lib, "ws2_32.lib")
#include "..\Encrypt\IEncryptClass.h"

using namespace std;

class LLGameNetClient
{
public:
	LLGameNetClient();
	~LLGameNetClient();
	//连接指定IP端口，非阻塞,连接成功后会自动调用协议处理方法。
	void StartConnect(wstring ip, int port);
	void StartConnect(wstring ipport);
	//发送协议
	bool SendProtocol(LLGameProtocol protocol);
	//停止连接
	void StopConnect();
	//添加合法的协议
	void AddLegalProtocol(LLGameServerProtocol* protocol);
	//设置进行加密解密的算法类
	void SetEncryptClass(IEncryptClass* encryptClass);
	//连接成功事件
	function<void()> OnConnectSuccessHandle;
	//连接失败事件
	function<void()> OnConnectFailHandle;
	//连接断开事件
	function<void()> OnDisconnectHandle;
	//处理协议事件
	function<void(LLGameServerProtocol*)> OnProcessProtocolHandle;
private:
	//接收协议，使用前需实现OnProcessProtocolHandle处理协议方法，非阻塞。
	void AcceptProtocol();
	//对协议进行加密,并附加协议长度
	string EncryptProtocol(string sBuffer);
	//对协议进行解密
	string DecodeProtocol(string sBuffer);
	IEncryptClass* encryptClass = nullptr;
	SOCKET clientSocket;
	bool connecting = false;
	wstring serverIP;
	int serverPort;
	thread connectThread;
	thread getProtocolThread;
	unordered_map<wstring, LLGameServerProtocol*> legalProtocolMap;
};