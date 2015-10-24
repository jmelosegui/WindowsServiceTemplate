using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jmelosegui.Windows.Service
{
    class ServiceInstaller
    {
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpScdb, int scParameter);

        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr scHandle, string lpSvcName, string lpDisplayName, int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName, string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll")]
        public static extern Boolean ChangeServiceConfig2(IntPtr schandle, int dwInfoLevel, IntPtr lpInfo);

        [DllImport("advapi32.dll")]
        public static extern IntPtr CloseServiceHandle(IntPtr schandle);

        [DllImport("advapi32.dll")]
        public static extern IntPtr StartService(IntPtr svhandle, int dwNumServiceArgs, string lpServiceArgVectors);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr schandle, string lpSvcName, int dwNumServiceArgs);

        [DllImport("advapi32.dll")]
        public static extern IntPtr DeleteService(IntPtr svhandle);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        public void Install(string servicePath, string serviceName, string serviceDisplayName, string serviceDescription)
        {
            int SC_MANAGER_CREATE_SERVICE = 0x0002;
            int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_AUTO_START				= 0x00000002;
            int SERVICE_DEMAND_START = 0x00000003;
            int SERVICE_ERROR_NORMAL = 0x00000001;

            int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            int SERVICE_QUERY_CONFIG = 0x0001;
            int SERVICE_CHANGE_CONFIG = 0x0002;
            int SERVICE_QUERY_STATUS = 0x0004;
            int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            int SERVICE_START = 0x0010;
            int SERVICE_STOP = 0x0020;
            int SERVICE_PAUSE_CONTINUE = 0x0040;
            int SERVICE_INTERROGATE = 0x0080;
            int SERVICE_USER_DEFINED_CONTROL = 0x0100;

            int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                SERVICE_QUERY_CONFIG |
                SERVICE_CHANGE_CONFIG |
                SERVICE_QUERY_STATUS |
                SERVICE_ENUMERATE_DEPENDENTS |
                SERVICE_START |
                SERVICE_STOP |
                SERVICE_PAUSE_CONTINUE |
                SERVICE_INTERROGATE |
                SERVICE_USER_DEFINED_CONTROL);

            IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);

            if (scmHandle == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            IntPtr svcHandle = CreateService(scmHandle,
                                                serviceName,
                                                serviceDisplayName,
                                                SERVICE_ALL_ACCESS,
                                                SERVICE_WIN32_OWN_PROCESS,
                                                SERVICE_DEMAND_START,
                                                SERVICE_ERROR_NORMAL,
                                                servicePath,
                                                null, 0, null,
                                                null, null);

            if (svcHandle == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            int SERVICE_CONFIG_DESCRIPTION = 0x0001;

            ServiceDescription descriptionStruct = new ServiceDescription
            {
                Description = serviceDescription
            };
            
            IntPtr lpInfo = Marshal.AllocHGlobal(Marshal.SizeOf(descriptionStruct));

            if (lpInfo == IntPtr.Zero)
            {
                throw new Exception("Error setting service description", new Win32Exception(GetLastError()));
            }

            Marshal.StructureToPtr(descriptionStruct, lpInfo, false);

            if (!ChangeServiceConfig2(svcHandle, SERVICE_CONFIG_DESCRIPTION, lpInfo))
            {
                Marshal.FreeHGlobal(lpInfo);
                throw new Exception("Error setting service description", new Win32Exception(GetLastError()));
            }

            Marshal.FreeHGlobal(lpInfo);

            if (svcHandle == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            if (CloseServiceHandle(svcHandle) == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

        }

        public void Uninstall(string serviceName)
        {
            const int genericWrite = 0x40000000;
            const int delete = 0x10000;

            var scmHandle = OpenSCManager(null, null, genericWrite);

            if (scmHandle == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            var svcHandle = OpenService(scmHandle, serviceName, delete);
            if (svcHandle == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            if (DeleteService(svcHandle) == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }

            if (CloseServiceHandle(scmHandle) == IntPtr.Zero)
            {
                throw new Win32Exception(GetLastError());
            }
        }
    }

    struct ServiceDescription
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String Description;
    }
}