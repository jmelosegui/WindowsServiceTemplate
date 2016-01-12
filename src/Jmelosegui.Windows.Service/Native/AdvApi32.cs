using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jmelosegui.Windows.Service.Native
{
    class AdvApi32
    {
        [DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeServiceHandle OpenSCManager(string lpMachineName, string lpDatabaseName, ServiceManagerAccess dwDesiredAccess);
        
        [DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeServiceHandle CreateService(SafeServiceHandle hSCManager, string lpServiceName, string lpDisplayName, ServiceAccess dwDesiredAccess, ServiceType dwServiceType, ServiceStartType dwStartType, ServiceErrorControl dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

        [DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static unsafe extern bool ChangeServiceConfig2(SafeServiceHandle hService, ServiceInfoLevel dwInfoLevel, void* lpInfo);

        [DllImport(nameof(AdvApi32), SetLastError = true)]
        public static extern bool CloseServiceHandle(IntPtr hSCObject);

        [DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeServiceHandle OpenService(SafeServiceHandle hSCManager, string lpServiceName, ServiceAccess dwDesiredAccess);

        [DllImport(nameof(AdvApi32), SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteService(SafeServiceHandle hService);

        public static unsafe void CreateService(string lpBinaryPathName, string lpServiceName, string lpDisplayName, string lpDescription, string lpServiceStartName, string lpPassword)
        {
            if (string.IsNullOrWhiteSpace(lpBinaryPathName))
            {
                throw new ArgumentException("Binary path name must not be null nor empty", nameof(lpBinaryPathName));
            }

            if (string.IsNullOrWhiteSpace(lpServiceName))
            {
                throw new ArgumentException("Service name must not be null nor empty", nameof(lpServiceName));
            }

            using (SafeServiceHandle scmHandle = OpenSCManager(null, null, ServiceManagerAccess.SC_MANAGER_CREATE_SERVICE))
            {
                if (scmHandle.IsInvalid)
                {
                    throw new Win32Exception();
                }

                SafeServiceHandle svcHandle = CreateService(
                    scmHandle,
                    lpServiceName,
                    lpDisplayName,
                    ServiceAccess.SERVICE_ALL_ACCESS,
                    ServiceType.SERVICE_WIN32_OWN_PROCESS,
                    ServiceStartType.SERVICE_DEMAND_START,
                    ServiceErrorControl.SERVICE_ERROR_NORMAL,
                    lpBinaryPathName,
                    null,
                    0,
                    null,
                    lpServiceStartName,
                    lpPassword);

                using (svcHandle)
                {
                    if (svcHandle.IsInvalid)
                    {
                        throw new Win32Exception();
                    }

                    ServiceDescription descriptionStruct = new ServiceDescription
                    {
                        lpDescription = lpDescription
                    };

                    fixed (void* lpInfo = new byte[Marshal.SizeOf(descriptionStruct)])
                    {
                        Marshal.StructureToPtr(descriptionStruct, new IntPtr(lpInfo), false);
                        if (!ChangeServiceConfig2(svcHandle, ServiceInfoLevel.SERVICE_CONFIG_DESCRIPTION, lpInfo))
                        {
                            throw new Win32Exception();
                        }

                        Marshal.DestroyStructure(new IntPtr(lpInfo), typeof(ServiceDescription));
                    }
                }
            }
        }

        public static void DeleteService(string lpServiceName)
        {
            if (string.IsNullOrWhiteSpace(lpServiceName))
            {
                throw new ArgumentException("Service name must not be null nor empty", nameof(lpServiceName));
            }

            using (SafeServiceHandle scmHandle = OpenSCManager(null, null, ServiceManagerAccess.GenericWrite))
            {
                if (scmHandle.IsInvalid)
                {
                    throw new Win32Exception();
                }

                using (SafeServiceHandle svcHandle = OpenService(scmHandle, lpServiceName, ServiceAccess.Delete))
                {
                    if (svcHandle.IsInvalid)
                    {
                        throw new Win32Exception();
                    }

                    if (!DeleteService(svcHandle))
                    {
                        throw new Win32Exception();
                    }
                }
            }
        }
    }
}