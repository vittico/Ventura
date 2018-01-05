﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ventura
{
    /// <summary>
    /// Indicates type of entropy sources used to 
    /// reseed the generator. 
    /// Local: only sources from the local system used
    /// Remote: only remote sources (e.g RemoteQUantumRngExtractor) used
    /// Full: both types used
    /// </summary>
    public enum ReseedEntropySources
    {
        Local,
        Remote,
        Full = Local | Remote
    }

    public enum Cipher
    {
        Aes,
        TwoFish,
        BlowFish,
        Serpent
    }

    public enum Output
    {
        Byte,
        Int32,
        String,
        Hex
    }

    public class Constants
    {
        /// <summary>
        /// 256-bit key size
        /// </summary>
        public const int KeyBlockSize = 32; 

        /// <summary>
        /// Size of each cipher block generated by the encryptor
        /// 128 bits according to spec
        /// </summary>
        public const int CipherBlockSize = 16; 

        /// <summary>
        /// Maximum aamount of pseudorandom data generated
        /// before the state key changes
        /// </summary>
        public const int MaximumRequestSizeForStateKey = 1048576;

        /// <summary>
        /// Maximum anount of entropy data in bytes
        /// </summary>
        public const int MaximumEventSize = 32;

        /// <summary>
        /// The spec suggests a minimum of 128 bits of entropic data
        //  is needed for an attacker to lose track of the generator state.
        /// We set it to ten times (160 bytes) which means each pool should hold
        /// a minimum of five events before a reseed is triggered.
        /// </summary>
        public const int MinimumPoolSize = 1280;

        public const int MaximumNumberOfSources = 255;
        public const int MaximumNumberOfPools = 32;
    }
}
