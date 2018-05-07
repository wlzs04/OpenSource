#pragma once
#include "../Helper/WStringHelper.h"

class IEncryptClass
{
public:
	//加密
	virtual string Encrypt(string content) = 0;

	//解密
	virtual string Decode(string content) = 0;
};