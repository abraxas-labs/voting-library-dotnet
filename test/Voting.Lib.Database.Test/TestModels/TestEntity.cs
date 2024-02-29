// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Test.TestModels;

public class TestEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int Value { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((TestEntity)obj);
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, Name, Value);

    protected bool Equals(TestEntity other) => Id == other.Id && Name == other.Name && Value == other.Value;
}
