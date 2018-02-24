#include "LLXML.h"

LLXMLDocument::LLXMLDocument()
{
}

LLXMLDocument::~LLXMLDocument()
{
}

bool LLXMLDocument::LoadXMLFromFile(wstring filePath)
{
	wifstream file;
	file.open(filePath);
	if (file)
	{
		FileEncode fe = CheckFileEncode(file);
		switch (fe)
		{
		case ANSI:
			break;
		case UTF_8_WITH_BOM:
			break;
		case UTF_8_NO_BOM:
			break;
		case UTF_16_LITTLE_ENDIAN:
			break;
		case UTF_16_BIG_ENDIAN:
			break;
		default:
			break;
		}
		wstring ws;
		file >> ws;
		file.close();
	}
	else
	{
		return false;
	}
	return false;
}

bool LLXMLDocument::SaveXMLToFile(wstring filePath)
{
	return false;
}

FileEncode LLXMLDocument::CheckFileEncode(wifstream& file)
{
	FileEncode fileEncode = FileEncode::ANSI;
	wchar_t wch1, wch2;
	file.get(wch1);
	file.get(wch2);//从文件读取前四个字节
	//不同的编码符合不同的规则
	if (wch1 == 0xFF && wch2 == 0xFE)
	{
		return FileEncode::UTF_16_LITTLE_ENDIAN;
	}
	else if(wch1 == 0xFE && wch2 == 0xFF)
	{
		return FileEncode::UTF_16_BIG_ENDIAN;
	}
	else if (wch1 == 0xEF && wch2 == 0xBB)
	{
		wchar_t wch3; 
		file.get(wch3);
		if (wch3 == 0xBF)
		{
			return FileEncode::UTF_8_WITH_BOM;
		}
	}
	//之后就要判断是无BOM型UTF_8还是ANSI，需要将全文遍历找规则。

	file.seekg(0, ios_base::end); // 移动到文件尾。
	size_t fileLength = file.tellg(); // 取得当前位置的指针长度，即文件长度。
	file.seekg(0, ios_base::beg); // 移动到文件头。
	wchar_t* fileBuffer = new wchar_t[fileLength];
	file.read(fileBuffer, fileLength);
	bool multiBytes = false;
	while (fileLength>0)
	{
		if ((*fileBuffer & 0x80) == 0)//0x80代表10000000，该方法判断首位是否为0,以下类似逐位判断。
		{
			++fileBuffer;
			--fileLength;
		}
		else
		{
			multiBytes = true;
			if ((*fileBuffer & 0xf0) == 0xe0)
			{
				if (fileLength < 3)
				{
					return ANSI;
				}
				if ((*(fileBuffer + 1) & 0xc0) != 0x80 || (*(fileBuffer + 2) & 0xc0) != 0x80)
				{
					return ANSI;
				}
				fileBuffer += 3;
				fileLength -= 3;
			}
			else if ((*fileBuffer & 0xe0) == 0xc0)
			{
				if (fileLength < 2)
				{
					return ANSI;
				}
				int a = (*(fileBuffer + 1) & 0xc0);
				if ( a!= 0x80)
				{
					return ANSI;
				}
				fileBuffer += 2;
				fileLength -= 2;
			}
			else if ((*fileBuffer & 0xf8) == 0xf0)
			{
				if (fileLength < 4)
				{
					return ANSI;
				}
				if ((*(fileBuffer + 1) & 0xc0) != 0x80 || (*(fileBuffer + 2) & 0xc0) != 0x80 || (*(fileBuffer + 3) & 0xc0) != 0x80)
				{
					return ANSI;
				}
				fileBuffer += 4;
				fileLength -= 4;
			}
			else
			{
				return ANSI;
			}
		}
	}
	
	return FileEncode::UTF_8_NO_BOM;
}
