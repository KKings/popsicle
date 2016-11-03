
namespace KKings.Foundation.Popsicle.Events
{
    using System;

    public interface ITrackingEvent
    {
        DateTime DateTime { get; set; }

        Guid DefinitionId { get; set; }

        string Data { get; set; }

        string DataKey { get; set; }

        string Text { get; set; }
    }
}