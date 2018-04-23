#pragma once
#include "IPhysObject.h"

class PhysRectangle :public IPhysObject
{
public:
	PhysRectangle();
	void SetRect(float width,float height);
	float GetWidth();
	float GetHeight();
	virtual bool IsCollision(IPhysObject* iPhysObject) override;
	virtual void DoCollision(IPhysObject* iPhysObject) override;
protected:
	virtual void CreateBoundingVolume() override;
private:
	float width=2;
	float height=2;
};