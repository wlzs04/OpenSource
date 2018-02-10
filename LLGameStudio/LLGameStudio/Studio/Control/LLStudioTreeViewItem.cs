using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LLGameStudio.Studio.Control
{
    public class LLStudioTreeViewItem : TreeViewItem
    {
        IUINode uiNode;
        public void SetUINodeItem(IUINode uiNode)
        {
            this.uiNode = uiNode;
            Header = uiNode.name.Value;
        }

        public IUINode GetUINode()
        {
            return uiNode;
        }
    }
}
