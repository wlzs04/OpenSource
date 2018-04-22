#pragma once
#include "..\..\Common\Helper\MathHelper.h"

class IPhysObject
{
public:
	//创建包围体
	virtual void CreateBoundingVolume() = 0;
	//设置位置
	void SetPosition(float x,float y);
	//设置旋转
	void SetAngle(float angle);
private:
	Vector2 position;
	Vector2 position;
	float angle = 0;
};