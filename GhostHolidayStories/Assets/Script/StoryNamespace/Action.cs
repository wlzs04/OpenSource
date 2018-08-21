﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    enum GameState
    {
        Controlled,//角色受控制，玩家无法控制角色，一般用在剧情中。
        Free,//角色由玩家控制。
    }

    enum MoveState
    {
        AI,
        Line,
        Jump
    }

    class Action
    {
        string actorName;
        string targetActorName;
        GameState gameState;
        bool isAsync = false;
        bool isAnimation = false;
        bool isTalk = false;

        string functionName;

        Vector2 position;

        public void LoadContent(XElement node)
        {
            foreach (var item in node.Attributes())
            {
                switch (item.Name.ToString())
                {
                    case "actor":
                        actorName = item.Value;
                        break;
                    case "gameState":
                        gameState = (GameState)Enum.Parse(typeof(GameState), item.Value);
                        break;
                    case "isAsync":
                        isAsync = Convert.ToBoolean(item.Value);
                        break;
                    case "setPosition":
                    case "moveToPosition":
                        functionName = item.Name.ToString();
                        int tempIndex = item.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(item.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(item.Value.Substring(tempIndex + 1));
                        position = new Vector2(x, y);
                        break;
                    case "setFollowActor":
                        functionName = item.Name.ToString();
                        targetActorName = item.Value;
                        break;
                    case "animationName":
                        actorName = item.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
