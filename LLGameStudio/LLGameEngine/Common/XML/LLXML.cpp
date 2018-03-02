#include "LLXML.h"

LLXMLProperty::LLXMLProperty(wstring name)
{
	this->name = name;
}

void LLXMLProperty::SetValue(wstring value)
{
	this->value = value;
}

wstring LLXMLProperty::GetName()
{
	return name;
}

wstring LLXMLProperty::GetValue()
{
	return value;
}

int LLXMLProperty::GetValueInt()
{
	return _wtoi(value.c_str());
}

float LLXMLProperty::GetValueFloat()
{
	return _wtof(value.c_str());
}

bool LLXMLProperty::GetValueBool()
{
	return !(value == L""
		|| value == L"0"
		|| value == L"false"
		|| value == L"False"
		|| value == L"FALSE")
		;
}

LLXMLNode::LLXMLNode(wstring name)
{
	this->name = name;
}

void LLXMLNode::AddProperty(LLXMLProperty* llProperty)
{
	propertyMap[llProperty->GetName()] = llProperty;
}

void LLXMLNode::RemoveProperty(wstring name)
{
	propertyMap.erase(name);
}

void LLXMLNode::SetInnerText(wstring innerText)
{
	this->innerText = innerText;
}

wstring LLXMLNode::GetInnerText()
{
	return innerText;
}

void LLXMLNode::AddNode(LLXMLNode* llNode)
{
	childNodeList.push_back(llNode);
}

void LLXMLNode::RemoveNode(LLXMLNode * llNode)
{
	childNodeList.remove(llNode);
}

unordered_map<wstring, LLXMLProperty*>& LLXMLNode::GetPropertyMap()
{
	return propertyMap;
}

list<LLXMLNode*>& LLXMLNode::GetChildNodeList()
{
	return childNodeList;
}

wstring LLXMLNode::GetName()
{
	return name;
}

LLXMLProperty * LLXMLNode::GetProperty(wstring name)
{
	return propertyMap[name];
}

bool LLXMLDocument::LoadXMLFromFile(wstring filePath, FileEncode fileEncode)
{
	while (!nodeStack.empty())
	{
		nodeStack.pop();
	}
	wifstream file(filePath, wifstream::binary);
	//在Windows下，文件中回车是“\n\r”,在获得文件长度时是2，读取却当成一个字符，有可能会影响编码判断。
	
	if (file)
	{
		if (fileEncode == FileEncode::UNKNOWN)
		{
			fileEncode = CheckFileEncode(file);
		}
		int markBufferNum = 0;
		int markBufferLength = 0;
		switch (fileEncode)
		{
		case FileEncode::ANSI:
			file.imbue(locale(""));
			break;
		case FileEncode::UTF_8_WITH_BOM:
			markBufferNum = 1;
			markBufferLength = 3;
		case FileEncode::UTF_8_NO_BOM:
			file.imbue(locale(locale::empty(), new codecvt_utf8<wchar_t>));
			break;
		case FileEncode::UTF_16_LITTLE_ENDIAN:
			//先空出来。
			markBufferNum = 1;
			markBufferLength = 1;
			break;
		case FileEncode::UTF_16_BIG_ENDIAN:
			markBufferNum = 1;
			markBufferLength = 1;
			file.imbue(locale(locale::empty(), new codecvt_utf16<wchar_t>));
			break;
		default:
			break;
		}
		file.seekg(0, ios_base::end); // 移动到文件尾。
		int fileLength = file.tellg(); // 取得当前位置的指针长度，即文件长度。
		file.seekg(0, ios_base::beg);
		fileLength++;//需要多一位来存储文件结尾标记。
		wchar_t* fileBuffer = new wchar_t[fileLength];
		//使用file.read(fileBuffer, fileLength);读取文件时,正文部分正常，结尾处多出一串乱码，不知道原因。
		file.get(fileBuffer, fileLength,EOF);
		wchar_t* fileBufferStart = fileBuffer;
		wchar_t* fileBufferEnd = fileBuffer + fileLength;
		fileBuffer += markBufferNum;
		fileLength -= markBufferLength;

		//开始分析语法。
		while (fileBuffer<fileBufferEnd&&fileLength>0 &&* fileBuffer != L'\0')
		{
			if (WCharCanIgnore(*fileBuffer))
			{
				fileBuffer++;
				fileLength--;
			}
			else if(!LoadUnknown(fileBuffer, fileLength))
			{
				delete[] fileBufferStart;
				fileBuffer = NULL;
				fileBufferStart = NULL;
				fileBufferEnd = NULL;
				file.close();
				return false;
			}
		}
		delete[] fileBufferStart;
		file.close();
		return true;
	}
	else
	{
		return false;
	}
	return false;
}

bool LLXMLDocument::SaveXMLToFile(wstring filePath, FileEncode fileEncode, bool writeDefine)
{
	wofstream file(filePath);
	if (file)
	{
		switch (fileEncode)
		{
		case FileEncode::ANSI:
			file.imbue(locale(""));
			break;
		case FileEncode::UTF_8_WITH_BOM:
		case FileEncode::UTF_8_NO_BOM:
			file.imbue(locale(locale::empty(), new codecvt_utf8<wchar_t>));
			break;
		case FileEncode::UTF_16_LITTLE_ENDIAN:
			break;
		case FileEncode::UTF_16_BIG_ENDIAN:
			file.imbue(locale(locale::empty(), new codecvt_utf16<wchar_t>));
			break;
		default:
			break;
		}
		if (rootNode)
		{
			if (writeDefine)//编码不是utf_8的话也这么写不太好，但用不到定义内容，追求细节的话就做一下判断吧！
			{
				file << L"<?xml version=\"1.0\" encoding=\"utf-8\"?>" << endl;
			}
			SaveNode(file,rootNode,0);
		}
		else
		{
			file.close();
			return false;
		}
		file.close();
	}



	return false;
}

LLXMLNode * LLXMLDocument::GetRootNode()
{
	return rootNode;
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
		int fileLength = (int)file.tellg(); // 取得当前位置的指针长度，即文件长度。
		if (fileLength == -1)
		{
			return fileEncode;
		}
		fileLength++;//需要多一位来存储文件结尾标记。
		file.seekg(0, ios_base::beg); // 移动到文件头。
		wchar_t* fileBuffer = new wchar_t[fileLength];
		wchar_t* fileBufferStartPos = fileBuffer;
		file.get(fileBuffer, fileLength,EOF);
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
		file.seekg(0, ios_base::beg);
	}
	return fileEncode;
}

bool LLXMLDocument::WCharCanIgnore(wchar_t wc)
{
	return (wc == L' ')//半角空格 
		|| (wc == L'　') //全角空格（输入法快捷键Shift+Space可以切换半角和全角）
		|| wc == L'\n' //换行
		|| wc == L'\r'//回车（“\r”和“\r\n”编码一样）
		|| wc == L'\t'//水平制表符
		;
}

bool LLXMLDocument::WCharIsLegalNameStart(wchar_t wc)
{
	return (L'a' <= wc&&wc <= L'z') 
		|| (L'A' <= wc&&wc <= L'Z')
		;
}

bool LLXMLDocument::WCharIsLegalName(wchar_t wc)
{
	return (L'a' <= wc&&wc <= L'z') 
		|| (L'A' <= wc&&wc <= L'Z')
		|| (L'0' <= wc&&wc <= L'9')
		|| (wc == L'_')
		;
}

bool LLXMLDocument::LoadUnknown(wchar_t*& fileBuffer, int& bufferSize)
{
	if (*fileBuffer == L'<')
	{
		fileBuffer++;
		bufferSize--;
		if (*fileBuffer==L'?')
		{
			fileBuffer++;
			bufferSize--;
			LoadDefine(fileBuffer, bufferSize);
		}
		else if (*fileBuffer == L'!')
		{
			if (*(fileBuffer + 1) == L'-'&&*(fileBuffer + 2) == L'-')
			{
				fileBuffer+=3;
				bufferSize-=3;
				LoadComment(fileBuffer, bufferSize);
			}
			else
			{
				return false;
			}
		}
		else if(*fileBuffer == L'/')
		{
			fileBuffer++;
			bufferSize--;
			wchar_t* nameStart = fileBuffer;
			while (WCharIsLegalName(*fileBuffer))
			{
				fileBuffer++;
				bufferSize--;
				if (bufferSize == 0)
				{
					return false;
				}
			}
			wstring nodeName = wstring(nameStart, fileBuffer - nameStart);
			if (nodeStack.top()->GetName() == nodeName&& *fileBuffer==L'>')
			{
				nodeStack.pop();
				fileBuffer++;
				bufferSize--;
			}
			else
			{
				return false;
			}
		}
		else
		{
			LoadNode(fileBuffer, bufferSize);
		}
	}
	else if(!nodeStack.empty())
	{
		wchar_t* textStart = fileBuffer;
		while (*fileBuffer!=L'<')
		{
			fileBuffer++;
			bufferSize--;
			if (bufferSize == 0)
			{
				return false;
			}
		}
		wstring innerText = wstring(textStart, fileBuffer - textStart);
		nodeStack.top()->SetInnerText(FormatWStringFromXML(innerText));
	}
	else
	{
		return false;
	}
	return true;
}

bool LLXMLDocument::LoadDefine(wchar_t*& fileBuffer, int& bufferSize)
{
	while (*fileBuffer != L'>')
	{
		fileBuffer++;
		bufferSize--;
		if (bufferSize == 0)
		{
			return false;
		}
	}
	fileBuffer++;
	bufferSize--;
	return true;
}

bool LLXMLDocument::LoadComment(wchar_t *& fileBuffer, int & bufferSize)
{
	while (!((*fileBuffer == L'-')&& (*(fileBuffer+1) == L'-')&&(*(fileBuffer + 2) == L'>')))
	{
		fileBuffer++;
		bufferSize--;
		if (bufferSize == 0)
		{
			return false;
		}
	}
	fileBuffer+=3;
	bufferSize-=3;
	return true;
}

bool LLXMLDocument::LoadNode(wchar_t*& fileBuffer, int& bufferSize)
{
	if (WCharIsLegalNameStart(*fileBuffer))
	{
		wchar_t* nameStart = fileBuffer;
		fileBuffer++;
		bufferSize--;
		while (WCharIsLegalName(*fileBuffer))
		{
			fileBuffer++;
			bufferSize--;
			if (bufferSize == 0)
			{
				return false;
			}
		}
		wstring nodeName = wstring(nameStart, fileBuffer- nameStart);
		LLXMLNode* node = new LLXMLNode(nodeName);
		if (!nodeStack.empty())
		{
			nodeStack.top()->AddNode(node);
		}
		else
		{
			rootNode = node;
		}
		nodeStack.push(node);
		while (true)
		{
			while (WCharCanIgnore(*fileBuffer))
			{
				fileBuffer++;
				bufferSize--;
				if (bufferSize == 0)
				{
					return false;
				}
			}
			if (*fileBuffer == L'>')
			{
				fileBuffer ++;
				bufferSize --;
				return true;
			}
			else if (*fileBuffer == L'/'&&*(fileBuffer + 1)==L'>')
			{
				fileBuffer += 2;
				bufferSize -= 2;
				nodeStack.pop();
				return true;
			}
			else
			{
				if (!LoadProperty(fileBuffer, bufferSize))
				{
					return false;
				}
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}

bool LLXMLDocument::LoadProperty(wchar_t *& fileBuffer, int & bufferSize)
{
	if (WCharIsLegalNameStart(*fileBuffer))
	{
		wchar_t* nameStart = fileBuffer;
		fileBuffer++;
		bufferSize--;
		while (WCharIsLegalName(*fileBuffer))
		{
			fileBuffer++;
			bufferSize--;
			if (bufferSize == 0)
			{
				return false;
			}
		}
		wstring propertyName = wstring(nameStart, fileBuffer - nameStart);
		LLXMLProperty* llProperty = new LLXMLProperty(propertyName);
		while (*fileBuffer != L'=')
		{
			fileBuffer++;
			bufferSize--;
			if (bufferSize == 0)
			{
				return false;
			}
		}
		fileBuffer++;
		bufferSize--;
		while (WCharIsLegalName(*fileBuffer))
		{
			fileBuffer++;
			bufferSize--;
			if (bufferSize == 0)
			{
				return false;
			}
		}
		llProperty->SetValue(LoadPropertyValue(fileBuffer, bufferSize));
		if (bufferSize == 0)
		{
			return false;
		}
		if (!nodeStack.empty())
		{
			nodeStack.top()->AddProperty(llProperty);
		}
		else
		{
			return false;
		}
	}
	else
	{
		return false;
	}
	return true;
}

wstring LLXMLDocument::LoadPropertyValue(wchar_t*& fileBuffer, int& bufferSize)
{
	wchar_t startWChar;
	if (*fileBuffer == L'"')
	{
		startWChar = L'"';
	}
	else if(*fileBuffer == L'\'')
	{
		startWChar = L'\'';
	}
	else
	{
		return wstring(L"");
	}
	fileBuffer++;
	bufferSize--;

	wchar_t* valueStart = fileBuffer;
	while (*fileBuffer != startWChar)
	{
		fileBuffer++;
		bufferSize--;
		if (bufferSize == 0)
		{
			return false;
		}
	}
	wstring value = wstring(valueStart, fileBuffer - valueStart);
	fileBuffer++;
	bufferSize--;
	return FormatWStringFromXML(value);
}

wstring LLXMLDocument::FormatWStringFromXML(wstring ws)
{
	wsstream.str(L"");//需要进行两步操作，只用clear的话wsstream内的保存的字符串是不变的。
	wsstream.clear();//需要清空缓存
	int curPos = 0;
	int wsSize = ws.size();
	while (curPos<wsSize)
	{
		if (ws[curPos] == L'&')
		{
			if (ws.substr(curPos,6)==L"&quot;")
			{
				wsstream << L'"';
				curPos += 6;
			}
			else if (ws.substr(curPos, 6) == L"&apos;")
			{
				wsstream << L'\'';
				curPos += 6;
			}
			else if (ws.substr(curPos, 5) == L"&amp;")
			{
				wsstream << L'&';
				curPos += 5;
			}
			else if (ws.substr(curPos, 4) == L"&lt;")
			{
				wsstream << L'<';
				curPos += 4;
			}
			else if (ws.substr(curPos, 4) == L"&gt;")
			{
				wsstream << L'>';
				curPos += 4;
			}
			else
			{
				wsstream << ws[curPos];
				curPos++;
			}
		}
		else
		{
			wsstream << ws[curPos];
			curPos++;
		}
	}
	return wsstream.str();
}

wstring LLXMLDocument::FormatWStringToXML(wstring ws)
{
	wsstream.str(L"");//需要进行两步操作，只用clear的话wsstream内的保存的字符串是不变的。
	wsstream.clear();//需要清空缓存
	int curPos = 0;
	int wsSize = ws.size();
	while (curPos < wsSize)
	{
		if (ws[curPos] == L'"')
		{
			wsstream << L"&quot;";
		}
		else if (ws[curPos] == L'\'')
		{
			wsstream << L"&apos;";
		}
		else if (ws[curPos] == L'&')
		{
			wsstream << L"&amp;";
		}
		else if (ws[curPos] == L'<')
		{
			wsstream << L"&lt;";
		}
		else if (ws[curPos] == L'>')
		{
			wsstream << L"&gt;";
		}
		else
		{
			wsstream << ws[curPos];
		}
		curPos++;
	}
	return wsstream.str();
}

void LLXMLDocument::SaveNode(wofstream& file, LLXMLNode* node, int depth)
{
	for (int i=0;i<depth;i++)
	{
		file << L'\t';
	}
	file << L"<" + node->GetName() ;
	if (node->GetPropertyMap().size()!=0)
	{
		file << L" ";
		for (auto var : node->GetPropertyMap())
		{
			SaveProperty(file, var.second);
			file << L" ";
		}
	}
	bool tempBool = false;
	if (node->GetInnerText() != L""|| node->GetChildNodeList().size() > 0)
	{
		tempBool = true;
	}
	if (!tempBool)
	{
		file << L"/>"<< endl;
		return;
	}
	file <<L">"<< endl;
	if (node->GetInnerText() != L"")
	{
		for (int i = 0; i < depth+1; i++)
		{
			file << L'\t';
		}
		file << node->GetInnerText() << endl;
	}
	for (auto var : node->GetChildNodeList())
	{
		SaveNode(file, var,depth+1);
	}
	for (int i = 0; i < depth; i++)
	{
		file << L'\t';
	}
	file << L"</" << node->GetName() << L">"<<endl;
}

void LLXMLDocument::SaveProperty(wofstream & file, LLXMLProperty * llProperty)
{
	file << llProperty->GetName() << L"=" << L'"' << FormatWStringToXML(llProperty->GetValue()) << L'"';
}
