#include "PhysCircle.h"
#include "PhysRectangle.h"
//class PhysRectangle;

PhysCircle::PhysCircle()
{
	physicsShapeType = PhysicsShapeType::Circle;
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
	switch (iPhysObject->GetPhysicsShapeType())
	{
	case PhysicsShapeType::Circle:
	{
		float length = MathHelper::GetLengthBetweenPoints(position,iPhysObject->GetPosition());
		return length < radius + ((PhysCircle*)iPhysObject)->GetRedius();
	}
	case PhysicsShapeType::Rectangle:
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
	switch (iPhysObject->GetPhysicsShapeType())
	{
	case PhysicsShapeType::Circle:
	{
		//将相交的物体分离
		{
			float length = MathHelper::GetLengthBetweenPoints(position, iPhysObject->GetPosition());
			float superpositionLength = radius + ((PhysCircle*)iPhysObject)->GetRedius() - length;
			float moveLength = superpositionLength / 2;
			Vector2 vp1 = position - iPhysObject->GetPosition();
			Vector2 vp2 = iPhysObject->GetPosition() - position;
			if (IsCanMove()&&iPhysObject->IsCanMove())
			{
				SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, vp1, moveLength));
				iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), vp2, moveLength));
			}
			else if (IsCanMove())
			{
				SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, vp1, superpositionLength));
			}
			else if (iPhysObject->IsCanMove())
			{
				iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), vp2, superpositionLength));
			}
		}

		//计算碰撞物体之后的自身运动
		float m1 = mass;//圆1的质量
		float m2 = iPhysObject->GetMass();//圆2的质量
		Vector2 p1 = position;//圆1的中心点
		Vector2 p2 = iPhysObject->GetPosition();//圆2的中心点
		Vector2 v1 = velocity;//圆1的速度
		Vector2 v2 = iPhysObject->GetVelocity();//圆2的速度
		Vector2 p1Top2 = MathHelper::GetNormalVector2(Vector2(p2.x - p1.x, p2.y - p1.y));//向量从p1到p2，连线
		Vector2 p1Np2 = Vector2(-p1Top2.y, p1Top2.x);//向量从p1到p2，连线
		//圆1速度在连线上的分量
		float fl1 = MathHelper::GetDotMultiply(v1, p1Top2)/ MathHelper::GetVector2Length(p1Top2);
		float fl1real = 0;
		float fn1real = sqrtf(MathHelper::GetVector2Length(v1)*MathHelper::GetVector2Length(v1) - fl1 * fl1);
		fn1real = MathHelper::GetDotMultiply(v1, p1Np2) / MathHelper::GetVector2Length(p1Np2);
		//圆2速度在连线上的分量
		float fl2 = iPhysObject->IsDynamic()? MathHelper::GetDotMultiply(v2, p1Top2) / MathHelper::GetVector2Length(p1Top2):0;
		float fl2real = 0;
		float fn2real = iPhysObject->IsDynamic() ? sqrtf(MathHelper::GetVector2Length(v2) - fl2 * fl2):0;
		fn2real = MathHelper::GetDotMultiply(v2, p1Np2) / MathHelper::GetVector2Length(p1Np2);
		
		if (iPhysObject->IsDynamic())
		{
			fl1real = (fl1 * (m1 - m2) + fl2 * 2 * m2) / (m1 + m2);
			fl2real = (fl2 * (m2 - m1) + fl1 * 2 * m1) / (m1 + m2);
		}
		else
		{
			fl1real = -fl1;
		}

		Vector2 vzl1 = Vector2(p1Top2.x*fl1real, p1Top2.y*fl1real);
		Vector2 vzn1 = Vector2(p1Np2.x*fn1real, p1Np2.y*fn1real);
		SetVelocity(vzl1+ vzn1);

		if (iPhysObject->IsDynamic())
		{
			Vector2 vzl2 = Vector2(p1Top2.x*fl2real, p1Top2.y*fl2real);
			Vector2 vzn2 = Vector2(p1Np2.x*fn2real, p1Np2.y*fn2real);
			iPhysObject->SetVelocity(vzl2 + vzn2);
		}

		break;
	}
	case PhysicsShapeType::Rectangle:
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

		if (IsCanMove()&&iPhysObject->IsCanMove())
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, Vector2(xzf*(-xLength), yzf*(-yLength)), moveLength /2));
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), Vector2(xzf*xLength, yzf*yLength), moveLength / 2));
		}
		else if (IsCanMove())
		{
			SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, Vector2(xzf*(-xLength), yzf*(-yLength)), moveLength));
		}
		else if (iPhysObject->IsCanMove())
		{
			iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), Vector2(xzf*xLength, yzf*yLength), moveLength));
		}

		//计算碰撞物体之后的自身运动
		if (IsDynamic())
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
