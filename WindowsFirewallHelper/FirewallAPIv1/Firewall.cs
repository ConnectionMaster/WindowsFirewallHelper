﻿using System;
using System.Collections.Generic;
using WindowsFirewallHelper.FirewallAPIv1.Rules;
using WindowsFirewallHelper.Helpers;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    /// <summary>
    ///     Contains properties and methods of Windows Firewall v1
    /// </summary>
    public class Firewall : IFirewall
    {
        private static Firewall _instance;
        private static readonly object InstanceLock = new object();

        private readonly ActiveCollection<IRule> _rules = new ActiveCollection<IRule>();

        private Firewall()
        {
            if (Type.GetTypeFromProgID(@"HNetCfg.FwMgr", false) != null)
            {
                UnderlyingObject = (INetFwMgr) Activator.CreateInstance(Type.GetTypeFromProgID(@"HNetCfg.FwMgr", false));
                Profiles = new IProfile[]
                {
                    new FirewallProfile(
                        UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)),
                    new FirewallProfile(
                        UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD))
                };
            }
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static Firewall Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    return _instance ?? (_instance = new Firewall());
                }
            }
        }

        internal INetFwMgr UnderlyingObject { get; }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreateApplicationRule(FirewallProfiles profile, string name, FirewallAction action, string filename,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            if (!protocol.Equals(FirewallProtocol.Any))
                throw new FirewallAPIv1NotSupportedException();
            if (action != FirewallAction.Allow)
                throw new FirewallAPIv1NotSupportedException();
            return new ApplicationRule(name, filename, profile);
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreateApplicationRule(FirewallProfiles profile, string name, FirewallAction action, string filename)
        {
            return CreateApplicationRule(profile, name, action, filename, FirewallProtocol.Any);
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreateApplicationRule(FirewallProfiles profile, string name, string filename)
        {
            return CreateApplicationRule(profile, name, FirewallAction.Allow, filename);
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreatePortRule(FirewallProfiles profile, string name, FirewallAction action, ushort portNumber,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            if (action != FirewallAction.Allow)
                throw new FirewallAPIv1NotSupportedException();
            return new PortRule(name, portNumber, profile) {Protocol = protocol};
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreatePortRule(FirewallProfiles profile, string name, FirewallAction action, ushort portNumber)
        {
            return CreatePortRule(profile, name, action, portNumber, FirewallProtocol.Any);
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked setting is not supported with this class</exception>
        public IRule CreatePortRule(FirewallProfiles profile, string name, ushort portNumber)
        {
            return CreatePortRule(profile, name, FirewallAction.Allow, portNumber);
        }

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object implementing <see cref="IProfile" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked profile is not supported with this class</exception>
        public IProfile GetProfile(FirewallProfiles profile)
        {
            if (!IsSupported)
                throw new NotSupportedException();
            foreach (var p in Profiles)
                if (p.Type == profile)
                    return p;
            throw new FirewallAPIv1NotSupportedException();
        }

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object implementing <see cref="IProfile" /> interface or null if no firewall
        ///     profile is currently active
        /// </returns>
        /// <exception cref="FirewallAPIv1NotSupportedException">The asked profile is not supported with this class</exception>
        public IProfile GetProfile()
        {
            if (!IsSupported)
                throw new NotSupportedException();
            foreach (var p in Profiles)
                if (p.IsActive)
                    return p;
            return null;
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public bool IsSupported => UnderlyingObject != null;

        /// <summary>
        ///     Gets the name of the firewall
        /// </summary>
        public string Name => "Windows Firewall";

        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
        public IProfile[] Profiles { get; }

        /// <summary>
        ///     Gets the list of all registered rules of the firewall
        /// </summary>
        public IList<IRule> Rules
        {
            get
            {
                SyncRules();
                return _rules;
            }
        }

        private void RulesOnItemsModified(object sender, ActiveCollectionChangedEventArgs<IRule> e)
        {
            lock (_rules)
            {
                if (e.ActionType == ActiveCollectionChangeType.Added)
                {
                    if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Public))
                        throw new FirewallAPIv1NotSupportedException();
                    // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                    if (e.Item is ApplicationRule)
                    {
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Domain))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)
                                .AuthorizedApplications.Add(((ApplicationRule) e.Item).UnderlyingObject);
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Private))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD)
                                .AuthorizedApplications.Add(((ApplicationRule) e.Item).UnderlyingObject);
                    }
                    else if (e.Item is PortRule)
                    {
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Domain))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)
                                .GloballyOpenPorts.Add(((PortRule) e.Item).UnderlyingObject);
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Private))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD)
                                .GloballyOpenPorts.Add(((PortRule) e.Item).UnderlyingObject);
                    }
                }
                else if (e.ActionType == ActiveCollectionChangeType.Removed)
                {
                    if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Public))
                        throw new FirewallAPIv1NotSupportedException();
                    // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                    if (e.Item is ApplicationRule)
                    {
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Domain))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)
                                .AuthorizedApplications.Remove(
                                    ((ApplicationRule) e.Item).UnderlyingObject.ProcessImageFileName);
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Private))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD)
                                .AuthorizedApplications.Remove(
                                    ((ApplicationRule) e.Item).UnderlyingObject.ProcessImageFileName);
                    }
                    else if (e.Item is PortRule)
                    {
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Domain))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN)
                                .GloballyOpenPorts.Remove(((PortRule) e.Item).UnderlyingObject.Port,
                                    ((PortRule) e.Item).UnderlyingObject.Protocol);
                        if (EnumHelper.HasFlag(e.Item.Profiles, FirewallProfiles.Private))
                            UnderlyingObject.LocalPolicy.GetProfileByType(NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD)
                                .GloballyOpenPorts.Remove(((PortRule) e.Item).UnderlyingObject.Port,
                                    ((PortRule) e.Item).UnderlyingObject.Protocol);
                    }
                }
            }
            SyncRules();
        }

        private void SyncRules()
        {
            lock (_rules)
            {
                var rules = new List<IRule>();
                foreach (var profile in Profiles)
                {
                    var pro = profile as FirewallProfile;
                    if (pro != null)
                    {
                        foreach (var application in pro.UnderlyingObject.AuthorizedApplications)
                        {
                            var authorizedApplication = application as INetFwAuthorizedApplication;
                            if (authorizedApplication != null)
                                rules.Add(new ApplicationRule(authorizedApplication, pro.Type));
                        }
                        foreach (var port in pro.UnderlyingObject.GloballyOpenPorts)
                        {
                            var openPort = port as INetFwOpenPort;
                            if (openPort != null)
                                rules.Add(new PortRule(openPort, pro.Type));
                        }
                    }
                }
                _rules.ItemsModified -= RulesOnItemsModified;
                _rules.Sync(rules.ToArray());
                _rules.ItemsModified += RulesOnItemsModified;
            }
        }
    }
}