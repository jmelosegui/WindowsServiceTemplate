using System.Runtime.InteropServices;

namespace Jmelosegui.Windows.Service.Native
{
    /// <summary>
    /// Contains a service description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceDescription
    {
        /// <summary>
        /// The description of the service. If this member is NULL, the description remains unchanged.
        /// If this value is an empty string (""), the current description is deleted.
        /// The service description must not exceed the size of a registry value of type REG_SZ.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpDescription;
    }
}