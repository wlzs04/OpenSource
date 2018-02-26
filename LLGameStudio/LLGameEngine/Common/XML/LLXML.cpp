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
	file.open(filePath,ifstream::binary);//一定要声明使用二进制读取，默认按字符读取。
	
	//在Windows下，文件中回车是“\n\r”,在获得文件长度时是2，读取却当成一个字符，有可能会影响编码判断。
	if (file)
	{
		FileEncode fe = CheckFileEncode(file);

		int markNum = 0;

		switch (fe)
		{
		case FileEncode::ANSI:
			file.imbue(locale(""));
			break;
		case FileEncode::UTF_8_WITH_BOM:
			markNum = 3;
		case FileEncode::UTF_8_NO_BOM:
			file.imbue(locale(locale::empty(), new codecvt_utf8<wchar_t>));
			break;
		case FileEncode::UTF_16_LITTLE_ENDIAN:
			//先空出来。
			markNum = 1;
			break;
		case FileEncode::UTF_16_BIG_ENDIAN:
			markNum = 1;
			file.imbue(locale(locale::empty(), new codecvt_utf16<wchar_t>));
			break;
		default:
			break;
		}
		
		file.seekg(0, ios_base::end); // 移动到文件尾。
		int fileLength = file.tellg(); // 取得当前位置的指针长度，即文件长度。
		file.seekg(0, ios_base::beg);
		wchar_t* fileBuffer = new wchar_t[fileLength];
		file.read(fileBuffer, fileLength);
		wchar_t* fileBufferStart = fileBuffer;
		wchar_t* fileBufferEnd = fileBuffer + fileLength;
		fileBuffer += markNum;
		fileLength -= markNum;
		int flag = 0;
		//开始分析语法。
		while (fileBuffer<fileBufferEnd&&fileLength>0)
		{
			if (!LoadUnknown(fileBuffer, fileLength))
			{
				file.close();
				delete[] fileBufferStart;
				fileBuffer = NULL;
				fileBufferStart = NULL;
				fileBufferEnd = NULL;
				return false;
			}
		}
		delete[] fileBufferStart;
		fileBuffer = NULL;
		fileBufferStart = NULL;
		fileBufferEnd = NULL;
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
	FileEncode fileEncode = FileEncode::UTF_8_NO_BOM;
	wchar_t wch1, wch2;
	file.get(wch1);
	file.get(wch2);//从文件读取前四个字节
	//不同的编码符合不同的规则
	if (wch1 == 0xFF && wch2 == 0xFE)
	{
		fileEncode = FileEncode::UTF_16_LITTLE_ENDIAN;
	}
	else if(wch1 == 0xFE && wch2 == 0xFF)
	{
		fileEncode = FileEncode::UTF_16_BIG_ENDIAN;
	}
	else if (wch1 == 0xEF && wch2 == 0xBB)
	{
		wchar_t wch3; 
		file.get(wch3);
		if (wch3 == 0xBF)
		{
			fileEncode = FileEncode::UTF_8_WITH_BOM;
		}
	}
	else
	{
		//之后就要判断是无BOM型UTF_8还是ANSI，需要将全文遍历找规则，这部分笔者直接拷贝了SlimXML的写法。
		//（其实在Window下可以不用考虑无BOM型UTF_8，新建的文件会自动带标志，除非在别处下载的或强行转成无BOM格式，但为了通用性还是需要加上判断。）

		file.seekg(0, ios_base::end); // 移动到文件尾。
		int fileLength = file.tellg(); // 取得当前位置的指针长度，即文件长度。
		file.seekg(0, ios_base::beg); // 移动到文件头。
		wchar_t* fileBuffer = new wchar_t[fileLength];
		wchar_t* fileBufferStartPos = fileBuffer;
		file.read(fileBuffer, fileLength);
		bool multiBytes = false;
		while (fileLength > 0)
		{
			wchar_t w = *fileBuffer;
			if ((w & 0x80) == 0)//0x80代表10000000，该方法判断首位是否为0,以下类似逐位判断。
			{
				++fileBuffer;
				--fileLength;
			}
			else
			{
				wchar_t w1 = *(fileBuffer + 1);
				multiBytes = true;
				if ((w & 0xf0) == 0xe0)
				{
					if (fileLength < 3)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					if ((w1 & 0xc0) != 0x80 || (*(fileBuffer + 2) & 0xc0) != 0x80)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					fileBuffer += 3;
					fileLength -= 3;
				}
				else if ((w1 & 0xe0) == 0xc0)
				{
					if (fileLength < 2)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					int a = (w1 & 0xc0);
					if (a != 0x80)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					fileBuffer += 2;
					fileLength -= 2;
				}
				else if ((w1 & 0xf8) == 0xf0)
				{
					if (fileLength < 4)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					if ((w1 & 0xc0) != 0x80 || (*(fileBuffer + 2) & 0xc0) != 0x80 || (*(fileBuffer + 3) & 0xc0) != 0x80)
					{
						fileEncode = FileEncode::ANSI;
						break;
					}
					fileBuffer += 4;
					fileLength -= 4;
				}
				else
				{
					fileEncode = FileEncode::ANSI;
					break;
				}
			}
		}
		delete[] fileBufferStartPos;
		fileBufferStartPos = NULL;
		fileBuffer = NULL;
		file.seekg(0, ios_base::beg);
	}
	return fileEncode;
}

bool LLXMLDocument::LoadUnknown(wchar_t*& fileBuffer, int& bufferSize)
{
	wchar_t w = *fileBuffer;
	if (*fileBuffer == L'<')
	{
		if (*(fileBuffer+1)==L'?')
		{
			fileBuffer++;
			bufferSize--;
			LoadDefine(fileBuffer, bufferSize);
		}
		else
		{
			LoadNode(fileBuffer, bufferSize);
		}
	}
	else
	{
		return false;
	}
	return true;
}

bool LLXMLDocument::LoadNode(wchar_t*& fileBuffer, int& bufferSize)
{
	while (*fileBuffer != L' '|| *fileBuffer != L'>')
	{

	}
	return true;
}

bool LLXMLDocument::LoadDefine(wchar_t*& fileBuffer, int& bufferSize)
{
	while (*fileBuffer!=L'>')
	{
		fileBuffer++;
		bufferSize--;
		if (bufferSize == 0)
		{
			return false;
		}
	}
	return true;
}
