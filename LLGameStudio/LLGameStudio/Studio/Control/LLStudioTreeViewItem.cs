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
        object uiNode;
        public void SetUINodeItem(object uiNode)
        {
            this.uiNode = uiNode;
        }

        public object GetUINode()
        {
            return uiNode;
        }
    }
}
