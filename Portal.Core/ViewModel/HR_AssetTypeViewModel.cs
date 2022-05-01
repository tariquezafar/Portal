using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_AssetTypeViewModel : IModel
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public bool AssetType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
