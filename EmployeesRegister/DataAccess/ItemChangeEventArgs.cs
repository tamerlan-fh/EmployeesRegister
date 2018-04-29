using System;

namespace EmployeesRegister.DataAccess
{
    class ItemChangedEventArgs<T> : EventArgs
    {
        public ItemChangedEventArgs(T item, ActionType action)
        {
            Item = item;
            Action = action;
        }

        public T Item { get; }

        public ActionType Action { get; }

        public enum ActionType
        {
            Add,
            Update,
            Remove
        }
    }
}
