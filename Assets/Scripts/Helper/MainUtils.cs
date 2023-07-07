namespace Helper
{
    public class MainUtils
    {
        #region dllimport

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif

        #endregion
        
        public static bool IsOnMobile()
        {
            bool isMobile = false;
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();
#endif
            return isMobile;
        }
    }
}