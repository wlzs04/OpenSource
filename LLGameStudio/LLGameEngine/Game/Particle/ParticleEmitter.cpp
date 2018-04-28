#include "ParticleEmitter.h"
#include "ParticleSystem.h"

ParticleEmitter::ParticleEmitter()
{
	propertyMap[isLoop.name] = &isLoop;
	propertyMap[loopTime.name] = &loopTime;
	propertyMap[particleType.name] = &particleType;
	propertyMap[maxNumber.name] = &maxNumber;
	propertyMap[startNumber.name] = &startNumber;
	propertyMap[createNumberBySecond.name] = &createNumberBySecond;
	propertyMap[radius.name] = &radius;
	propertyMap[radiusError.name] = &radiusError;
	propertyMap[color.name] = &color;
	propertyMap[velocity.name] = &velocity;
	propertyMap[velocityError.name] = &velocityError;
	propertyMap[direction.name] = &direction;
	propertyMap[angleRange.name] = &angleRange;
	propertyMap[position.name] = &position;
	propertyMap[positionError.name] = &positionError;
	propertyMap[imagePath.name] = &imagePath;
	propertyMap[row.name] = &row;
	propertyMap[column.name] = &column;
}

void ParticleEmitter::SetProperty(wstring name, wstring value)
{
	if (name == L"color")
	{
		Color co;
		co.GetValueFromWString(value);
		colorBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(co.r, co.g, co.b,co.a);
	}
	IUINode::SetProperty(name, value);
}

void ParticleEmitter::Update()
{
	if (!propertyEnable.value || !play)
	{
		return;
	}

	if (isLoop.value && currentPlayTime > loopTime.value)
	{
		ResetParticle();
		currentPlayTime = 0;
		currentImageIndex = 0;
	}

	currentPlayTime += GameHelper::thisTickTime;
	double particleCreateNumberThisTickTime = GameHelper::thisTickTime * createNumberBySecond.value;
	int addNumber = (int)particleCreateNumberThisTickTime;
	if (MathHelper::RandFloat() < particleCreateNumberThisTickTime - addNumber)
	{
		addNumber++;
	}

	for (int i = 0; i < addNumber; i++)
	{
		AddParticle();
	}

	if (particleType.value == ParticleType::Sequence)
	{
		currentImageIndex = (int)((currentPlayTime / loopTime.value) * row.value * column.value);

		if (currentImageIndex >= row.value*column.value)
		{
			currentImageIndex = row.value * column.value - 1;
		}
	}

	//粒子移动和移除。
	for (int i = 0; i < particleList.size();)
	{
		if (particleList[i].MoveByTime(GameHelper::thisTickTime))
		{
			particleList.erase(particleList.cbegin()+i);
		}
		else
		{
			i++;
		}
	}
}

void ParticleEmitter::Render()
{
	if (!propertyEnable.value)
	{
		return;
	}
	int particleCount = particleList.size();
	GraphicsApi::GetGraphicsApi()->SetCurrentBrush(colorBrush);
	switch (particleType.value.value)
	{
	case ParticleType::Point:
		for (auto var : particleList)
		{
			ParticleSystem* particleSystem = (ParticleSystem*) parentNode;
			Vector2 parentPosition = particleSystem->GetActualPosition();
			GraphicsApi::GetGraphicsApi()->DrawEllipse(true, parentPosition.x + var.x, parentPosition.y + var.y, var.radius, var.radius);
		}
		break;
	case ParticleType::Star:
		/*for (auto var : particleList)
		{
			ParticleSystem* particleSystem = (ParticleSystem*)parentNode;
			Vector2 parentPosition = particleSystem->GetActualPosition();

			GraphicsApi::GetGraphicsApi()->DrawPolygon(polygon,true, parentPosition.x + var.x, parentPosition.y + var.y, var.radius, var.radius);
		}*/
		break;
	case ParticleType::Image:
		for (auto var : particleList)
		{
			ParticleSystem* particleSystem = (ParticleSystem*)parentNode;
			Vector2 parentPosition = particleSystem->GetActualPosition();
			GraphicsApi::GetGraphicsApi()->DrawImage(imagePath.value, parentPosition.x + var.x - var.radius / 2, parentPosition.y + var.y - var.radius / 2, var.radius*2, var.radius*2);
		}
		break;
	case ParticleType::Sequence:
		for (auto var : particleList)
		{
			if (currentImageIndex != 0)
			{
				int y = 0;
			}
			ParticleSystem* particleSystem = (ParticleSystem*)parentNode;
			Vector2 parentPosition = particleSystem->GetActualPosition();
			int r = currentImageIndex / column.value;
			int c = currentImageIndex % column.value;
			GraphicsApi::GetGraphicsApi()->DrawImagePart(imagePath.value, 
				parentPosition.x + var.x- var.radius/2, parentPosition.y + var.y- var.radius/2, var.radius * 2, var.radius * 2,
				c*(1.0/ column.value), r*(1.0 / row.value), 1.0 / column.value, 1.0 / row.value);
		}
		break;
	default:
		break;
	}
	GraphicsApi::GetGraphicsApi()->ResetDefaultBrush();
}

void ParticleEmitter::StartPlay()
{
	InitParticle();
	play = true;
	currentPlayTime = 0;
}

void ParticleEmitter::PausePlay()
{
	play = false;
}

void ParticleEmitter::StopPlay()
{
	play = false;
	currentPlayTime = 0;
}

void ParticleEmitter::ResetParticle()
{
	particleList.clear();
	for (int i = 0; i < startNumber.value; i++)
	{
		AddParticle();
	}
}

void ParticleEmitter::AddParticle()
{
	if (particleList.size() > maxNumber.value)
	{
		return;
	}
	double x = position.value.x + MathHelper::RandFloat() * positionError.value.x;
	double y = position.value.y + MathHelper::RandFloat() * positionError.value.y;
	double radius = (radiusError.value - 2 * MathHelper::RandFloat()* radiusError.value) + this->radius.value;
	double angle = ((1 - 2 * MathHelper::RandFloat()) * angleRange.value / 180 * MathHelper::PI);
	double dx = direction.value.x * cos(angle) - direction.value.y * sin(angle);
	double dy = direction.value.x * sin(angle) + direction.value.y * cos(angle);
	double vx = dx * (velocity.value + MathHelper::RandFloat() * velocityError.value);
	double vy = dy * (velocity.value + MathHelper::RandFloat() * velocityError.value);
	Particle particle = Particle(x, y, radius, vx, vy);
	particleList.push_back(particle);
}

void ParticleEmitter::InitParticle()
{
	ResetParticle();
	/*if (particleType.value == ParticleType::Star)
	{
		if (polygon!=nullptr)
		{
			delete polygon;
			polygon = NULL;
		}
		polygon = GraphicsApi::GetGraphicsApi()->CreatePolygon();
	}*/
}
