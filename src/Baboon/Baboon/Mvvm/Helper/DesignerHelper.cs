using System.ComponentModel;
using System.Windows;

namespace Baboon.Mvvm
{
    public static class DesignerHelper
    {
        private static bool m_isValueAlreadyValidated = default;


        /* 项目“Baboon (net6.0-windows)”的未合并的更改
        在此之前:
                private static bool m_isInDesignMode = default;


                public static bool IsInDesignMode => IsCurrentAppInDebugMode();
        在此之后:
                private static bool m_isInDesignMode = default;


                public static bool IsInDesignMode => IsCurrentAppInDebugMode();
        */
        private static bool m_isInDesignMode = default;


        public static bool IsInDesignMode => IsCurrentAppInDebugMode();


        public static bool IsDebugging => System.Diagnostics.Debugger.IsAttached;

        private static bool IsCurrentAppInDebugMode()
        {
            if (m_isValueAlreadyValidated)
            {
                return m_isInDesignMode;
            }

            m_isInDesignMode = (bool)(
                DesignerProperties.IsInDesignModeProperty
                    .GetMetadata(typeof(DependencyObject))
                    ?.DefaultValue ?? false
            );

            m_isValueAlreadyValidated = true;

            return m_isInDesignMode;
        }
    }
}
