namespace HidLibrary
{
#if !NETCOREAPP1_0_OR_GREATER
    // data provided to delegate BeginInvoke callbacks
    // no longer needed for .NET Core and later (includes .NET 5 and above) since BeginInvoke and EndInvoke delegate
    // calls are not supported, and Task<> is used instead

    public class HidAsyncState
    {
        private readonly object _callerDelegate;
        private readonly object _callbackDelegate;

        public HidAsyncState(object callerDelegate, object callbackDelegate)
        {
            _callerDelegate = callerDelegate;
            _callbackDelegate = callbackDelegate;
        }

        public object CallerDelegate { get { return _callerDelegate; } }
        public object CallbackDelegate { get { return _callbackDelegate; } }
    }
#endif
}
