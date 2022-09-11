using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Indexes;

namespace UI.Areas.Services.DTO.ViewModels
{
    public class EmployeeContactViewModel
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public DateTime DateOfBirth { get; set; }

        private NumericKeyValuePair _nationality;

       
        public NumericKeyValuePair Nationality
        {
            get
            {
                if (this._nationality == null)
                {
                    var o = new NumericKeyValuePair {Key = 0, Value = string.Empty};

                    return o;
                }

                return this._nationality;
            }

            set
            {
                this._nationality = value;
            }
        }
    }
}