﻿using System;

namespace Data.Adapters.Mysql.Helper
{
    public class MysqlDbTypeMap
    {
        public static string MapCsharpType(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype))
            {
                return dbtype;
            }
            //如果是枚举
            if (dbtype.Contains("Enum"))
            {
                return dbtype;
            }
            dbtype = dbtype.ToLower();

            string csharpType = "object";
            switch (dbtype)
            {
                case "int": csharpType = "int"; break;
                case "mediumint": csharpType = "int"; break;
                case "integer": csharpType = "int"; break;

                case "varchar": csharpType = "string"; break;
                case "text": csharpType = "string"; break;
                case "char": csharpType = "string"; break;
                case "enum": csharpType = "string"; break;
                case "mediumtext": csharpType = "string"; break;
                case "tinytext": csharpType = "string"; break;
                case "longtext": csharpType = "string"; break;

                case "tinyint": csharpType = "byte[]"; break;
                case "smallint": csharpType = "short"; break;
                case "bigint": csharpType = "long"; break;
                case "bit": csharpType = "bool"; break;
                case "real": csharpType = "double"; break;
                case "double": csharpType = "double"; break;
                case "float": csharpType = "float"; break;
                case "decimal": csharpType = "decimal"; break;
                case "numeric": csharpType = "decimal"; break;
                case "year": csharpType = "int"; break;

                case "datetime": csharpType = "DateTime"; break;
                case "timestamp": csharpType = "DateTime"; break;
                case "date": csharpType = "DateTime"; break;
                case "time": csharpType = "DateTime"; break;

                case "blob": csharpType = "byte[]"; break;
                case "longblob": csharpType = "byte[]"; break;
                case "tinyblob": csharpType = "byte[]"; break;
                case "varbinary": csharpType = "byte[]"; break;
                case "binary": csharpType = "byte[]"; break;
                case "multipoint": csharpType = "byte[]"; break;
                case "geometry": csharpType = "byte[]"; break;
                case "multilinestring": csharpType = "byte[]"; break;
                case "polygon": csharpType = "byte[]"; break;
                case "mediumblob": csharpType = "byte[]"; break;

                default: csharpType = "object"; break;
            }
            return csharpType;
        }

        public static string VueCsharpType(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype))
            {
                return dbtype;
            }
            //如果是枚举
            if (dbtype.Contains("Enum"))
            {
                return dbtype;
            }
            dbtype = dbtype.ToLower();
            string csharpType = "any";
            switch (dbtype)
            {
                case "bigint": csharpType = "string"; break;
                case "nvarchar": csharpType = "string"; break;
                case "varchar": csharpType = "string"; break;
                case "text": csharpType = "string"; break;
                case "char": csharpType = "string"; break;
                case "enum": csharpType = "string"; break;

                case "int": csharpType = "number"; break;
                case "double": csharpType = "number"; break;
                case "float": csharpType = "number"; break;
                case "decimal": csharpType = "number"; break;
                case "numeric": csharpType = "number"; break;

                case "bit": csharpType = "boolean"; break;

                case "datetime": csharpType = "Date"; break;
                case "timestamp": csharpType = "Date"; break;
                case "date": csharpType = "Date"; break;
                case "time": csharpType = "Date"; break;

                default: csharpType = "any"; break;
            }
            return csharpType;
        }

        public static string VueDefault(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype))
            {
                return dbtype;
            }
            //如果是枚举
            if (dbtype.Contains("Enum"))
            {
                return "0";
            }
            dbtype = dbtype.ToLower();
            string csharpType = "any";
            switch (dbtype)
            {
                case "bigint": csharpType = "''"; break;
                case "nvarchar": csharpType = "''"; break;
                case "varchar": csharpType = "''"; break;
                case "text": csharpType = "''"; break;
                case "char": csharpType = "''"; break;
                case "enum": csharpType = "''"; break;

                case "int": csharpType = "0"; break;
                case "double": csharpType = "0"; break;
                case "float": csharpType = "0"; break;
                case "decimal": csharpType = "0"; break;
                case "numeric": csharpType = "0"; break;

                case "bit": csharpType = "false"; break;

                case "datetime": csharpType = "null"; break;
                case "timestamp": csharpType = "null"; break;
                case "date": csharpType = "null"; break;
                case "time": csharpType = "null"; break;

                default: csharpType = "null"; break;
            }
            return csharpType;
        }

        public static Type MapCommonType(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype))
            {
                return Type.Missing.GetType();
            }

            dbtype = dbtype.ToLower();
            Type commonType = typeof(object);
            switch (dbtype)
            {
                case "int": commonType = typeof(int); break;
                case "mediumint": commonType = typeof(int); break;
                case "integer": commonType = typeof(int); break;

                case "varchar": commonType = typeof(string); break;
                case "text": commonType = typeof(string); break;
                case "char": commonType = typeof(string); break;
                case "enum": commonType = typeof(string); break;
                case "mediumtext": commonType = typeof(string); break;
                case "tinytext": commonType = typeof(string); break;
                case "longtext": commonType = typeof(string); break;

                case "tinyint": commonType = typeof(byte[]); break;
                case "smallint": commonType = typeof(short); break;
                case "bigint": commonType = typeof(long); break;
                case "bit": commonType = typeof(bool); break;
                case "real": commonType = typeof(double); break;
                case "double": commonType = typeof(double); break;
                case "float": commonType = typeof(float); break;
                case "decimal": commonType = typeof(decimal); break;
                case "numeric": commonType = typeof(decimal); break;
                case "year": commonType = typeof(int); break;

                case "datetime": commonType = typeof(DateTime); break;
                case "timestamp": commonType = typeof(DateTime); break;
                case "date": commonType = typeof(DateTime); break;
                case "time": commonType = typeof(DateTime); break;

                case "blob": commonType = typeof(byte[]); break;
                case "longblob": commonType = typeof(byte[]); break;
                case "tinyblob": commonType = typeof(byte[]); break;
                case "varbinary": commonType = typeof(byte[]); break;
                case "binary": commonType = typeof(byte[]); break;
                case "multipoint": commonType = typeof(byte[]); break;
                case "geometry": commonType = typeof(byte[]); break;
                case "multilinestring": commonType = typeof(byte[]); break;
                case "polygon": commonType = typeof(byte[]); break;
                case "mediumblob": commonType = typeof(byte[]); break;

                default: commonType = typeof(object); break;
            }
            return commonType;
        }
    }
}