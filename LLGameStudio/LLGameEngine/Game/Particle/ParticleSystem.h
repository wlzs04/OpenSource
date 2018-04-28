#pragma once
#include "..\UI\IUINode.h"
#include "ParticleEmitter.h"

class ParticleSystem :public IUINode
{
public:
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void LoadParticleFromFile(wstring filePath);
	void AddEmitter(ParticleEmitter* emitter);
	Vector2 GetActualPosition();
	void StartPlay();
	void PausePlay();
	void StopPlay();
	virtual void Update() override;
	virtual void Render() override;

private:
	vector<ParticleEmitter*> paticleEmitters;
};