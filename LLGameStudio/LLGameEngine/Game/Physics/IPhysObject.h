#pragma once
#include "..\..\Common\Helper\MathHelper.h"

//物理类型
enum class PhysicsType
{
	Rectangle,//长方形
	Circle,//圆形
	Ellipse,//椭圆形
	Triangle,//三角形
	Polygon,//多边形
};

class IPhysObject
{
public:
	IPhysObject();
	~IPhysObject() {};
	//设置位置
	void SetPosition(Vector2 position);
	void SetPosition(float x,float y);
	Vector2 GetPosition();
	//设置旋转
	void SetAngle(float angle);
	//设置速度
	void SetVelocity(Vector2 velocity);
	void SetVelocity(float x, float y);
	void SetMass(float mass);
	float GetMass();
	Vector2 GetVelocity();
	float GetEnergy();
	PhysicsType GetPhysicsType();
	//检测两个物体是否发生碰撞。
	virtual bool IsCollision(IPhysObject* iPhysObject) = 0;
	//做碰撞后的处理。
	virtual void DoCollision(IPhysObject * var2) = 0;
	void SetDynamic();
	void SetStatic();
	bool IsDynamic();
protected:
	//创建包围体
	virtual void CreateBoundingVolume() = 0;
	void ResetEnergy();
	Vector2 position;
	PhysicsType physicsType = PhysicsType::Polygon;
	float angle = 0;//旋转角度
	float angularVelocity = 0;//角速度
	float energy = 0;//动能
	float mass=1;//质量
	Vector2 velocity;//速度
	bool isDynamic = true;
	float maxVelocity = 5;
};