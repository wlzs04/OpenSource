#pragma once
#include "PhysicsWorld.h"
#include "PhysRectangle.h"
#include "PhysEllipse.h"
#include "PhysCircle.h"
#include "PhysTriangle.h"
#include "PhysPolygon.h"

class PhysicsManager
{
public:
	//创建物理世界
	PhysicsWorld* CreatePhysicsWorld();
	//创建矩形包围盒
	PhysRectangle* CreateRectangle(int width,int height);
	//创建圆形包围盒
	PhysCircle* CreateCircle(int radius);
	//创建椭圆形包围盒
	PhysEllipse* CreateEllipse(int radiusX,int radiusY);
	//创建椭圆形包围盒
	PhysTriangle* CreateTriangle(int radiusX, int radiusY);
	//创建多边形包围盒
	PhysPolygon* CreatePolygon();
};