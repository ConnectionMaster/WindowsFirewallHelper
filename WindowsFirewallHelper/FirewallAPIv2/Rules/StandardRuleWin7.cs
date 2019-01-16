﻿using System;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2.Rules
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule in Windows 7 and above
    /// </summary>
    public class StandardRuleWin7 : StandardRule, IEquatable<StandardRuleWin7>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin7(string name, string filename, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles)
            : base(name, filename, action, direction, profiles)
        {
            if (UnderlyingObjectV2 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin7(string name, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles) : base(name, action, direction, profiles)
        {
            if (UnderlyingObjectV2 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        /// <summary>
        ///     Creates a new port rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin7(string name, ushort port, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles)
            : base(name, port, action, direction, profiles)
        {
            if (UnderlyingObjectV2 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        internal StandardRuleWin7(INetFwRule2 rule) : base(rule)
        {
        }

        /// <summary>
        ///     Gets or sets the behavior of this rule about the Edge Traversal
        /// </summary>
        public EdgeTraversalAction EdgeTraversalOptions
        {
            get
            {
                if (!Enum.IsDefined(typeof(EdgeTraversalAction), UnderlyingObjectV2.EdgeTraversalOptions))
                    throw new NotSupportedException();
                return (EdgeTraversalAction) UnderlyingObjectV2.EdgeTraversalOptions;
            }
            set
            {
                if (!Enum.IsDefined(typeof(EdgeTraversalAction), value))
                    throw new NotSupportedException();
                UnderlyingObjectV2.EdgeTraversalOptions = (int) value;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public new static bool IsSupported => Environment.OSVersion.Version >= new Version(6, 1);

        private INetFwRule2 UnderlyingObjectV2 => UnderlyingObject as INetFwRule2;

        /// <inheritdoc />
        public bool Equals(StandardRuleWin7 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!base.Equals(other)) return false;
            return UnderlyingObjectV2.EdgeTraversalOptions == other.UnderlyingObjectV2.EdgeTraversalOptions;
        }

        /// <inheritdoc />
        public static bool operator ==(StandardRuleWin7 left, StandardRuleWin7 right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(StandardRuleWin7 left, StandardRuleWin7 right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StandardRuleWin7);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode*467 + UnderlyingObjectV2.EdgeTraversalOptions;
                return hashCode;
            }
        }
    }
}