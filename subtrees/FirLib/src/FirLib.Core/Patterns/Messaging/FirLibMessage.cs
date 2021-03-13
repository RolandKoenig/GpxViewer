using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace FirLib.Core.Patterns.Messaging
{
    /// <summary>
    /// Base class of all messages sent and received through ApplicationMessenger class.
    /// </summary>
    public class FirLibMessage
    {
        /// <summary>
        /// Gets a list containing all target messengers for message routing.
        /// An empty list means that no routing logic applies.
        /// </summary>
        public string[] GetAsyncRoutingTargetMessengers()
        {
            return GetAsyncRoutingTargetMessengersOfMessageType(this.GetType());
        }

        /// <summary>
        /// Gets a list containing all target messengers for message routing.
        /// An empty list means that no routing logic applies.
        /// </summary>
        /// <param name="messageType">The type of the message.</param>
        public static string[] GetAsyncRoutingTargetMessengersOfMessageType(Type messageType)
        {
            if (messageType.GetTypeInfo().GetCustomAttribute(typeof(MessageAsyncRoutingTargetsAttribute)) is
                MessageAsyncRoutingTargetsAttribute routingAttrib)
            {
                return routingAttrib.AsyncTargetMessengers;
            }
            return new string[0];
        }

        /// <summary>
        /// Gets a list containing all possible source messengers of this message.
        /// An empty list means that every messenger can fire this message
        /// </summary>
        public string[] GetPossibleSourceMessengers()
        {
            return GetPossibleSourceMessengersOfMessageType(this.GetType());
        }

        /// <summary>
        /// Gets a list containing all possible source messengers of this message.
        /// An empty list means that every messenger can fire this message
        /// </summary>
        /// <param name="messageType">The type of the message.</param>
        public static string[] GetPossibleSourceMessengersOfMessageType(Type messageType)
        {
            if (messageType.GetTypeInfo().GetCustomAttribute(typeof(MessagePossibleSourceAttribute)) is
                MessagePossibleSourceAttribute routingAttrib)
            {
                return routingAttrib.PossibleSourceMessengers;
            }
            return new string[0];
        }
    }
}
