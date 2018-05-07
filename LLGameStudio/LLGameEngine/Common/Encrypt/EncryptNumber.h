#pragma once
#include "IEncryptClass.h"

class EncryptNumber :public IEncryptClass
{
public:
	int teamLength = 4;
	int byteLength = 255;
	string key = "01230";

	string Encrypt(string content) override;

	string Decode(string content) override;

	void SetKey(string key);
};