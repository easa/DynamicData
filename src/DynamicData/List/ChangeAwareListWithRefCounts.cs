﻿// Copyright (c) 2011-2020 Roland Pheasant. All rights reserved.
// Roland Pheasant licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

using DynamicData.Kernel;
using DynamicData.List.Internal;

// ReSharper disable once CheckNamespace
namespace DynamicData;

internal class ChangeAwareListWithRefCounts<T> : ChangeAwareList<T>
{
    private readonly ReferenceCountTracker<T> _tracker = new();

    public override void Clear()
    {
        _tracker.Clear();
        base.Clear();
    }

    public override bool Contains(T item)
    {
        return _tracker.Contains(item);
    }

    protected override void InsertItem(int index, T item)
    {
        _tracker.Add(item);
        base.InsertItem(index, item);
    }

    protected override void OnInsertItems(int startIndex, IEnumerable<T> items)
    {
        items.ForEach(t => _tracker.Add(t));
    }

    protected override void OnRemoveItems(int startIndex, IEnumerable<T> items)
    {
        items.ForEach(t => _tracker.Remove(t));
    }

    protected override void OnSetItem(int index, T newItem, T oldItem)
    {
        _tracker.Remove(oldItem);
        _tracker.Add(newItem);
        base.OnSetItem(index, newItem, oldItem);
    }

    protected override void RemoveItem(int index, T item)
    {
        _tracker.Remove(item);
        base.RemoveItem(index, item);
    }
}
