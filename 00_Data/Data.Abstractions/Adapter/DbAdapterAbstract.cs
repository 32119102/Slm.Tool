﻿using Data.Abstractions.Entities;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using DbColumn = Data.Abstractions.Entities.DbColumn;

namespace Data.Abstractions.Adapter
{
    public abstract class DbAdapterAbstract<SugarParameter>
    {
        public abstract DbProvider Provider { get; }

        /// <summary>
        /// 执行Sql语句获取DataTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public abstract Task<DataTable> GetDataTable(string connectionString, string commandText, params SugarParameter[] parms);

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public abstract Task<List<DbTable>> GetDbTables(string connectionString, string tables = "");

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract Task<List<DbColumn>> GetDbColumns(string connectionString, string tableName, DbTable table);
    }
}