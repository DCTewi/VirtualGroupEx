namespace VirtualGroupEx.Server.Data
{
    public class ActionWaiter
    {
        private bool waiting = false;

        public bool CantInvoke
        {
            get
            {
                return !QueryForInvoke();
            }
        }

        public bool QueryForInvoke()
        {
            if (waiting) return false;

            return waiting = true;
        }

        public void EndInvoke()
        {
            waiting = false;
        }
    }
}
