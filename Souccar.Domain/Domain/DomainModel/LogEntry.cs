using System;

namespace Souccar.Domain.DomainModel
{
    [Serializable]
    public class LogEntry : Entity
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        protected LogEntry()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">User who modified task</param>
        /// <param name="message">Message</param>
        public LogEntry(string userName, string message)
        {
            UserName = userName;
            Message = message;
            Created = DateTime.Now;
        }

        /// <summary>
        /// User who modified the task 
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Date and time of creation
        /// </summary>
        public virtual DateTime Created { get; set; }
    }
}