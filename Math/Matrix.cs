﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Math
{
    using NUM = System.Int64;
    public class Matrix
    {
        private NUM[][] item { get; }
        public int Height { get; }
        public int Width { get; }
        //単位元
        public static NUM e { get; set; }
        //add,mulは環をなす必要がある
        public static Func<NUM, NUM, NUM> add { get; set; }
        public static Func<NUM, NUM, NUM> mul { get; set; }
        static Matrix()
        {
            const int mod = 1000000007; e = 1;
            add = (a, b) => a + b >= mod ? a + b - mod : a + b; mul = (a, b) => (a * b) % mod;
        }
        public NUM[] this[int i]
        { get { return item[i]; } set { item[i] = value; } }
        public NUM this[int i1, int i2]
        {
            get { return item[i1][i2]; }
            set { item[i1][i2] = value; }
        }
        public Matrix(int size) : this(size, size) { }
        public Matrix(int height, int width)
        {
            Height = height;
            Width = width;
            item = Enumerable.Repeat(0, height).Select(_ => new NUM[width]).ToArray();
        }
        //単位行列
        private static Matrix E(int size)
        {
            var tm = new Matrix(size, size);
            for (var i = 0; i < size; i++)
                tm[i, i] = e;
            return tm;
        }
        /// <summary>
        /// 転置行列を求めます
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix Trans(Matrix m)
        {
            var n = m.Width; var p = m.Height;
            var tm = new Matrix(n, p);
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < p; j++)
                    tm[i, j] = m[j, i];
            }
            return tm;
        }
        private static NUM Dot(NUM[] ar1, NUM[] ar2)
        {
            var tm = default(NUM);
            for (var i = 0; i < ar1.Length; i++)
                tm = add(tm, mul(ar1[i], ar2[i]));
            return tm;
        }
        public static Matrix Add(Matrix m1, Matrix m2)
        {
            var tm = new Matrix(m1.Height, m1.Width);
            for (var i = 0; i < m1.Height; i++)
                for (var j = 0; j < m1.Width; j++)
                    tm[i, j] = add(m1[i, j], m2[i, j]);
            return tm;
        }
        public static NUM[] Mul(Matrix m, NUM[] ar)
            => Enumerable.Range(0, m.Height).Select(v => Dot(m[v], ar)).ToArray();
        /// <summary>
        /// 行列同士の積を求めます
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix Mul(Matrix m1, Matrix m2)
        {
            var tr = Trans(m2);
            var tm = new Matrix(m1.Height, m2.Width);
            for (var i = 0; i < m1.Height; i++)
                tm[i] = Mul(tr, m1[i]);
            return tm;
        }
        public static Matrix operator +(Matrix l, Matrix r)
            => Add(l, r);
        public static Matrix operator *(Matrix l, Matrix r)
            => Mul(l, r);
        public static NUM[] operator *(Matrix l, NUM[] r)
            => Mul(l, r);
        /// <summary>
        /// 行列累乗
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Matrix Pow(long n)
        {
            if (n == 0) return E(Height);
            var tm = Pow(n / 2);
            if (n % 2 == 0) return Mul(tm, tm);
            else return Mul(Mul(tm, tm), this);
        }
    }
}