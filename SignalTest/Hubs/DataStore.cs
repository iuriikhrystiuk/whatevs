using System;
using System.Runtime.Caching;

namespace SignalTest.Hubs
{
    public class DatatItem
    {
        public Guid Guid { get; set; }

        public string Data { get; set; }
    }

    public class DataStore
    {
        private static DatatItem currentDatatItem = new DatatItem();

        public DatatItem GetDataItem(Guid guid)
        {
            lock (currentDatatItem)
            {
                // stat of application
                if (currentDatatItem.Guid == Guid.Empty)
                {
                    currentDatatItem = new DatatItem { Guid = Guid.NewGuid(), Data = new Random().Next().ToString() };
                }

                return currentDatatItem;
            }
        }

        public Guid RefreshData()
        {
            lock (currentDatatItem)
            {
                currentDatatItem = new DatatItem { Guid = Guid.NewGuid(), Data = new Random().Next().ToString() };
                return currentDatatItem.Guid;
            }
        }
    }
}