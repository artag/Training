﻿using System;
using Xunit;

namespace Immutability.Tests
{
    public class AuditManagerTests
    {
        [Fact]
        public void AddRecord_adds_a_record_to_an_existing_file_if_not_overflowed()
        {
            var manager = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
            });

            var action = manager.AddRecord(file, "Jane Doe", new DateTime(2016, 4, 6, 17, 0, 0));

            Assert.Equal(ActionType.Update, action.Type);
            Assert.Equal("Audit_1.txt", action.FileName);
            Assert.Equal(new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
                "2;Jane Doe;2016-04-06T17:00:00"
            }, action.Content);
        }

        [Fact]
        public void AddRecord_add_a_record_to_a_new_file_if_overflowed()
        {
            var manager = new AuditManager(3);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
                "2;Jane Doe;2016-04-06T16:40:00",
                "3;Jack Rich;2016-04-06T17:00:00"
            });

            var action = manager.AddRecord(file, "Tom Tomson", new DateTime(2016, 4, 6, 17, 30, 0));

            Assert.Equal(ActionType.Create, action.Type);
            Assert.Equal("Audit_2.txt", action.FileName);
            Assert.Equal(new[]
            {
                "1;Tom Tomson;2016-04-06T17:30:00"
            }, action.Content);
        }
    }
}
