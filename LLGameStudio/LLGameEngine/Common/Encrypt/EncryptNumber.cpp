#include "EncryptNumber.h"

string EncryptNumber::Encrypt(string content)
{
	string byteSource;
	int* keyInt = new int[teamLength];
	int keyLength = key.size();
	int moveLength = keyLength - teamLength;
	int contentLength = content.size();

	for (int i = 0; i < teamLength; i++)
	{
		keyInt[i] = key[i] - '0';
	}

	int moveInt = atoi(key.substr(teamLength).c_str());
	int tempi = 0;
	unsigned char tempb = 0;
	int tempBase = 0;
	
	int teamCount = (int)floor((double)contentLength / teamLength);
	for (int i = 0; i < teamCount; i++)
	{
		for (int j = 0; j < teamLength; j++)
		{
			tempb = content[tempBase + keyInt[j]];
			tempi = (tempb + moveInt) % byteLength;
			byteSource += (unsigned char)tempi;
		}
		tempBase += teamLength;
	}
	if (tempBase < contentLength)
	{
		for (int i = 0; i < teamLength; i++)
		{
			if (tempBase + keyInt[i] < contentLength)
			{
				tempb = content[tempBase + keyInt[i]];
				tempi = (tempb + moveInt) % byteLength;
				byteSource += (unsigned char)tempi;
			}
		}
	}
	return byteSource;
}

std::string EncryptNumber::Decode(string content)
{
	int* keyInt = new int[teamLength];
	int keyLength = key.size();
	int moveLength = keyLength - teamLength;
	int contentLength = content.size();
	for (int i = 0; i < teamLength; i++)
	{
		keyInt[i] = key[i]-'0';
	}
	int moveInt = byteLength - atoi(key.substr(teamLength).c_str());
	string byteSource = content;
	int tempi = 0;
	unsigned char tempb = 0;
	int tempBase = 0;
	int teamCount = (int)floor((double)contentLength / teamLength);
	for (int i = 0; i < teamCount; i++)
	{
		for (int j = 0; j < teamLength; j++)
		{
			tempb = content[tempBase + j];
			tempi = (tempb + moveInt) % byteLength;
			byteSource[tempBase + keyInt[j]] = (unsigned char)tempi;
		}
		tempBase += teamLength;
	}
	if (tempBase < contentLength)
	{
		int re = contentLength - tempBase;
		int reIndex = 0;
		for (int i = 0; i < teamLength; i++)
		{
			if (keyInt[i] < re)
			{
				tempb = content[tempBase + reIndex];
				tempi = (tempb + moveInt) % byteLength;
				byteSource[tempBase + keyInt[i]] = (unsigned char)tempi;
				reIndex++;
			}
		}
	}
	return byteSource;
}

void EncryptNumber::SetKey(string key)
{
	teamLength = atoi(key.substr(0, 1).c_str());
	this->key = key.substr(1);
}
