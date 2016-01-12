using System;

namespace Jmelosegui.Windows.Service.Native
{
    /// <summary>
    /// Describes service type flags.
    /// </summary>
    [Flags]
    public enum ServiceType : uint
    {
        /// <summary>
        /// Driver service.
        /// </summary>
        SERVICE_KERNEL_DRIVER = 0x00000001,

        /// <summary>
        /// File system driver service.
        /// </summary>
        SERVICE_FILE_SYSTEM_DRIVER = 0x00000002,

        /// <summary>
        /// Services of type <see cref="SERVICE_KERNEL_DRIVER"/> and <see cref="SERVICE_FILE_SYSTEM_DRIVER"/>.
        /// </summary>
        SERVICE_DRIVER = SERVICE_KERNEL_DRIVER | SERVICE_FILE_SYSTEM_DRIVER,

        /// <summary>
        /// Reserved.
        /// </summary>
        SERVICE_ADAPTER = 0x00000004,

        /// <summary>
        /// Reserved.
        /// </summary>
        SERVICE_RECOGNIZER_DRIVER = 0x00000008,

        /// <summary>
        /// Service that runs in its own process.
        /// </summary>
        SERVICE_WIN32_OWN_PROCESS = 0x00000010,

        /// <summary>
        /// Service that shares a process with other services.
        /// </summary>
        SERVICE_WIN32_SHARE_PROCESS = 0x00000020,

        /// <summary>
        /// Services of type <see cref="SERVICE_WIN32_OWN_PROCESS"/> and <see cref="SERVICE_WIN32_SHARE_PROCESS"/>.
        /// </summary>
        SERVICE_WIN32 = SERVICE_WIN32_OWN_PROCESS | SERVICE_WIN32_SHARE_PROCESS,

        /// <summary>
        /// The service can interact with the desktop.
        /// </summary>
        SERVICE_INTERACTIVE_PROCESS = 0x00000100
    }
}