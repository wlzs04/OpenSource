#include "ParticleSystem.h"
#include "ParticleEmitter.h"

void ParticleSystem::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"ParticleEmitter")
		{
			ParticleEmitter* emitter = new ParticleEmitter();
			emitter->LoadFromXMLNode(var);
			AddNode(emitter);
			AddEmitter(emitter);
		}
	}
}

void ParticleSystem::LoadParticleFromFile(wstring filePath)
{
	paticleEmitters.clear();
	LLXMLDocument doc;
	doc.LoadXMLFromFile(SystemHelper::GetResourceRootPath() + L"\\" + filePath);
	LoadFromXMLNode(doc.GetRootNode());
}

void ParticleSystem::AddEmitter(ParticleEmitter * emitter)
{
	paticleEmitters.push_back(emitter);
}

Vector2 ParticleSystem::GetActualPosition()
{
	return Vector2(GetActualLeft()+actualWidth/2, GetActualTop() + actualHeight / 2);
}

void ParticleSystem::StartPlay()
{
	for (auto var : paticleEmitters)
	{
		var->StartPlay();
	}
}

void ParticleSystem::PausePlay()
{
	for (auto var : paticleEmitters)
	{
		var->PausePlay();
	}
}

void ParticleSystem::StopPlay()
{
	for (auto var : paticleEmitters)
	{
		var->StopPlay();
	}
}

void ParticleSystem::Update()
{
	if (!propertyEnable.value)
	{
		return;
	}
	for(auto var : paticleEmitters)
	{
		var->Update();
	}
}

void ParticleSystem::Render()
{
	if (!propertyEnable.value)
	{
		return;
	}
	for (auto var : paticleEmitters)
	{
		var->Render();
	}
}
