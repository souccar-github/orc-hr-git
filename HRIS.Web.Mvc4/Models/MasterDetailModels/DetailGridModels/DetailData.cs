using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels
{
    public class DetailData
    {
        public DetailData()
        {
            List = new List<DetailObj>();
            RemovedObjects = new List<DetailObj>();
            OldObjects = new List<DetailObj>();
        }
        public string DetailName { get; set; }
        public string TypeFullNameViewModel { get; set; }
        public IList<DetailObj> List { get; set; }

        public IList<DetailObj> OldObjects { get; set; }
        public IList<DetailObj> RemovedObjects { get; set; }
        public IList<DetailObj> UpdatedObjects
        {
            get
            {
                return List.Where(x => x.Properties.Any(y => y.PropName == "IsDirty"
                    && Convert.ToBoolean(y.Value) == true)).ToList();
            }
        }
        public IList<DetailObj> InsertedObjects
        {
            get
            {
                return List.Where(x => x.Properties.Any(y => y.PropName == "IsVirtualNew"
                    && Convert.ToBoolean(y.Value) == true)).ToList();
            }
        }
        public IList<int> UpdatedObjectsIds
        {
            get
            {
                return UpdatedObjects.SelectMany(x => x.Properties).Where(x => x.PropName == "Id").Select(x => Convert.ToInt32(x.Value)).ToList();
            }
        }

        public IList<int> InsertedObjectsIds
        {
            get
            {
                return InsertedObjects.SelectMany(x => x.Properties).Where(x => x.PropName == "Id").Select(x => Convert.ToInt32(x.Value)).ToList();

            }
        }
        public IList<int> DirtyIds
        {
            get
            {
                return InsertedObjectsIds.Concat(InsertedObjectsIds).ToList();
            }
        }








    }
}