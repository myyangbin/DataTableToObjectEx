﻿namespace DataTableToObjectEx.Tests.Data
{
    using System;

    public class FlatData
    {
        public Guid? Id { get; set; }

        public DateTime? Date { get; set; }

        public Enumeration? Enum { get; set; }

        public string Text { get; set; }

        public int[] Array { get; set; }
    }
}
