#pragma once
#include "..\Game\LLGame.h"

//用来测试游戏功能的类。
class TestLLGame: public LLGame
{
public:
	void InitUserData()override;
	void UpdateUserData()override;
	void RenderCanvas(void* iuiNode, int i);

	LLGameCanvas* nodeCanvas = nullptr;
	vector<PhysCircle*> vectorCircle;
	vector<PhysRectangle*> vectorRectangle;
	PhysicsWorld* physicsWorld;
	void* whiteBrush = nullptr;
};