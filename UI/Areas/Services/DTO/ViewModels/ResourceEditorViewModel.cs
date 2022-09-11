using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Entities;

namespace UI.Areas.Services.DTO.ViewModels
{
    public class ResourceEditorViewModel
    {
        public ResourceEditorViewModel()
        {
            
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
