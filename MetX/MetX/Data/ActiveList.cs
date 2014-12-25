/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

namespace Metta.SubSonic
{
    /// <summary>C#CD: </summary>
    [Serializable]
    public class ActiveList<ItemType> : List<ItemType> where ItemType : ActiveRecord<ItemType>, new()
    {
        /// <summary>C#CD: </summary>
        /// <param name="rdr">C#CD: </param>
        public ActiveList<ItemType> Load(IDataReader rdr)
        {
            ItemType item = null;
            while (rdr.Read())
            {
                item = new ItemType();
                item.Load(rdr);
                item.IsNew = false;
                this.Add(item);
            }
            rdr.Close();
            rdr.Dispose();
            return this;
        }

        /// <summary>C#CD: </summary>
        /// <param name="tbl">C#CD: </param>
        public void Load(DataTable tbl)
        {
            ItemType item = null;
            foreach (DataRow dr in tbl.Rows)
            {
                item = new ItemType();
                item.Load(dr);
                item.IsNew = false;
                this.Add(item);
            }
        }
    }
}
*/