﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Events;

    public class ProductCreatedEvent : DomainEvent
    {
        public ProductCreatedEvent(Product item)
        {
            Item = item;
        }

        public Product Item { get; }
    }

