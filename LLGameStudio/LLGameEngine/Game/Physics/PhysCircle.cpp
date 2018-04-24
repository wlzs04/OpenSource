#include "PhysCircle.h"
#include "PhysRectangle.h"
//class PhysRectangle;

PhysCircle::PhysCircle()
{
	physicsType = PhysicsType::Circle;
}

void PhysCircle::SetRedius(float radius)
{
	this->radius = radius;
}

float PhysCircle::GetRedius()
{
	return radius;
}

bool PhysCircle::IsCollision(IPhysObject * iPhysObject)
{
	switch (iPhysObject->GetPhysicsType())
	{
	case PhysicsType::Circle:
	{
		float length = MathHelper::GetLengthBetweenPoints(position,iPhysObject->GetPosition());
		return length < radius + ((PhysCircle*)iPhysObject)->GetRedius();
	}
	case PhysicsType::Rectangle:
	{
		PhysRectangle* rect = (PhysRectangle*)iPhysObject;
		Vector2 vp = rect->GetPosition();
		float xLength = abs(vp.x - position.x);
		float yLength = abs(vp.y - position.y);

		return xLength - radius - rect->GetWidth()/2 < 0 && yLength - radius - rect->GetHeight()/2 < 0;
	}
	default:
		break;
	}
	return false;
}

void PhysCircle::DoCollision(IPhysObject * iPhysObject)
{
	switch (iPhysObject->GetPhysicsType())
	{
	case PhysicsType::Circle:
	{
		//将相交的物体分离
		float length = MathHelper::GetLengthBetweenPoints(position, iPhysObject->GetPosition());
		float superpositionLength = radius + ((PhysCircle*)iPhysObject)->GetRedius() - length;
		float moveLength = superpositionLength/2;
		Vector2 v1 = position - iPhysObject->GetPosition();
		Vector2 v2 = iPhysObject->GetPosition() - position;
		if (isDynamic&&iPhysObject->IsDynamic())
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, v1, moveLength));
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), v2, moveLength));
		}
		else if(isDynamic)
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, v1, superpositionLength));
		}
		else if(iPhysObject->IsDynamic())
		{
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), v2, superpositionLength));
		}
		//计算碰撞物体之后的自身运动
		if (isDynamic)
		{
			Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(velocity, v1);
			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(velocity));
			SetVelocity(newVelocity);
		}

		//计算碰撞物体之后的另一个运动
		if (iPhysObject->IsDynamic())
		{
			Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(iPhysObject->GetVelocity(), v2);
			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(iPhysObject->GetVelocity()));
			iPhysObject->SetVelocity(newVelocity);
		}
		break;
	}
	case PhysicsType::Rectangle:
	{
		PhysRectangle* rect = (PhysRectangle*)iPhysObject;

		//将相交的物体分离
		Vector2 vp = rect->GetPosition();
		float xLength = abs(vp.x - position.x);
		int xzf = vp.x - position.x > 0 ? 1 : -1;
		float yLength = abs(vp.y - position.y);
		int yzf = vp.y - position.y > 0 ? 1 : -1;

		xLength = radius + rect->GetWidth() / 2 - xLength;
		yLength = radius + rect->GetHeight() / 2 - yLength;
		float moveLength = 0;
		if (xLength <= yLength)
		{
			moveLength = xLength;
			yLength = 0;
		}
		else
		{
			moveLength = yLength;
			xLength = 0;
		}

		if (isDynamic&&iPhysObject->IsDynamic())
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, Vector2(xzf*(-xLength), yzf*(-yLength)), moveLength /2));
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), Vector2(xzf*xLength, yzf*yLength), moveLength / 2));
		}
		else if (isDynamic)
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, Vector2(xzf*(-xLength), yzf*(-yLength)), moveLength));
		}
		else if (iPhysObject->IsDynamic())
		{
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), Vector2(xzf*xLength, yzf*yLength), moveLength));
		}

		//计算碰撞物体之后的自身运动
		if (isDynamic)
		{
			Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(velocity, Vector2(xzf*(-xLength), yzf*(-yLength)));
			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(velocity));
			SetVelocity(newVelocity);
		}

		//计算碰撞物体之后的另一个运动
		if (iPhysObject->IsDynamic())
		{
			Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(iPhysObject->GetVelocity(), Vector2(xzf*xLength, yzf*yLength));
			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(iPhysObject->GetVelocity()));
			iPhysObject->SetVelocity(newVelocity);
		}

		break;
	}
	default:
		break;
	}
}

void PhysCircle::CreateBoundingVolume()
{
}
