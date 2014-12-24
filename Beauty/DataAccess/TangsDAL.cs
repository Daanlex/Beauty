using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace Beauty.DataAccess
{
    public class TangsDAL
    {
        const string _dbName = "bb.db";
        public void CreateTable()
        {
            using (var con = new Connection().GetConnection)
            {
                con.Execute("create table BaseInfomation (bb int not null)", null);                
            }
        }

        public List<T> Select<T>(string name,string hospital,string startTime,string endTime)
        {
            return new List<T>();
        }



    }
}
