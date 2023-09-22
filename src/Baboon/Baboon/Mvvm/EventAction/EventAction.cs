using System;

namespace Baboon.Mvvm
{
    public class EventAction<TSender, TE> : IEventAction
    {
        private readonly Action<TSender, TE> m_action;

        public EventAction(string eventName, Action<TSender, TE> action)
        {
            this.EventName = eventName;
            this.m_action = action;
        }

        public string EventName { get; }

        private void Event(TSender sender, TE e)
        {
            this.m_action?.Invoke(sender, e);
        }
    }
}