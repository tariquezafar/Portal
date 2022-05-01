﻿using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class StateViewModel:IModel
    {
       
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string message { get; set; }
        public string status { get; set; }

     
    }
    
}
