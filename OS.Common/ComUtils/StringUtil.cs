﻿
using System;
using System.Text;

namespace OS.Common.Utilss
{
    public static class StringUtil
    {
        #region Filter

        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string source)
        {
            source = source.Replace("\"", "");
            source = source.Replace("&", "&amp");
            source = source.Replace("<", "&lt");
            source = source.Replace(">", "&gt");
            source = source.Replace("delete", "");
            source = source.Replace("update", "");
            source = source.Replace("insert", "");
            source = source.Replace("'", "''");
            source = source.Replace(";", "；");
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");
            source = source.Replace("0x", "0 x");
            return source;
        }

        #endregion

        #region 字符串转码

        /// <summary>
        ///    根据指定编码转化成对应的64位编码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64(this string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source", "转化Base64字符串不能为空");
            }
            byte[] bytes = encoding.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///    从base64编码解码出正常的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string FromBase64(this string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source", "解码Base64字符串不能为空");
            }
            byte[] bytes = encoding.GetBytes(source);
            return encoding.GetString(bytes);
        }

        #endregion
    }
}