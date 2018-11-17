using System;

namespace ProductBacklog.Interfaces
{
    public interface IClosable
    {
        Action<bool> OnClose { get; set; }
    }
}
