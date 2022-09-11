
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Project.Web.Mvc4.Factories
{
    public class CommandFactory
    {
        public static List<ActionListCommand> GetActionListCommands(Type type)
        {
            var result = new List<ActionListCommand>();
            var commands = type.GetCustomAttributes(typeof(CommandAttribute), false);
            foreach (var command in commands.Select(x => x as CommandAttribute).Where(x => !x.IsToolbarCommand))
            {
                result.Add(new ActionListCommand()
                {
                    GroupId = command.GroupId,
                    StyleClass = string.Format("action-list-{0}", command.Name),
                    ImageClass = string.Format("action-list-img-{0}", command.Name),
                    Order = command.Order,
                    HandlerName = command.Name,
                    Name =LocalizationHelper.GetResource(GlobalResorce.GetCommandResourceGroupName(), command.Name),
                    ShowCommand = true
                });
            }
            return result;
        }
       

        
    }
}