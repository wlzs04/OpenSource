#pragma once
#include "..\Game\LLGame.h"

//用来测试物理碰撞游戏功能的类。
//桌上冰球游戏
class TableHockeyGame : public LLGame
{
public:
	void InitUserData()override;
	void KeyDownEvent(void* sender, int key);
	void CollisionEvent(IPhysObject* object1, IPhysObject* object2);
	void UpdateUserData()override;
	void RenderCanvas(void* iuiNode, int i);
	
	void OnStartGame(void* sender, int e);
	void OnRestartGame(void* sender, int e);

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

	LLGameLayout* youResultLayout=nullptr;
	LLGameImage* nodeResultImage = nullptr;
	LLGameButton* nodeRestartButton = nullptr;
	LLGameLayout* startLayout = nullptr;
	LLGameButton* startButton = nullptr;

	bool gameStart = false;
};