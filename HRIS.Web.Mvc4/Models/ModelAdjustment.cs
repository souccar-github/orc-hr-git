using  Project.Web.Mvc4.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 namespace Project.Web.Mvc4.Models
{
    public class ModelAdjustment
    {
        public virtual void AdjustModule(Module module)
            {

            }
        public virtual ViewModel AdjustGridModel(string type)
        {
            return new ViewModel();
        }
        }
    }