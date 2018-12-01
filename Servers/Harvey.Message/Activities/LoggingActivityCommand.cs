using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Activities
{
    public interface LoggingActivityCommand
    {
        string UserId { get; }
        int ActionAreaPath { get; }
        int ActionType { get; }
        string Description { get; }
        string Comment { get; }
        string CreatedByName { get; }
        string Value { get; }
    }
}
