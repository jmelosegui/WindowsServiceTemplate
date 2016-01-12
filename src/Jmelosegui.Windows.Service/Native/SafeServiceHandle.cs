using System;
using System.Runtime.InteropServices;

namespace Jmelosegui.Windows.Service.Native
{
    /// <summary>
    /// Represents a preparsed data handle created by
    /// <see cref="AdvApi32.OpenSCManager(string,string,ServiceManagerAccess)"/> or <see cref="AdvApi32.OpenService(SafeServiceHandle,string,ServiceAccess)"/>
    /// that can be closed with <see cref="AdvApi32.CloseServiceHandle"/>.
    /// </summary>
    internal class SafeServiceHandle : SafeHandle
    {
        /// <summary>
        /// A handle that may be used in place of <see cref="IntPtr.Zero"/>.
        /// </summary>
        public static readonly SafeServiceHandle Null = new SafeServiceHandle();

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeServiceHandle"/> class.
        /// </summary>
        public SafeServiceHandle()
            : base(IntPtr.Zero, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeServiceHandle"/> class.
        /// </summary>
        /// <param name="preexistingHandle">An object that represents the pre-existing handle to use.</param>
        /// <param name="ownsHandle"><see langword="true"/> to reliably release the handle during the finalization
        /// phase; <see langword="false"/> to prevent reliable release.</param>
        public SafeServiceHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(IntPtr.Zero, ownsHandle)
        {
            this.SetHandle(preexistingHandle);
        }

        /// <inheritdoc />
        public override bool IsInvalid => this.handle == IntPtr.Zero;

        /// <inheritdoc />
        protected override bool ReleaseHandle() => AdvApi32.CloseServiceHandle(this.handle);
    }
}