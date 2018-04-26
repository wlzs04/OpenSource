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
		{
			float length = MathHelper::GetLengthBetweenPoints(position, iPhysObject->GetPosition());
			float superpositionLength = radius + ((PhysCircle*)iPhysObject)->GetRedius() - length;
			float moveLength = superpositionLength / 2;
			Vector2 vp1 = position - iPhysObject->GetPosition();
			Vector2 vp2 = iPhysObject->GetPosition() - position;
			if (isDynamic&&iPhysObject->IsDynamic())
			{
				SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, vp1, moveLength));
				iPhysObject->SetPosition(MathHelper::GetPointMoveByVelocityAndLength(iPhysObject->GetPosition(), vp2, moveLength));
			}
			else if (isDynamic)
			{
				SetPosition(MathHelper::GetPointMoveByVelocityAndLength(position, vp1, superpositionLength));
			}
			else if (iPhysObject->IsDynamic())
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
		float fn1real = sqrtf(MathHelper::GetVector2Length(v1) - fl1 * fl1);
		fn1real = MathHelper::GetDotMultiply(v1, p1Np2) / MathHelper::GetVector2Length(p1Np2);
		//圆2速度在连线上的分量
		float fl2 = MathHelper::GetDotMultiply(v2, p1Top2) / MathHelper::GetVector2Length(p1Top2);
		float fl2real = 0;
		float fn2real = sqrtf(MathHelper::GetVector2Length(v2) - fl2 * fl2);
		fn2real = MathHelper::GetDotMultiply(v2, p1Np2) / MathHelper::GetVector2Length(p1Np2);
		
		fl1real = (fl1 * (m1 - m2) + fl2 * 2 * m2) / (m1 + m2);
		fl2real = (fl2 * (m2 - m1) + fl1 * 2 * m1) / (m1 + m2);

		Vector2 vzl1 = Vector2(p1Top2.x*fl1real, p1Top2.y*fl1real);
		Vector2 vzn1 = Vector2(p1Np2.x*fn1real, p1Np2.y*fn1real);
		SetVelocity(vzl1+ vzn1);

		Vector2 vzl2 = Vector2(p1Top2.x*fl2real, p1Top2.y*fl2real);
		Vector2 vzn2 = Vector2(p1Np2.x*fn2real, p1Np2.y*fn2real);
		iPhysObject->SetVelocity(vzl2+ vzn2);

//		float m1 = mass;
//		float m2 = iPhysObject->GetMass();
//		Vector2 v10 = velocity;
//		Vector2 v20 = iPhysObject->GetVelocity();
//
//		float xF = (position.x - iPhysObject->GetPosition().x);
//		xF *= xF;
//		float yF = (position.y - iPhysObject->GetPosition().y);
//		yF *= yF;
//		float x2x1y2y1 = (iPhysObject->GetPosition().x - position.x);
//		x2x1y2y1 *= (iPhysObject->GetPosition().y - position.y);
//		float v1x = velocity.x;
//		float v1y = velocity.y;
//		float v2x = iPhysObject->GetVelocity().x;
//		float v2y = iPhysObject->GetVelocity().y;
//
//		float lengthF = xF + yF;
//
//		if (isDynamic)
//		{
//			/*Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(velocity, v1);
//			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(velocity));
//			SetVelocity(newVelocity);*/
//
//			SetVelocity((v1x*yF + v2x * xF + (v2y - v1y)*(x2x1y2y1)) / lengthF, (v1y*xF + v2y * yF + (v2x - v1x)*(x2x1y2y1)) / lengthF);
//
//			/*Vector2 fangxiang = Vector2((v1x*yF + v2x * xF + (v2y - v1y)*(x2x1y2y1)) / lengthF, (v1y*xF + v2y * yF + (v2x - v1x)*(x2x1y2y1)) / lengthF);
//			fangxiang = MathHelper::GetNormalVector2(fangxiang);
//			float ff = MathHelper::GetVector2Length((v10 * (m1 - m2) + v20 * 2 * m2) / (m1 + m2));
//
//			SetVelocity(MathHelper::GetVector2SetLength(fangxiang, ff));
//*/
//			/*Vector2 v1 = ;
//			SetVelocity(v1);*/
//		}
//
//		//计算碰撞物体之后的另一个运动
//		if (iPhysObject->IsDynamic())
//		{
//			/*Vector2 newVelocity = MathHelper::GetReflectByNormalVector2(iPhysObject->GetVelocity(), v2);
//			newVelocity = MathHelper::GetVector2SetLength(newVelocity, MathHelper::GetVector2Length(iPhysObject->GetVelocity()));
//			iPhysObject->SetVelocity(newVelocity);*/
//
//			iPhysObject->SetVelocity((v1x*xF + v2x * yF - (v2y - v1y)*(x2x1y2y1)) / lengthF, (v1y*yF + v2y * xF - (v2x - v1x)*(x2x1y2y1))/ lengthF);
//
//			/*Vector2 fangxiang = Vector2((v1x*xF + v2x * yF - (v2y - v1y)*(x2x1y2y1)) / lengthF, (v1y*yF + v2y * xF - (v2x - v1x)*(x2x1y2y1)) / lengthF);
//			fangxiang = MathHelper::GetNormalVector2(fangxiang);
//			float ff = MathHelper::GetVector2Length((v20 * (m2 - m1) + v10 * 2 * m1) / (m1 + m2));
//			iPhysObject->SetVelocity(MathHelper::GetVector2SetLength(fangxiang, ff));*/
//
//			/*Vector2 v2 = (v20 * (m2 - m1) + v10 * 2 * m1) / (m1 + m2);
//			SetVelocity(v2);*/
//		}
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
