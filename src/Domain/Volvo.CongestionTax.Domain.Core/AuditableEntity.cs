﻿using System;

namespace Volvo.CongestionTax.Domain.Core
{
    public abstract class AuditableEntity
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }
    }

    public abstract class AuditableEntity<TKey> : AuditableEntity
    {
        public TKey Id { get; set; }
    }
}