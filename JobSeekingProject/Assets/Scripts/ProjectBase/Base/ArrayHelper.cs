﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class ArrayHelper
    {
        //查找，升序，降序
        //最大值，最小值，筛选

        /// <summary>
        /// 查找满足条件的单个元素
        /// </summary>
        /// <typeparam name="T">  数组的类型  </typeparam>
        /// <param name="array">  数组  </param>
        /// <param name="condition">  查找条件  </param>
        /// <returns></returns>
        public static T Find<T>(this T[]array, Func<T,bool> condition)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                    return array[i];
            }
            return default(T);
        }

        /// <summary>
        /// 查找满足条件的所有元素
        /// </summary>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
        {
            List<T> list = new List<T>();
            for(int i = 0; i < array.Length; ++i)
            {
                if (condition(array[i]))
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="T">  代表的数组的类型  </typeparam>
        /// <typeparam name="Q">  比较条件的返回值类型  </typeparam>
        /// <param name="array">  数组  </param>
        /// <param name="condition">  要比较的方法  </param>
        /// <returns></returns>
        public static T GetMax<T,Q>(this T[] array,Func<T,Q> condition)where Q : IComparable
        {
            T max = array[0];
            for(int i = 0; i < array.Length; ++i)
            {
                if (condition(max).CompareTo(condition(array[i])) < 0)
                    max = array[i];
            }
            return max;
        } 

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <returns></returns>
        public static T GetMin<T,Q>(this T[] array,Func<T,Q> condition)where Q : IComparable
        {
            T min = array[0];
            for(int i = 0; i < array.Length; ++i)
            {
                if (condition(min).CompareTo(condition(array[i])) > 0)
                    min = array[i];
            }
            return min;
        }

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="T">  数组类型  </typeparam>
        /// <typeparam name="Q">  返回值类型  </typeparam>
        /// <param name="array">  数组  </param>
        /// <param name="condition">  委托类型  </param>
        public static void OrderBy<T,Q>(this T[]array,Func<T,Q>condition)where Q: IComparable
        {
            for(int i = 0; i < array.Length; ++i)
            {
                for(int j = 0; j < array.Length - 1 - i; ++j)
                {
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                } 
            }
        }

        /// <summary>
        /// 降序
        /// </summary>
        public static void OrderDescding<T,Q>(this T[]array,Func<T,Q> condition)where Q : IComparable
        {
            for(int i = 0; i < array.Length; ++i)
            {
                for(int j = 0; j < array.Length - 1 - i; ++j)
                {
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 筛选
        /// </summary>
        public static Q[] Select<T,Q>(this T[] array, Func<T, Q> condition)
        {
            Q[] result = new Q[array.Length];
            for(int i = 0; i < array.Length; ++i)
            {
                result[i] = condition(array[i]);
            }
            return result;
        }
    }
}