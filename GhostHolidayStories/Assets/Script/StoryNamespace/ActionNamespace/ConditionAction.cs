using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 条件指令：用于判断当前状态是否符合指定要求
    /// </summary>
    class ConditionAction : ActionBase
    {
        string expression = "";//条件表达式
        ConditionNode conditionNode=null;
        bool result = false;
        TrueAction trueAction = null;
        FalseAction falseAction = null;
        ActionBase resultAction = null;
        
        public ConditionAction() : base("Condition")
        {
        }

        public override void Update()
        {
            base.Update();
            if(resultAction != null&& !resultAction.IsCompleted())
            {
                resultAction.Update();
            }
            else
            {
                Complete();
            }
        }

        protected override void Execute(ActorBase executor)
        {
            result = conditionNode.GetResult();
            if (result && trueAction != null)
            {
                resultAction = trueAction;
                trueAction.Execute();
            }
            else if (!result && falseAction != null)
            {
                resultAction = falseAction;
                falseAction.Execute();
            }
            else
            {
                Complete();
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            ConditionAction action = new ConditionAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "expression":
                        expression = attribute.Value;
                        conditionNode = new ConditionNode();
                        int currentPos = 0;
                        conditionNode.SetConditionExpression(expression,ref currentPos);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in node.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "True":
                        if(trueAction!=null)
                        {
                            GameManager.ShowErrorMessage("Condition中出现多个真指令节点。");
                        }
                        trueAction = (TrueAction)ActionBase.LoadAction(item);
                        break;
                    case "False":
                        if (falseAction != null)
                        {
                            GameManager.ShowErrorMessage("Condition中出现多个假指令节点。");
                        }
                        falseAction = (FalseAction)ActionBase.LoadAction(item);
                        break;
                    default:
                        GameManager.ShowErrorMessage("Condition中出现未知子节点：" + item.Name.ToString());
                        break;
                }
            }
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }

        public override void Init()
        {
            base.Init();
            result = false;
            if(resultAction!=null)
            {
                resultAction.Init();
                resultAction = null;
            }
        }
        
        /// <summary>
        /// 执行判断条件的方法
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public static object ExecuteConditionFunction(string functionName,params object[] parameterList)
        {
            if(functionName== "GetObjectNumberById")
            {
                return DirectorActor.GetInstance().GetObjectNumberById(Convert.ToInt32(parameterList[0]));
            }
            else if(functionName == "HaveFinishChapterAndSection")
            {
                return DirectorActor.GetInstance().HaveFinishChapterAndSection(Convert.ToInt32(parameterList[0]), Convert.ToInt32(parameterList[1]));
            }
            GameManager.ShowErrorMessage("在条件指令表达式中存在未知方法："+functionName);
            return 0;
        }
    }

    /// <summary>
    /// 比较类型
    /// </summary>
    enum ComparerType
    {
        None,//无
        Equal,//相等
        MoreThan,//大于
        LessThan,//小于
        MoreThanOrEqual,//大于等于
        LessThanOrEqual,//小于等于
    }

    /// <summary>
    /// 条件类型
    /// </summary>
    enum ConditionType
    {
        None,//无
        And,//并且
        Or,//或者
    }

    /// <summary>
    /// 条件节点
    /// </summary>
    class ConditionNode
    {
        //当子条件节点数量为0时使用此节点判断真假，否则使用子节点判断

        //当使用自身条件判断时
        string leftFunctionName = "";
        object[] leftParamList;
        object leftValue = "";
        bool leftIsFunction = false;
        ComparerType comparerType;
        string rightFunctionName = "";
        object[] rightParamList;
        object rightValue = "";
        bool rightIsFunction = false;

        //当使用子条件节点判断时
        ConditionNode leftConditionNode = null;
        ConditionNode rightConditionNode = null;
        ConditionType conditionType;

        /// <summary>
        /// 获得此节点的真假
        /// </summary>
        /// <returns></returns>
        public bool GetResult()
        {
            if (conditionType == ConditionType.None)
            {
                if(leftIsFunction)
                {
                    leftValue = ConditionAction.ExecuteConditionFunction(leftFunctionName, leftParamList);
                }
                if (rightIsFunction)
                {
                    rightValue = ConditionAction.ExecuteConditionFunction(rightFunctionName, rightParamList);
                }
                switch (comparerType)
                {
                    case ComparerType.None:
                        return (bool)ConditionAction.ExecuteConditionFunction(leftFunctionName, leftParamList);
                    case ComparerType.Equal:
                        return leftValue == rightValue;
                    case ComparerType.MoreThan:
                        return Convert.ToDouble(leftValue) > Convert.ToDouble(rightValue);
                    case ComparerType.LessThan:
                        return Convert.ToDouble(leftValue) < Convert.ToDouble(rightValue);
                    case ComparerType.MoreThanOrEqual:
                        return Convert.ToDouble(leftValue) >= Convert.ToDouble(rightValue);
                    case ComparerType.LessThanOrEqual:
                        return Convert.ToDouble(leftValue) <= Convert.ToDouble(rightValue);
                    default:
                        break;
                }
            }
            else
            {
                bool leftResult = leftConditionNode.GetResult();
                bool rightResult = rightConditionNode.GetResult();
                if (conditionType == ConditionType.And)
                {
                    return leftResult && rightResult;
                }
                else if (conditionType == ConditionType.Or)
                {
                    return leftResult || rightResult;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置条件表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="currentCharPosition"></param>
        public void SetConditionExpression(string expression, ref int currentCharPosition)
        {
            ConditionType tempConditionType;
            ComparerType tempComparerType;
            
            //开始判断条件节点开始，以'('为起点
            if (expression[currentCharPosition]!='(')
            {
                GameManager.ShowErrorMessage("条件表达式："+expression+"语法不符合要求：每个判断条件都需要在()中。");
            }
            else
            {
                currentCharPosition++;
            }
            //循环判断
            while (currentCharPosition<expression.Length)
            {
                //开始子判断条件节点开始，以'('为起点
                if (expression[currentCharPosition] == '(')
                {
                    if(leftConditionNode==null)
                    {
                        leftConditionNode = new ConditionNode();
                        leftConditionNode.SetConditionExpression(expression, ref currentCharPosition);
                    }
                    else if(rightConditionNode==null)
                    {
                        rightConditionNode = new ConditionNode();
                        rightConditionNode.SetConditionExpression(expression, ref currentCharPosition);
                    }
                    else
                    {
                        GameManager.ShowErrorMessage("条件表达式"+ expression + "暂时仅支持在同一个节点下存在两个条件判断。");
                    }
                    continue;
                }
                //开始判断条件节点结束，以')'为起点
                if (expression[currentCharPosition] == ')')
                {
                    currentCharPosition++;
                    return;
                }

                //判断是否为条件符号
                tempConditionType = IsConditionSymbol(expression, ref currentCharPosition);
                if (tempConditionType != ConditionType.None)
                {
                    if (conditionType == ConditionType.None)
                    {
                        conditionType = tempConditionType;
                    }
                    else
                    {
                        GameManager.ShowErrorMessage("条件表达式" + expression + "中同一层级下出现一个以上条件符号。");
                    }
                    continue;
                }
                //判断是否为比较符号
                tempComparerType = IsComparerSymbol(expression, ref currentCharPosition);
                if (tempComparerType != ComparerType.None)
                {
                    if (comparerType == ComparerType.None)
                    {
                        comparerType = tempComparerType;
                    }
                    else
                    {
                        GameManager.ShowErrorMessage("条件表达式" + expression + "中同一层级下出现一个以上比较符号。");
                    }
                    continue;
                }
                //开始读取比较内容（方法或值）
                int startCharPosition = currentCharPosition;
                while (char.IsLetterOrDigit(expression[currentCharPosition]))
                {
                    currentCharPosition++;
                }
                //未读取比较方法代表正在读取左侧内容
                if(comparerType==ComparerType.None)
                {
                    //如果下一个值为'('，代表之前读取的内容为方法名称,否则为值
                    if (expression[currentCharPosition] == '(')
                    {
                        leftIsFunction = true;
                        leftFunctionName = expression.Substring(startCharPosition, currentCharPosition - startCharPosition);
                        int lastIndex = expression.IndexOf(')', currentCharPosition);
                        string paramString = expression.Substring(currentCharPosition+1, lastIndex - currentCharPosition-1);
                        leftParamList = paramString.Split(',');
                        currentCharPosition = lastIndex + 1;
                    }
                    else
                    {
                        leftValue = expression.Substring(startCharPosition, currentCharPosition - startCharPosition);
                    }
                }
                else
                {
                    //如果下一个值为'('，代表之前读取的内容为方法名称,否则为值
                    if (expression[currentCharPosition] == '(')
                    {
                        rightIsFunction = true;
                        rightFunctionName = expression.Substring(startCharPosition, currentCharPosition - startCharPosition);
                        int lastIndex = expression.IndexOf(')', currentCharPosition);
                        string paramString = expression.Substring(currentCharPosition + 1, lastIndex - currentCharPosition-1);
                        rightParamList = paramString.Split(',');
                        currentCharPosition = lastIndex + 1;
                    }
                    else
                    {
                        rightValue = expression.Substring(startCharPosition, currentCharPosition - startCharPosition);
                    }
                }
                continue;
            }
        }

        /// <summary>
        /// 判断在字符串指定位置处是否为条件符号
        /// </summary>
        /// <returns></returns>
        public ConditionType IsConditionSymbol(string content,ref int pos)
        {
            if (content.Substring(pos, 2).StartsWith("&&"))
            {
                pos = pos + 2;
                return ConditionType.And;
            }
            if (content.Substring(pos, 2).StartsWith("||"))
            {
                pos = pos + 2;
                return ConditionType.Or;
            }
            return ConditionType.None;
        }

        /// <summary>
        /// 判断在字符串指定位置处是否为比较符号
        /// </summary>
        /// <returns></returns>
        public ComparerType IsComparerSymbol(string content,ref int pos)
        {
            if(content[pos]=='<')
            {
                if(content[pos+1]=='=')
                {
                    pos = pos + 2;
                    return ComparerType.LessThanOrEqual;
                }
                else
                {
                    pos = pos + 1;
                    return ComparerType.LessThan;
                }
            }
            else if(content[pos] == '>')
            {
                if (content[pos+1] == '=')
                {
                    pos = pos + 2;
                    return ComparerType.MoreThanOrEqual;
                }
                else
                {
                    pos = pos + 1;
                    return ComparerType.MoreThan;
                }
            }
            else if(content[pos]=='=' && content[pos+1] == '=')
            {
                pos = pos + 2;
                return ComparerType.Equal;
            }
            return ComparerType.None;
        }
    }
}
