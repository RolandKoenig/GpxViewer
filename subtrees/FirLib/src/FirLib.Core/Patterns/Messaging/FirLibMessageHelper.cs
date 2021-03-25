using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using FirLib.Core.Checking;

namespace FirLib.Core.Patterns.Messaging
{
    public static class FirLibMessageHelper
    {
        public static bool ValidateMessageTypeAndValue<T>(T messageValue)
        {
            Type messageType = typeof(T);

            if(!ValidateMessageType(messageType))
            {
                return false;
            }

            if((messageType.IsClass) &&
               (messageValue == null))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateMessageType(Type messageType)
        {
            if (messageType.GetCustomAttribute<FirLibMessageAttribute>() == null)
            {
                return false;
            }
            return true;
        }

        public static void EnsureValidMessageTypeAndValue<T>(T messageValue)
        {
            Type messageType = typeof(T);

            EnsureValidMessageType(messageType);

            if((messageType.IsClass) &&
               (messageValue == null))
            {
                throw new FirLibCheckException(
                    $"Invalid message type {messageType.FullName}: Message value is null!");
            }
        }

        public static void EnsureValidMessageType(Type messageType)
        {
            if (messageType.GetCustomAttribute<FirLibMessageAttribute>() == null)
            {
                throw new FirLibCheckException(
                    $"Invalid message type {messageType.FullName}: Message types have to be market with FirLibMessageAttribute!");
            }
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
