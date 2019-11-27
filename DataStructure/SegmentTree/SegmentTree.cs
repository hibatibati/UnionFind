﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class SegmentTree<T>
{
    protected readonly T[] item;
    protected readonly int size;
    protected readonly Func<T, T, T> merge;
    protected readonly Func<T, T, T> update;
    protected readonly T idT;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int Parent(int i)
        => (i - 1) >> 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int Left(int i)
        => (i << 1) + 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int Right(int i)
        => (i + 1) << 1;
    public T this[int i]
    {
        get { return item[i + size - 1]; }
        set { item[i + size - 1] = value; }
    }

    public SegmentTree(int N, T idT, Func<T, T, T> merge, Func<T, T, T> update = null)
    {
        this.merge = merge;
        this.size = 1;
        this.idT = idT;
        this.update = update ?? ((T val1, T val2) => val2);
        while (size < N)
            size <<= 1;
        item = new T[2 * this.size - 1];
        for (var i = 0; i < 2 * size - 1; i++)
            item[i] = idT;
    }

    public void Update(int index, T value)
    {
        index += size - 1;
        item[index] = update(item[index], value);
        while (index > 0)
        {
            index = Parent(index);
            item[index] = merge(item[Left(index)], item[Right(index)]);
        }
    }

    public void Build()
    {
        for (int i = size - 2; i >= 0; i--)
            item[i] = merge(item[Left(i)], item[Right(i)]);
    }

    public virtual T Query(int left, int right, int k = 0, int l = 0, int r = -1)
    {
        if (r == -1) r = size;
        if (r <= left || right <= l) return idT;
        if (left <= l && r <= right) return item[k];
        else
            return merge(Query(left, right, Left(k), l, (l + r) >> 1), Query(left, right, Right(k), (l + r) >> 1, r));
    }

    /// <summary>
    /// check(func(item[st]...item[i]))がtrueとなる最小のi
    /// </summary>
    public int Find(int st, Func<T, bool> check)
    {
        var x = idT;
        return Find(st, check, ref x, 0, 0, size);
    }
    private int Find(int st, Func<T, bool> check, ref T x, int k, int l, int r)
    {
        if (l + 1 == r)
        { x = merge(x, item[k]); return check(x) ? k - size + 1 : -1; }
        var m = (l + r) >> 1;
        if (m <= st) return Find(st, check, ref x, Right(k), m, r);
        if (st <= l && !check(merge(x, item[k])))
        { x = merge(x, item[k]); return -1; }
        var xl = Find(st, check, ref x, Left(k), l, m);
        if (xl >= 0) return xl;
        return Find(st, check, ref x, Right(k), m, r);
    }
}