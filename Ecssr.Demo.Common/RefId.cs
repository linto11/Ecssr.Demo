using System;

namespace Ecssr.Demo.Common
{
    public interface IRefId
    {
        string Id { get; }
    }

    public class RefId : IRefId
    {
        Guid _guid;
        public RefId() : this(Guid.NewGuid()) { }
        public RefId(Guid guid)
        {
            _guid = guid;
        }

        public string Id => _guid.ToString();
    }
}
