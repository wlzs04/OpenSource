#pragma once
#include <string>
#include <functional>
#include <thread>
#include "LLGameProtocol.h"
#pragma comment(lib, "ws2_32.lib")

using namespace std;

class LLGameNetClient
{
public:
	LLGameNetClient();
	~LLGameNetClient();
	//连接指定IP端口，非阻塞,连接成功后会自动调用协议处理方法。
	void StartConnect(wstring ip, int port);
	//发送协议
	bool SendProtocol(LLGameProtocol protocol);
	//停止连接
	void StopConnect();
	//连接成功事件
	function<void()> OnConnectSuccessHandle;
	//连接失败事件
	function<void()> OnConnectFailHandle;
	//连接断开事件
	function<void()> OnDisconnectHandle;
	//处理协议事件
	function<void(LLGameProtocol)> OnProcessProtocolHandle;
private:
	//接收协议，使用前需实现OnProcessProtocolHandle处理协议方法，非阻塞。
	void AcceptProtocol();

	SOCKET clientSocket;
	bool connecting = false;
	wstring serverIP;
	int serverPort;
	thread connectThread;
	thread getProtocolThread;
};