#pragma once
#include "..\Game\LLGame.h"

//用来测试物理碰撞游戏功能的类。
//桌上冰球游戏
class TableHockeyGame : public LLGame
{
public:
	void InitUserData()override;
	void InitAudio();
	void InitLayout();
	void InitObject();
	void InitConnectNet();
	void ProcessProtocol(LLGameServerProtocol* protocol);
	void KeyDownEvent(void* sender, int key);
	void CollisionEvent(IPhysObject* object1, IPhysObject* object2);
	void UpdateUserData()override;
	void RenderCanvas(void* iuiNode, int i);
	
	//我赢了。
	void OnWin();
	//我输了。
	void OnLost();
	//点击开始游戏
	void OnStartGame(void* sender, int e);
	//由服务器调用开始游戏
	void StartGameByServer();
	//重新开始游戏
	void OnRestartGame(void* sender, int e);
	//由服务器调用重新开始游戏
	void RestartGameByServer();
	//由服务器设置对手球的位置
	void SetOpponentBallPosition(float x, float y);
	//发球
	void ServeBall();

	LLGameCanvas* nodeCanvas = nullptr;
	vector<PhysCircle*> vectorCircle;
	PhysCircle* iceBallPhys = nullptr;
	PhysCircle* myHandBallPhys = nullptr;
	PhysCircle* opponentHandBallPhys = nullptr;
	PhysRectangle* myHolePhys = nullptr;
	PhysRectangle* opponentHolePhys = nullptr;
	vector<PhysRectangle*> vectorRectangle;

	PhysicsWorld* physicsWorld;

	void* iceBallBrush = nullptr;//冰球画刷
	void* handBallBrush = nullptr;//手球画刷
	void* holeBrush = nullptr;//球洞画刷
	void* blockBrush = nullptr;//边缘画刷
	void* backBrush = nullptr;//背景画刷

	POINT lastMousePosition;

	LLGameLayout* youResultLayout = nullptr;
	LLGameImage* nodeResultImage = nullptr;
	LLGameButton* nodeRestartButton = nullptr;

	LLGameLayout* startLayout = nullptr;
	LLGameButton* startButton = nullptr;

	LLGameLayout* gameRecordLayout = nullptr;
	LLGameText* textMyRecord = nullptr;
	LLGameText* textOpponentRecord = nullptr;

	LLGameLayout* gameCountDownLayout = nullptr;
	LLGameText* textCountDown = nullptr;

	ParticleSystem* particleSystem = nullptr;

	Actor* actor = nullptr;
	Bone* myHandbone = nullptr;

	bool gameStart = false;
	float blockWidth = 40;
	float holeWidth = 80;
	float ballRadius = 40;
	Vector2 handBallConstraintPoint;;//手球的约束点。
	float handBallMaxLength = 200;//手球相对于约束点，最大移动距离。
	float serveCountDown = 3;//开球倒计时。
	float realCountDown = 0;//开球倒计时。
	bool isCountDownState = false;
	int myRecord = 0;
	int opponentRecord = 0;

	/*wstring ip = L"10.237.20.13";
	int port = 1234;*/
	bool playOnLine = false;
};

class CStartGameProtocol : public LLGameClientProtocol
{
public:
	CStartGameProtocol() :LLGameClientProtocol(L"CStartGameProtocol") {}
};

class SStartGameProtocol : public LLGameServerProtocol
{
public:
	SStartGameProtocol() :LLGameServerProtocol(L"SStartGameProtocol") {}
	virtual LLGameServerProtocol* GetInstance() override
	{
		return new SStartGameProtocol();
	}
	virtual void Process(LLGame* ptr) override
	{
		((TableHockeyGame*)ptr)->StartGameByServer();
	}
};

class CRestartGameProtocol : public LLGameClientProtocol
{
public:
	CRestartGameProtocol() :LLGameClientProtocol(L"CRestartGameProtocol") {}
};

class SRestartGameProtocol : public LLGameServerProtocol
{
public:
	SRestartGameProtocol() :LLGameServerProtocol(L"SRestartGameProtocol") {}
	virtual LLGameServerProtocol* GetInstance() override
	{
		return new SRestartGameProtocol();
	}
	virtual void Process(LLGame* ptr) override
	{
		((TableHockeyGame*)ptr)->RestartGameByServer();
	}
};

class CSendMyHandBallInfoProtocol : public LLGameClientProtocol
{
public:
	CSendMyHandBallInfoProtocol() :LLGameClientProtocol(L"CSendMyHandBallInfoProtocol") {}
};

class SGetOpponentBallInfoProtocol : public LLGameServerProtocol
{
public:
	SGetOpponentBallInfoProtocol() :LLGameServerProtocol(L"SGetOpponentBallInfoProtocol") {}
	virtual LLGameServerProtocol* GetInstance() override
	{
		return new SGetOpponentBallInfoProtocol();
	}
	virtual void Process(LLGame* ptr) override
	{
		wstring px = GetContent(L"px");
		wstring py = GetContent(L"py");
		((TableHockeyGame*)ptr)->SetOpponentBallPosition(
			WStringHelper::GetFloat(px),
			WStringHelper::GetFloat(py));
	}
};