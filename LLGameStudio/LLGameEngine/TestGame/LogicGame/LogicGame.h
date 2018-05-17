#pragma once
#include "..\..\Game\LLGame.h"
#include "..\..\Game\Logic\LLScriptManager.h"

class Game;

class LogicGame : public LLGame
{
public :
	~LogicGame();
protected:
	virtual void InitUserData() override;
	virtual void UpdateUserData() override;

	//用来初始化可以导出到脚本的方法。
	void InitGameFunction();

	template<class T>
	void AddGameCppFunction(wstring functionName, wstring returnTypeName, Parameter(LogicGame::*cppFunction)(vector<Parameter>* inputList), T* classInstance);
	
	LLScriptManager* scriptManager = nullptr;
	PhysicsWorld* physicsWorld;
	Game* gamePtr =nullptr;

	unordered_map<wstring, ParticleSystem*>particleMap;
	unordered_map<wstring, Actor*>actorMap;
	unordered_map<wstring, IPhysObject*>physicsMap;
	unordered_map<wstring, void*>brushMap;

private:
	//游戏结束
	//参数：
	//返回：
	Parameter StopGame(vector<Parameter>* inputList = nullptr);

	//游戏在这一帧运行的时间
	//参数：
	//返回：float
	Parameter GetThisTickTime(vector<Parameter>* inputList = nullptr);

	//向游戏界面中添加布局
	//参数：布局的名称，布局的文件路径
	//返回：
	Parameter AddLayout(vector<Parameter>* inputList = nullptr);

	//从游戏界面中移除布局
	//参数：布局的名称
	//返回：
	Parameter RemoveLayout(vector<Parameter>* inputList = nullptr);

	//设置游戏中UI节点的属性
	//参数：节点的名称，属性的名称，属性的值
	//返回：
	Parameter SetUINodeProperty(vector<Parameter>* inputList = nullptr);

	//绑定按钮点击的事件
	//参数：按钮的名称，脚本中方法的名称
	//返回：
	Parameter BindButtonClickEvent(vector<Parameter>* inputList = nullptr);

	//绑定画布绘制的事件
	//参数：画布的名称，脚本中方法的名称
	//返回：
	Parameter BindCanvasRenderEvent(vector<Parameter>* inputList = nullptr);

	//向游戏界面中添加粒子
	//参数：粒子的名称，粒子的文件路径
	//返回：
	Parameter AddParticle(vector<Parameter>* inputList = nullptr);

	//从游戏界面中移除粒子
	//参数：粒子的名称
	//返回：
	Parameter RemoveParticle(vector<Parameter>* inputList = nullptr);

	//从游戏界面中添加角色
	//参数：角色的名称，角色的文件路径
	//返回：
	Parameter LoadActor(vector<Parameter>* inputList = nullptr);

	//从游戏界面中移除角色
	//参数：角色的名称
	//返回：
	Parameter RemoveActor(vector<Parameter>* inputList = nullptr);

	//设置角色位置
	//参数：角色的名称，角色的横坐标，角色的纵坐标
	//返回：
	Parameter SetActorPosition(vector<Parameter>* inputList = nullptr);

	//添加物理世界,需要使用游戏中提供的物理功能是需要在游戏初始化时调用一次
	//参数：
	//返回：
	Parameter CreatePhysicsWorld(vector<Parameter>* inputList = nullptr);

	//添加物理矩形并添加到世界中
	//参数：物体的名称，宽，高，状态（静态=0，动态=1，主动=2）
	//返回：
	Parameter CreatePhysRectangle(vector<Parameter>* inputList = nullptr);

	//添加物理圆形
	//参数：物体的名称，半径，状态（静态=0，动态=1，主动=2）
	//返回：
	Parameter CreatePhysCircle(vector<Parameter>* inputList = nullptr);

	//设置物理物体位置
	//参数：物体的名称，物体的横坐标，物体的纵坐标
	//返回：
	Parameter SetPhysPosition(vector<Parameter>* inputList = nullptr);

	//设置物理物体位置
	//参数：物体的名称，速度x，速度y
	//返回：
	Parameter SetPhysVelocity(vector<Parameter>* inputList = nullptr);
	
	//获得物理物体横坐标
	//参数：物体的名称
	//返回：float
	Parameter GetPhysPositionX(vector<Parameter>* inputList = nullptr);

	//获得物理物体纵坐标
	//参数：物体的名称
	//返回：float
	Parameter GetPhysPositionY(vector<Parameter>* inputList = nullptr);

	//绑定物理物体相互碰撞的事件
	//参数：脚本中方法的名称
	//返回：
	Parameter BindCollisionEvent(vector<Parameter>* inputList = nullptr);

	//让受物理管理的物体模拟一段时间
	//参数：模拟时长
	//返回：
	Parameter PhysSimulate(vector<Parameter>* inputList = nullptr);

	//开始物理模拟
	//参数：
	//返回：
	Parameter PhysStart(vector<Parameter>* inputList = nullptr);

	//停止物理模拟
	//参数：
	//返回：
	Parameter PhysStop(vector<Parameter>* inputList = nullptr);

	//绑定网络连接成功的事件
	//参数：脚本中方法的名称
	//返回：
	Parameter BindConnectServerSuccessEvent(vector<Parameter>* inputList = nullptr);

	//绑定网络连接失败的事件
	//参数：脚本中方法的名称
	//返回：
	Parameter BindConnectServerFailEvent(vector<Parameter>* inputList = nullptr);

	//开始连接网络
	//参数：
	//返回：
	Parameter StartConnectServer(vector<Parameter>* inputList = nullptr);

	//绑定按键点击的事件
	//参数：脚本中方法的名称
	//返回：
	Parameter BindKeyDownEvent(vector<Parameter>* inputList = nullptr);

	//获得当前鼠标横坐标
	//参数：
	//返回：float
	Parameter GetMouseX(vector<Parameter>* inputList = nullptr);

	//获得当前鼠标纵坐标
	//参数：
	//返回：float
	Parameter GetMouseY(vector<Parameter>* inputList = nullptr);

	//获得游戏窗体宽度
	//参数：
	//返回：float
	Parameter GetGameWidth(vector<Parameter>* inputList = nullptr);

	//获得游戏窗体高度
	//参数：
	//返回：float
	Parameter GetGameHeight(vector<Parameter>* inputList = nullptr);

	//添加画刷
	//参数：画刷的名称，r，g，b，a
	//返回：
	Parameter AddBrush(vector<Parameter>* inputList = nullptr);

	//设置当前画刷
	//参数：画刷的名称
	//返回：
	Parameter SetCurrentBrush(vector<Parameter>* inputList = nullptr);

	//绘制矩形
	//参数：是否填充，矩形中心点X坐标，矩形中心点Y坐标，宽，高
	//返回：
	Parameter DrawRectangle(vector<Parameter>* inputList = nullptr);

	//绘制椭圆
	//参数：是否填充，椭圆中心点X坐标，椭圆中心点Y坐标，半径1，半径2
	//返回：
	Parameter DrawEllipse(vector<Parameter>* inputList = nullptr);
};

template<class T>
void LogicGame::AddGameCppFunction(wstring functionName, wstring returnTypeName, Parameter(LogicGame::*cppFunction)(vector<Parameter>* inputList), T* classInstance)
{
	Function* functionPtr = new Function(functionName, returnTypeName, nullptr, gamePtr);
	functionPtr->SetCppFunction(std::bind(cppFunction, classInstance, placeholders::_1));
	gamePtr->AddCppFunction(functionPtr);
}
