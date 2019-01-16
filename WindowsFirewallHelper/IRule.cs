﻿namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Defines expected properties of a firewall rule
    /// </summary>
    public interface IRule
    {
        /// <summary>
        ///     Gets or sets the action that the rules defines
        /// </summary>
        FirewallAction Action { get; set; }

        /// <summary>
        ///     Gets or sets the data direction that the rule applies to
        /// </summary>
        FirewallDirection Direction { get; set; }

        /// <summary>
        ///     Gets or sets a Boolean value indicating if this rule is active
        /// </summary>
        bool IsEnable { get; set; }

        /// <summary>
        ///     Gets or sets the local addresses that the rule applies to
        /// </summary>
        IAddress[] LocalAddresses { get; set; }

        /// <summary>
        ///     Gets or sets the local ports that the rule applies to
        /// </summary>
        ushort[] LocalPorts { get; set; }

        /// <summary>
        ///     Gets or sets the type of local ports that the rules applies to
        /// </summary>
        FirewallPortType LocalPortType { get; set; }

        /// <summary>
        ///     Gets or sets the name of the rule
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets or sets the profiles that this rule belongs to
        /// </summary>
        FirewallProfiles Profiles { get; set; }

        /// <summary>
        ///     Gets or sets the protocol that the rule applies to
        /// </summary>
        FirewallProtocol Protocol { get; set; }

        /// <summary>
        ///     Gets or sets the remote addresses that the rule applies to
        /// </summary>
        IAddress[] RemoteAddresses { get; set; }

        /// <summary>
        ///     Gets or sets the remote ports that the rule applies to
        /// </summary>
        ushort[] RemotePorts { get; set; }

        /// <summary>
        ///     Gets or sets the scope that the rule applies to
        /// </summary>
        FirewallScope Scope { get; set; }
    }
}