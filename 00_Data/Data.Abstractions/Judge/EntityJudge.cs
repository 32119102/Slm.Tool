using Data.Abstractions.Entities;
using Data.Abstractions.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Abstractions.Judge
{
    public class EntityJudge
    {
        public static string BaseConfig(DbTable dbTable)
        {
            string baseEntity = string.Empty;
            var col = new List<string>
            {
                //"Created","Creator","CreatId","Lastmodified",
                //"Lastmodifier","LastmodifiId","IsDeleted","Deleted","Deletor","DeletId"
            };
            col = col.Select(a => a.ToLower()).ToList();
            var columns = dbTable.DbColumns.Select(x => x.ColumnName.ToLower()).ToList();
            bool isExcept = !col.Except(columns).Any();
            if (isExcept)
            {
                switch (dbTable.CShareType)
                {
                    case "long":
                        baseEntity += ":EntityOperation<long>";
                        break;

                    case "int":
                        baseEntity += ":EntityOperation<int>";
                        break;

                    case "guid":
                        baseEntity += ":EntityOperation<Guid>";
                        break;

                    default:
                        break;
                }
            }
            else
            {
                baseEntity += "";
            }
            return baseEntity;
        }

        public static string IRepository(DbTable dbTable)
        {
            string baseRepository = string.Empty;

            switch (dbTable.CShareType)
            {
                case "long":
                    baseRepository += "ILongRepository";
                    break;

                case "int":
                    baseRepository += "IIntRepository";
                    break;

                case "guid":
                    baseRepository += "IGuidRepository";
                    break;

                default:
                    break;
            }

            return baseRepository;
        }

        public static string Repository(DbTable dbTable)
        {
            string baseRepository = string.Empty;

            switch (dbTable.CShareType)
            {
                case "long":
                    baseRepository += "LongRepositoryAbstract";
                    break;

                case "int":
                    baseRepository += "IntRepositoryAbstract";
                    break;

                case "guid":
                    baseRepository += "GuidRepositoryAbstract";
                    break;

                default:
                    break;
            }

            return baseRepository;
        }

        public static List<string> IgnoreCol(bool isBase = true)
        {
            if (isBase)
            {
                return new List<string>
            {
                //"Id".ToLower(),"Created".ToLower(),"Creator".ToLower(),"CreatId".ToLower(),
                //    "Lastmodified".ToLower(),"Lastmodifier".ToLower(),"LastmodifiId".ToLower(),"IsDeleted".ToLower(),
                //    "Deleted".ToLower(),"Deletor".ToLower(),"DeletId".ToLower()
            };
            }
            else
            {
                return new List<string>();
            }
        }

        public static string GetIsNull(DbColumn column)
        {
            if (column.IsNullable && column.CommonType != typeof(Enum))
            {
                return column.CShareType + "?";
            }
            else
            {
                return column.CShareType;
            }
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpper(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string[] arry = value.Split('_');
            string str = "";
            foreach (string item in arry)
            {
                string newstr = item.Replace("(", "").Replace(".", "").Replace(")", "");
                string firstLetter = newstr.Substring(0, 1);
                string rest = newstr.Substring(1, newstr.Length - 1).ToLower();
                str += firstLetter.ToUpper() + rest;
            }
            return str;
        }

        public static string ToFirstUpper(string value)
        {
            return value.FirstCharToUpper();
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLower(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string[] arry = value.Split('_');
            string str = "";
            int i = 0;
            foreach (string item in arry)
            {
                string newstr = item.Replace("(", "").Replace(".", "").Replace(")", "");
                string firstLetter = newstr.Substring(0, 1);
                string rest = newstr.Substring(1, newstr.Length - 1).ToLower();
                if (i == 0)
                {
                    str += firstLetter.ToLower() + rest;
                }
                else
                {
                    str += firstLetter.ToUpper() + rest;
                }
                i++;
            }
            return str;
        }

        public static string IsPrimaryKey(bool isIsPrimaryKey)
        {
            string str = string.Empty;
            if (isIsPrimaryKey)
            {
                str += ",IsPrimaryKey = true";
            }
            str += ")]";
            return str;
        }
    }
}